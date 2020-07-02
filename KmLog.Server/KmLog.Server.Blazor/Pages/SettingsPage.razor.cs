using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Validation.Models;
using KmLog.Server.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KmLog.Server.Blazor.Pages
{
    public partial class SettingsPage
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private UserDto User { get; set; }

        private IEnumerable<GroupDto> Groups { get; set; }

        private GroupModel JoinGroup { get; set; } = new GroupModel();

        private GroupModel AddGroup { get; set; } = new GroupModel();

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            if (!authState.User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("api/authentication/signin", true);
            }

            try
            {
                User = await HttpClient.GetFromJsonAsync<UserDto>("api/user");
                Groups = await HttpClient.GetFromJsonAsync<IEnumerable<GroupDto>>("api/user/group");
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error loading settings");
            }
        }

        private async Task JoinFormSubmitted()
        {
            try
            {
                var group = new GroupDto
                {
                    Id = JoinGroup.Id,
                    Name = JoinGroup.Name
                };

                await HttpClient.PostAsJsonAsync("api/user/group", group);
                JoinGroup = new GroupModel();
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Could not join group!");
            }
        }

        private void UpdateGroupId(string groupId)
        {
            if (string.IsNullOrEmpty(groupId))
            {
                JoinGroup.Id = Guid.Empty;
            }
            else
            {
                JoinGroup.IdAsString = groupId;
            }
        }

        private async Task AddFormSubmitted()
        {
            try
            {
                var group = new GroupDto
                {
                    Name = AddGroup.Name
                };

                await HttpClient.PutAsJsonAsync("api/user/group", group);
                AddGroup = new GroupModel();
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Could not add group!");
            }
        }
    }
}
