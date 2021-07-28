using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> { new IdentityResources.OpenId()/*, new IdentityResources.Profile()*/,
            new IdentityResource {
                Name ="api.scope",
                UserClaims =
                {
                    "product_add"
                }

            } };
        }
        public static IEnumerable<ApiResource> GetApis() => new List<ApiResource> {
            new ApiResource("ApiOne", new string [] {"product_list" }),
            new ApiResource("ApiTwo") };

        public static IEnumerable<Client> GetClients() => new List<Client> {
                                new Client { ClientId = "client_id" ,
                                             ClientSecrets = new List<Secret> { new Secret("client_secret".ToSha256())},
                                             AllowedGrantTypes =  GrantTypes.ClientCredentials,
                                             AllowedScopes  = {"ApiOne"}
                                           },
                                new Client { ClientId = "client_id_mvc" ,
                                             ClientSecrets = new List<Secret> { new Secret("client_secret_mvc".ToSha256())},
                                             AllowedGrantTypes =  GrantTypes.Code,
                                             AllowedScopes  = {
                                                "ApiOne",
                                                "ApiTwo",
                                                IdentityServerConstants.StandardScopes.OpenId,
                                                /*IdentityServerConstants.StandardScopes.Profile,*/
                                                "api.scope"},

                                             RedirectUris ={ "https://localhost:44303/signin-oidc"},
                                             PostLogoutRedirectUris ={ "https://localhost:44303/Home/Index"},
                                             
                                             //We have to add records to the relevant table (PostLogoutRedirectUris) in the Database by associating the PostLogoutRedirectUri and the ClientId (e.g. clientIdMvc and client_id_js)

                                             //puts all the claims in the id token
                                             //AlwaysIncludeUserClaimsInIdToken=true,
                                             AllowOfflineAccess =true,
                                             RequireConsent =false
                                           },
                                new Client { ClientId = "client_id_js",
                                             RedirectUris = { "https://localhost:44394/home/signin"},
                                             PostLogoutRedirectUris ={ "https://localhost:44394/Home/Index"},

                                             AllowedCorsOrigins = { "https://localhost:44394" },
                                             AllowedGrantTypes =  GrantTypes.Implicit,
                                             AllowedScopes  = {
                                                IdentityServerConstants.StandardScopes.OpenId,
                                                "ApiOne",
                                                "ApiTwo",
                                                "api.scope"
                                            },
                                             AllowAccessTokensViaBrowser =true,
                                             RequireConsent=false
                                            }
                                        };
    }
}
