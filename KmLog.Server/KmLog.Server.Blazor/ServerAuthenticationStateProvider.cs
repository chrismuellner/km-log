﻿using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using KmLog.Server.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KmLog.Server.Blazor
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public ServerAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userInfo = await _httpClient.GetJsonAsync<UserInfoDto>("api/authentication");

            var identity = userInfo.IsAuthenticated
                ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userInfo.Name) }, "serverauth")
                : new ClaimsIdentity();

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
