﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Components.WebAssembly.Authentication;

/// <summary>
/// A <see cref="DelegatingHandler"/> that attaches access tokens to outgoing <see cref="HttpResponseMessage"/> instances.
/// Access tokens will only be added when the request URI is within one of the base addresses configured using
/// <see cref="ConfigureHandler(IEnumerable{string}, IEnumerable{string}, string)"/>.
/// </summary>
public class CustomAuthorizationMessageHandler : DelegatingHandler, IDisposable
{
    private readonly IAccessTokenProvider _provider;
    private readonly NavigationManager _navigation;
    private readonly AuthenticationStateChangedHandler? _authenticationStateChangedHandler;
    private AccessToken? _lastToken;
    private AuthenticationHeaderValue? _cachedHeader;
    private Uri[]? _authorizedUris;
    private string[]? _exceptionUris;
    private AccessTokenRequestOptions? _tokenOptions;

    /// <summary>
    /// Initializes a new instance of <see cref="CustomAuthorizationMessageHandler"/>.
    /// </summary>
    /// <param name="provider">The <see cref="IAccessTokenProvider"/> to use for provisioning tokens.</param>
    /// <param name="navigation">The <see cref="NavigationManager"/> to use for performing redirections.</param>
    public CustomAuthorizationMessageHandler(
        IAccessTokenProvider provider,
        NavigationManager navigation)
    {
        _provider = provider;
        _navigation = navigation;

        // Invalidate the cached _lastToken when the authentication state changes
        if (_provider is AuthenticationStateProvider authStateProvider)
        {
            _authenticationStateChangedHandler = _ => { _lastToken = null; };
            authStateProvider.AuthenticationStateChanged += _authenticationStateChangedHandler;
        }
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.Now;
        if (_authorizedUris == null)
        {
            throw new InvalidOperationException($"The '{nameof(CustomAuthorizationMessageHandler)}' is not configured. " +
                $"Call '{nameof(CustomAuthorizationMessageHandler.ConfigureHandler)}' and provide a list of endpoint urls to attach the token to.");
        }

        bool requestIsInException = false;
        foreach (var uri in _exceptionUris)
        {
            if (request.RequestUri?.AbsoluteUri.Contains(uri) ?? false)
            {
                requestIsInException = true;
            }
        }

        try
        {
            AccessTokenResult at = await _provider.RequestAccessToken();
            requestIsInException = at.Status == AccessTokenResultStatus.RequiresRedirect;
        }
        catch (Exception e)
        {

        }

        if (!requestIsInException && request.RequestUri != null && _authorizedUris.Any(uri => uri.IsBaseOf(request.RequestUri)))
        {
            if (_lastToken == null || now >= _lastToken.Expires.AddMinutes(-5))
            {
                var tokenResult = _tokenOptions != null ?
                    await _provider.RequestAccessToken(_tokenOptions) :
                    await _provider.RequestAccessToken();

                if (tokenResult.TryGetToken(out var token))
                {
                    _lastToken = token;
                    _cachedHeader = new AuthenticationHeaderValue("Bearer", _lastToken.Value);
                }
                else
                {
                    throw new AccessTokenNotAvailableException(_navigation, tokenResult, _tokenOptions?.Scopes);
                }
            }

            // We don't try to handle 401s and retry the request with a new token automatically since that would mean we need to copy the request
            // headers and buffer the body and we expect that the user instead handles the 401s. (Also, we can't really handle all 401s as we might
            // not be able to provision a token without user interaction).
            request.Headers.Authorization = _cachedHeader;
        }

        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _navigation.NavigateTo($"/error/{(int)response.StatusCode}");
            }
            return response;
        }
        catch (HttpRequestException)
        {
            _navigation.NavigateTo("/error/500");
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }

    /// <summary>
    /// Configures this handler to authorize outbound HTTP requests using an access token. The access token is only attached if at least one of
    /// <paramref name="authorizedUrls" /> is a base of <see cref="HttpRequestMessage.RequestUri" />.
    /// </summary>
    /// <param name="authorizedUrls">The base addresses of endpoint URLs to which the token will be attached.</param>
    /// <param name="scopes">The list of scopes to use when requesting an access token.</param>
    /// <param name="returnUrl">The return URL to use in case there is an issue provisioning the token and a redirection to the
    /// identity provider is necessary.
    /// </param>
    /// <returns>This <see cref="CustomAuthorizationMessageHandler"/>.</returns>
    public CustomAuthorizationMessageHandler ConfigureHandler(
        IEnumerable<string> authorizedUrls,
        IEnumerable<string> exceptionUrls,
        IEnumerable<string>? scopes = null,
        [StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null)
    {
        if (_authorizedUris != null)
        {
            throw new InvalidOperationException("Handler already configured.");
        }

        if (_exceptionUris != null)
        {
            throw new InvalidOperationException("Handler already configured.");
        }

        ArgumentNullException.ThrowIfNull(authorizedUrls);
        ArgumentNullException.ThrowIfNull(exceptionUrls);

        var uris = authorizedUrls.Select(uri => new Uri(uri, UriKind.Absolute)).ToArray();
        if (uris.Length == 0)
        {
            throw new ArgumentException("At least one URL must be configured.", nameof(authorizedUrls));
        }

        var excUris = exceptionUrls.ToArray();


        _authorizedUris = uris;
        _exceptionUris = excUris;
        var scopesList = scopes?.ToArray();
        if (scopesList != null || returnUrl != null)
        {
            _tokenOptions = new AccessTokenRequestOptions
            {
                Scopes = scopesList,
                ReturnUrl = returnUrl
            };
        }

        return this;
    }

    void IDisposable.Dispose()
    {
        if (_provider is AuthenticationStateProvider authStateProvider)
        {
            authStateProvider.AuthenticationStateChanged -= _authenticationStateChangedHandler;
        }
        Dispose(disposing: true);
    }
}
