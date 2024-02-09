using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServer.Configurations
{
    public class IdentityConfiguration
    {
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "movieClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"movieAPI"},
                    RequirePkce = false
                },
                new Client
                {
                    ClientId = "movie_mvc_client",
                    ClientName = "Movies MVC web App",
                    AllowedGrantTypes= GrantTypes.Hybrid,
                    RequirePkce = false,
                    AllowRememberConsent = false,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:7153/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:7153/signout-callback-oidc"
                    },
                    ClientSecrets = new List<Secret>
                    {
                         new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        "movieAPI",
                        "roles"
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("movieAPI", "Movie API")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("movieAPI", "Movie API")
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
           new IdentityResource[]
           {
               new IdentityResources.OpenId(),
               new IdentityResources.Profile(),
               new IdentityResources.Email(),
               new IdentityResources.Address(),
               new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string>() { "role" })
           };

        public static List<TestUser> TestUsers =>
           new List<TestUser>
           {
               new TestUser
               {
                   SubjectId = "4a27ed3c-e9a0-467d-b755-87bf0f205f7a",
                   Username = "oladayo",
                   Password = "oladayo",
                   Claims = new List<Claim>
                   {
                       new Claim(JwtClaimTypes.GivenName, "oladayo"),
                       new Claim(JwtClaimTypes.FamilyName, "ale")
                   }
               }
           };
    }
}
