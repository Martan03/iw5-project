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
                profileIdentityResources.UserClaims.Add("role");

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
            new ("iw5api", [JwtClaimTypes.Role])
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
                    "https://localhost:5258/authentication/login-callback",
                    "https://localhost:7036/authentication/login-callback"
                ],
                PostLogoutRedirectUris = [
                    "https://app-iw5-2024-team-xzatloa00-web.azurewebsites.net/authentication/logout-callback",
                    "https://localhost:5258/authentication/logout-callback",
                    "https://localhost:7036/authentication/logout-callback"
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
                    "iw5api"
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