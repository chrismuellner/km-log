using System;
using System.Windows;
using System.Threading.Tasks;
using Blazorise.Snackbar;
using KmLog.Server.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KmLog.Server.Blazor.Shared
{

    public partial class MainLayout : IErrorComponent
    {
        private Snackbar errorSnackbar;
        private string errorMessage;
        private Exception exception;

        [Inject]
        private AppState State { get; set; }

        [Inject]
        private ClipboardService Clipboard { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            if (authState.User.Identity.IsAuthenticated)
            {
                try
                {
                    await State.UpdateCars();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.StackTrace);
                }
            }
        }

        public void ShowError(string message, Exception e)
        {
            errorMessage = message;
            exception = e;
            errorSnackbar.Show();
        }

        private async void StackTraceToClipboard()
        {
            await Clipboard.WriteToClipboardAsync(exception.ToString());
        }
    }
}
