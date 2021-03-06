﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PokemonGo_UWP.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPokestopPage : Page
    {
        public SearchPokestopPage()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                // Of course binding doesn't work so we need to manually setup height for animations
                ShowGatheredItemsMenuAnimation.From = GatheredItemsTranslateTransform.Y = ActualHeight;
            };
        }

        #region Overrides of Page

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SubscribeToCaptureEvents();
        }


        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            UnsubscribeToCaptureEvents();
        }

        #endregion

        #region Handlers

        private void SubscribeToCaptureEvents()
        {
            App.ViewModelLocator.GameManagerViewModel.SearchInCooldown += GameManagerViewModelOnSearchInCooldown;
            App.ViewModelLocator.GameManagerViewModel.SearchInventoryFull += GameManagerViewModelOnSearchInventoryFull;
            App.ViewModelLocator.GameManagerViewModel.SearchOutOfRange += GameManagerViewModelOnSearchOutOfRange;
            App.ViewModelLocator.GameManagerViewModel.SearchSuccess += GameManagerViewModelOnSearchSuccess;
            // Add also handlers to report which items the user gained after the animation
            SpinPokestopImage.Completed += (s, e) => ShowGatheredItemsMenu.Begin();
        }

        private void UnsubscribeToCaptureEvents()
        {
            App.ViewModelLocator.GameManagerViewModel.SearchInCooldown -= GameManagerViewModelOnSearchInCooldown;
            App.ViewModelLocator.GameManagerViewModel.SearchInventoryFull -= GameManagerViewModelOnSearchInventoryFull;
            App.ViewModelLocator.GameManagerViewModel.SearchOutOfRange -= GameManagerViewModelOnSearchOutOfRange;
            App.ViewModelLocator.GameManagerViewModel.SearchSuccess -= GameManagerViewModelOnSearchSuccess;
        }

        private void GameManagerViewModelOnSearchOutOfRange(object sender, EventArgs eventArgs)
        {
            SearchPokestopButton.IsEnabled = false;
            OutOfRangeTextBlock.Visibility = ErrorMessageBorder.Visibility = Visibility.Visible;
        }

        private void GameManagerViewModelOnSearchInventoryFull(object sender, EventArgs eventArgs)
        {
            SearchPokestopButton.IsEnabled = false;
            InventoryFulleTextBlock.Visibility = ErrorMessageBorder.Visibility = Visibility.Visible;
        }

        private void GameManagerViewModelOnSearchInCooldown(object sender, EventArgs eventArgs)
        {
            SearchPokestopButton.IsEnabled = false;
            CooldownTextBlock.Visibility = ErrorMessageBorder.Visibility = Visibility.Visible;
        }

        private void GameManagerViewModelOnSearchSuccess(object sender, EventArgs eventArgs)
        {
            SearchPokestopButton.IsEnabled = false;
            SpinPokestopImage.Begin();
        }

        #endregion
    }
}