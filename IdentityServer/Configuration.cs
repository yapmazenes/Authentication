﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> { new IdentityResources.OpenId(), new IdentityResources.Profile() };
        }
        public static IEnumerable<ApiResource> GetApis() => new List<ApiResource> { new ApiResource("ApiOne"), new ApiResource("ApiTwo") };

        public static IEnumerable<Client> GetClients() => new List<Client> {
                                new Client { ClientId = "client_id" ,
                                             ClientSecrets = new List<Secret> { new Secret("client_secret".ToSha256())},
                                             AllowedGrantTypes =  GrantTypes.ClientCredentials,
                                             AllowedScopes  = {"ApiOne"}
                                           },
                                new Client { ClientId = "client_id_mvc" ,
                                             ClientSecrets = new List<Secret> { new Secret("client_secret_mvc".ToSha256())},
                                             AllowedGrantTypes =  GrantTypes.Code,
                                             AllowedScopes  = {"ApiOne","ApiTwo",IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile},
                                             RedirectUris ={ "https://localhost:44303/signin-oidc"},
                                             RequireConsent =false
                                           },
                                        };
    }
}
