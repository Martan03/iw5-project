﻿using System.Security.Claims;
using IW5Forms.IdentityProvider.BL.Facades;
using IW5Forms.IdentityProvider.BL.Models;
using IW5Forms.IdentityProvider.BL.Facades.Interfaces;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IW5Forms.IdentityProvider.BL.Models.AppUser;

namespace IW5Forms.IdentityProvider.App.Services;

public class LocalAppUserProfileService : IProfileService
{
    private readonly IAppUserFacade appUserFacade;
    private readonly IAppUserClaimsFacade appUserClaimsFacade;

    public LocalAppUserProfileService(
        IAppUserFacade appUserFacade,
        IAppUserClaimsFacade appUserClaimsFacade)
    {
        this.appUserFacade = appUserFacade;
        this.appUserClaimsFacade = appUserClaimsFacade;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var subjectId = context.Subject.GetSubjectId();

        AppUserDetailModel? user;

        if (Guid.TryParse(subjectId, out var id))
        {
            user = await appUserFacade.GetUserByIdAsync(id);
        }
        else
        {
            user = await appUserFacade.GetUserByUserNameAsync(subjectId);
        }

        if (user is not null)
        {
            if (user is not null)
            {
                var appUserClaims = await appUserClaimsFacade.GetAppUserClaimsByUserIdAsync(user.Id);
                var claims = appUserClaims.Select(claim =>
                {
                    if (claim.ClaimType is not null
                        && claim.ClaimValue is not null)
                    {
                        return new Claim(claim.ClaimType, claim.ClaimValue);
                    }
                    return null;
                }).ToList();

                claims.Add(new Claim("username", user.UserName));
                context.AddRequestedClaims(claims);
            }
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
    }
}