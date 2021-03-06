using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Data;
using Microsoft.HockeyApp;
using PokemonGo_UWP.Utils;
using PokemonGo_UWP.ViewModels;
using PokemonGo_UWP.Views;
using Template10.Common;
using System;
using Windows.System.Display;

namespace PokemonGo_UWP
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki
    [Bindable]
    sealed partial class App : BootStrapper
    {
        /// <summary>
        ///     Locator instance
        /// </summary>
        public static ViewModelLocator ViewModelLocator;

        /// <summary>
        /// Used to prevent lockscreen while playing
        /// </summary>
        public static DisplayRequest DisplayRequest;

        public App()
        {
            InitializeComponent();

            // Forces the display to stay on while we play
            DisplayRequest = new DisplayRequest();
            DisplayRequest.RequestActive();            
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // If we have a phone contract, hide the status bar
            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                var statusBar = StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
            }
            // Get a static reference to viewmodel locator to use it within viewmodels
            ViewModelLocator = (ViewModelLocator) Current.Resources["Locator"];
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // Init HockeySDK
            if (!string.IsNullOrEmpty(ApplicationKeys.HockeyAppToken))
                HockeyClient.Current.Configure(ApplicationKeys.HockeyAppToken);
            await NavigationService.NavigateAsync(typeof(MainPage));
            if (!string.IsNullOrEmpty(SettingsService.Instance.PtcAuthToken))
            {
                // We have a stored token, let's go to game page 
                NavigationService.Navigate(typeof(GameMapPage));
                await ViewModelLocator.GameManagerViewModel.InitGame(true);
            }
            await Task.CompletedTask;
        }
    }
}