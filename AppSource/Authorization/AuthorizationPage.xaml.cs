using System;
using System.Collections.Generic;

using OxygenVK.AppSource.Authorization.Controls;
using OxygenVK.AppSource.Authorization.DialogBoxes;
using OxygenVK.AppSource.LocalSettings.Attachments;
using OxygenVK.AppSource.Views.Settings;

using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OxygenVK.AppSource.Authorization
{
	public partial class AuthorizationPage : Page
	{
		public static bool IsThePageIsUsedInNavigation = false;

		private Parameter Parameter;

		public AuthorizationPage()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			ApplicationView.GetForCurrentView().Title = "Окно авторизации";
			IsThePageIsUsedInNavigation = true;

			Frame.Navigating += Frame_Navigating;

			WorkWithUserData.OnListStartUpdate += WorkWithUserData_OnListStartUpdate;
			WorkWithUserData.OnListUpdated += WorkWithUserData_OnListUpdated;

			new WorkWithUserData().UpdateList();
		}

		private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			IsThePageIsUsedInNavigation = false;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			Parameter = e?.Parameter as Parameter;
			if (e.Parameter as string != "")
			{
				ThemeHelper.RootTheme = App.GetEnum<ElementTheme>(Parameter.ApplicationSettings.ElementTheme.ToString());
				ElementSoundPlayer.State = Parameter.ApplicationSettings.ElementSoundPlayerState;
				ElementSoundPlayer.SpatialAudioMode = Parameter.ApplicationSettings.ElementSpatialAudioMode;
			}
		}

		private void ListUsersCard_GridView_Add(List<SettingsAttachments> authorizedUserCardsAttachments)
		{
			cardAddButton.Content = "Войти в другой аккаунт";
			borderHintRecentlyLoggedIn.Visibility = Visibility.Visible;
			listUsersCard_GridView.Items.Clear();

			if (authorizedUserCardsAttachments.Count != 0)
			{
				foreach (SettingsAttachments item in authorizedUserCardsAttachments)
				{
					listUsersCard_GridView.Items.Add(new CardAttachmetsHelper(Frame, item).AddNewUserCard());
				}
			}
			else
			{
				WebAuthControl_Closing();
				borderHintRecentlyLoggedIn.Visibility = Visibility.Collapsed;
				cardAddButton.Content = "Войти в аккаунт";
				listUsersCard_GridView.Items.Clear();
				CardAdd_Click(this, null);
			}
		}

		private async void WorkWithUserData_OnListStartUpdate()
		{
			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				ListUpdatedProgress_SetVisibility(Visibility.Visible);
			});
		}

		private async void WorkWithUserData_OnListUpdated(List<SettingsAttachments> authorizedUserCardsAttachments)
		{
			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				ListUpdatedProgress_SetVisibility(Visibility.Collapsed);
				ListUsersCard_GridView_Add(authorizedUserCardsAttachments);
			});
		}

		private void ListUpdatedProgress_SetVisibility(Visibility visibility)
		{
			if (visibility == Visibility.Visible)
			{
				if (WorkWithUserData.SettingsAttachments.Count == 0)
				{
					listUpdatedProgress.Visibility = Visibility.Visible;
					listUpdatedProgress.IsActive = true;
				}
				else
				{
					listUpdatedProgress2.Visibility = Visibility.Visible;
					listUpdatedProgress2.IsActive = true;
				}
			}
			if (visibility == Visibility.Collapsed)
			{
				listUpdatedProgress.Visibility = Visibility.Collapsed;
				listUpdatedProgress2.Visibility = Visibility.Collapsed;
				listUpdatedProgress.IsActive = false;
				listUpdatedProgress2.IsActive = false;
			}
		}

		private void CardAdd_Click(object sender, RoutedEventArgs e)
		{
			webAuthControl.OnAuthCompleted += WebAuthControl_OnAuthCompleted;
			webAuthControl.Wv_Navigate();
			webAuthControl.Opacity = 1;
			webAuthControl.Visibility = Visibility.Visible;
		}

		private void WebAuthControl_Closing()
		{
			webAuthControl.OnAuthCompleted -= WebAuthControl_OnAuthCompleted;
			webAuthControl.Opacity = 0;
			webAuthControl.Visibility = Visibility.Collapsed;
		}

		private async void WebAuthControl_OnAuthCompleted(string token)
		{
			if (IsThePageIsUsedInNavigation)
			{
				await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
				{
					InWhichWindowDialog dialog = new InWhichWindowDialog
					{
						Frame = Frame,
						Token = token
					};
					WebAuthControl_Closing();

					_ = dialog.ShowAsync(ContentDialogPlacement.InPlace);
				});
			}
		}

		private void HeaderListPanel_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.NewSize.Width < 655)
			{
				hintRecentlyLoggedIn.Width = 444 - 48;
			}
			else
			{
				hintRecentlyLoggedIn.Width = 500;
			}
		}
	}
}
