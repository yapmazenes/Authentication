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
                                             AllowedScopes  = {"ApiOne","ApiTwo",IdentityServerConstants.StandardScopes.OpenId,/*IdentityServerConstants.StandardScopes.Profile,*/"api.scope"},
                                             RedirectUris ={ "https://localhost:44303/signin-oidc"},
                                             
                                             //puts all the claims in the id token
                                             //AlwaysIncludeUserClaimsInIdToken=true,
                                             AllowOfflineAccess =true,
                                             RequireConsent =false
                                           },
                                new Client { ClientId = "client_id_js",
                                             RedirectUris = { "https://localhost:44394/home/signin"},
                                             AllowedCorsOrigins = { "https://localhost:44394" },
                                             AllowedGrantTypes =  GrantTypes.Implicit,
                                             AllowedScopes  = {"ApiOne",IdentityServerConstants.StandardScopes.OpenId},
                                             AllowAccessTokensViaBrowser =true,
                                             RequireConsent=false
                                            }
                                        };
    }
}
