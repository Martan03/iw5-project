using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IW5Forms.IdentityProvider.App
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources
        {
            get
            {
                var profileIdentityResources = new IdentityResources.Profile();
                profileIdentityResources.UserClaims.Add("username");

                return
                [
                    new IdentityResources.OpenId(),
                    profileIdentityResources
                ];
            }
        }

        public static IEnumerable<ApiResource> ApiResources =>
        [
            new ("formsclientaudience")
        ];

        public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ("formsapi", [JwtClaimTypes.Role])
        ];

        public static IEnumerable<Client> Clients =>
        [
            new()
            {
                ClientName = "Forms Client",
                ClientId = "formsclient",
                AllowOfflineAccess = true,
                RedirectUris =
                [
                    "https://oauth.pstmn.io/v1/callback",
                    "https://app-iw5-2024-team-xzatloa00-web.azurewebsites.net/authentication/login-callback",
                    "http://localhost:5258/authentication/login-callback"
                ],
                AllowedGrantTypes =
                [
                    GrantType.ClientCredentials,
                    GrantType.ResourceOwnerPassword,
                    GrantType.AuthorizationCode
                ],
                RequirePkce = true,
                AllowedScopes =
                [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "formsapi"
                ],
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                RequireClientSecret = false
            }
        ];
    }
}