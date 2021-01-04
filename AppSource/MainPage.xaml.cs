using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Timers;

using OxygenVK.AppSource.Authorization;
using OxygenVK.AppSource.Authorization.Controls;
using OxygenVK.AppSource.LocalSettings.Attachments;
using OxygenVK.AppSource.LocaSettings.Attachments;
using OxygenVK.AppSource.Views;
using OxygenVK.AppSource.Views.Settings;
using OxygenVK.AppSource.Views.User;
using OxygenVK.Authorization;

using VkNet.Enums.SafetyEnums;
using VkNet.Model;

using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using Application = Windows.UI.Xaml.Application;
using MUXC = Microsoft.UI.Xaml.Controls;

namespace OxygenVK.AppSource
{
	public sealed partial class MainPage : Page
	{
		private long UserID = 0;
		private bool IsNavigatedUserPage;
		private Parameter Parameter;
		private bool paneIsOpen;
		private Enum displayMode;
		private bool isNVFirstLoaded;
		private HorizontalAlignment AppNameTextBlock_HorizontalAlignment;
		private readonly Timer Timer = new Timer() { Interval = 300 };

		private enum Selection
		{
			SelectionNavigationItem,
			NavigationUserPage
		}

		public MainPage()
		{
			InitializeComponent();
			Navigation.IsPaneOpen = false;
			Window.Current.SetTitleBar(AppTitleBar);
		}

		private void Navigation_Loaded(object sender, RoutedEventArgs e)
		{
			isNVFirstLoaded = true;
			Timer.Elapsed += Timer_Elapsed;

			Navigation.ItemInvoked += Navigation_ItemInvoked;

			WorkWithUserData.OnListUpdated += WorkWithUserData_OnListUpdated;

			LoadNavigationContent();

			Navigation.IsPaneOpen = true;
		}

		private async void LoadNavigationContent()
		{
			VkNet.Model.RequestParams.AccountSaveProfileInfoParams profileInfo = await Parameter.VkApi.Account.GetProfileInfoAsync();
			firstANDlastNameSplitButton.Text = profileInfo.FirstName + " " + profileInfo.LastName;

			ApplicationView.GetForCurrentView().Title = new WindowNamesHelper(profileInfo.FirstName + " " + profileInfo.LastName).GetNameWindow();
			
			foreach (VkNet.Model.Attachments.Photo photo in await Parameter.VkApi.Photo.GetAsync(new VkNet.Model.RequestParams.PhotoGetParams
			{
				AlbumId = PhotoAlbumType.Profile,
				Count = 1
			}))
			{
				BitmapImage bitmapImage = new BitmapImage(new Uri(photo.Sizes.Last().Url.AbsoluteUri))
				{
					DecodePixelHeight = 50,
					DecodePixelWidth = 50
				};
				personPictureSplitButton.ProfilePicture = bitmapImage;
			}
			accountsSplitButtonProgressBar.Visibility = Visibility.Collapsed;
			accountsSplitButtonContent.Visibility = Visibility.Visible;
			accountsSplitButtonContent.Opacity = 1;
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			Parameter = e?.Parameter as Parameter;

			foreach (User item in await Parameter.VkApi.Users.GetAsync(new long[0]))
			{
				UserID = item.Id;
			}

			if (e.Parameter as string != "")
			{
				ThemeHelper.RootTheme = App.GetEnum<ElementTheme>(Parameter.ApplicationSettings.ElementTheme.ToString());
				ElementSoundPlayer.State = Parameter.ApplicationSettings.ElementSoundPlayerState;
				ElementSoundPlayer.SpatialAudioMode = Parameter.ApplicationSettings.ElementSpatialAudioMode;
			}

			AccountsSplitButtonList_Add(WorkWithUserData.SettingsAttachments);
		}

		private async void AddAccountsButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			if (WindowNamesHelper.AuthorizationPageWindowOpened && !AuthorizationPage.ThePageIsUsedInNavigation)
			{
				await ApplicationViewSwitcher.SwitchAsync(WindowNamesHelper.AuthorizationPageWindowID);
			}
			else
			{
				new WindowGenerator(GetParameter(), typeof(AuthorizationPage));
			}
		}

		private void AccountsSplitButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			new RootFrameNavigation(contentFrame, typeof(UserPage), null);
			UpdateIndicatorForeground(Selection.NavigationUserPage);
		}

		private async void WorkWithUserData_OnListUpdated(List<SettingsAttachments> SettingsAttachments)
		{
			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				AccountsSplitButtonList_Add(SettingsAttachments);
			});
		}

		private void AccountsSplitButtonList_Add(List<SettingsAttachments> settingsAttachments)
		{
			listAccounts.Visibility = Visibility.Visible;
			accountsSplitButtonList.Items.Clear();

			if (settingsAttachments.Count != 0)
			{
				foreach (SettingsAttachments item in settingsAttachments)
				{
					if (Parameter != null)
					{
						if (UserID != item.UserDataAttachments.UserID)
						{
							accountsSplitButtonList.Items.Add(new CardAttachmetsHelper(Frame, item).AddNewHorizontalUserCard());
						}
						else if (settingsAttachments.Count <= 1)
						{
							listAccounts.Visibility = Visibility.Collapsed;
							accountsSplitButtonList.Items.Clear();
						}
					}
				}
			}
			else
			{
				listAccounts.Visibility = Visibility.Collapsed;
				accountsSplitButtonList.Items.Clear();
			}
			if (isNVFirstLoaded)
			{
				Navigation.IsPaneOpen = true;
				isNVFirstLoaded = false;
			}
		}

		private void UpdateIndicatorForeground(Selection selection)
		{
			if (selection == Selection.NavigationUserPage)
			{
				Application.Current.Resources["NavigationViewSelectionIndicatorForeground"] = Colors.Transparent;
				IsNavigatedUserPage = true;
			}
			else
			{
				Application.Current.Resources["NavigationViewSelectionIndicatorForeground"] = Resources["SystemAccentColor"];
				IsNavigatedUserPage = false;
			}

			ElementTheme theme = RequestedTheme;
			if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
			{
				RequestedTheme = ElementTheme.Light;
			}
			else
			{
				RequestedTheme = ElementTheme.Dark;
			}
			RequestedTheme = theme;
		}

		private Parameter GetParameter()
		{
			return new Parameter
			{
				ApplicationSettings = new ApplicationSettingsAttachments
				{
					ElementTheme = ThemeHelper.RootTheme,
					ElementSoundPlayerState = ElementSoundPlayer.State,
					ElementSpatialAudioMode = ElementSoundPlayer.SpatialAudioMode
				},
				VkApi = Parameter.VkApi
			};
		}

		private void Navigation_ItemInvoked(MUXC.NavigationView sender, MUXC.NavigationViewItemInvokedEventArgs args)
		{
			if (IsNavigatedUserPage)
			{
				UpdateIndicatorForeground(Selection.SelectionNavigationItem);
			}
			switch (args.InvokedItemContainer.Tag.ToString())
			{
				case "news":
					new RootFrameNavigation(contentFrame, typeof(SettingsPage), GetParameter());
					break;
				case "test":
					break;

				case "Параметры":
					new RootFrameNavigation(contentFrame, typeof(SettingsPage), GetParameter());
						break;
			}
		}

		private void Navigation_DisplayModeChanged(MUXC.NavigationView sender, MUXC.NavigationViewDisplayModeChangedEventArgs args)
		{
			displayMode = args.DisplayMode;
			AppTitleBar_Margin(displayMode, paneIsOpen);
		}

		private void Navigation_PaneOpening(MUXC.NavigationView sender, object args)
		{
			accountsSplitButton.Visibility = Visibility.Visible;
			accountsSplitButton.Opacity = 1;
			AppNameTextBlock.Visibility = Visibility.Collapsed;
			AppNameTextBlock.Opacity = 0;
			AppNameTextBlock_HorizontalAlignment = HorizontalAlignment.Left;
			AppNameTextBlock.Margin = new Thickness(5, 0, 150, 0);
			paneIsOpen = true;

			AppTitleBar_Margin(displayMode, paneIsOpen);

			Timer.Start();
		}

		private void Navigation_PaneClosing(MUXC.NavigationView sender, MUXC.NavigationViewPaneClosingEventArgs args)
		{
			accountsSplitButton.Visibility = Visibility.Collapsed;
			accountsSplitButton.Opacity = 0;
			AppNameTextBlock.Visibility = Visibility.Collapsed;
			AppNameTextBlock.Opacity = 0;
			AppNameTextBlock_HorizontalAlignment = HorizontalAlignment.Right;
			AppNameTextBlock.Margin = new Thickness(5, -10, 150, 0);
			paneIsOpen = false;

			AppTitleBar_Margin(displayMode, paneIsOpen);

			Timer.Start();
		}

		private async void Timer_Elapsed(object sender, object e)
		{
			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				AppNameTextBlock.HorizontalAlignment = AppNameTextBlock_HorizontalAlignment;
				AppNameTextBlock.Visibility = Visibility.Visible;
				AppNameTextBlock.Opacity = 1;
			});

			Timer.Stop();
		}

		public void AppTitleBar_Margin(Enum displayMode, bool paneIsOpen)
		{
			switch (displayMode)
			{
				case MUXC.NavigationViewDisplayMode.Minimal:

					accountsSplitButtonContent.Width = 270;
					accountsSplitButton.Margin = new Thickness(5, 48, -75, 0);
					accountsSplitButton.Translation = new Vector3(-80, 0, 0);

					if (paneIsOpen)
					{
						AppTitleBar.Margin = new Thickness(80, 0, 0, 0);
					}
					else
					{
						AppTitleBar.Margin = new Thickness(80, 0, 0, 0);
					}

					break;
				case MUXC.NavigationViewDisplayMode.Compact:

					AppTitleBar.Margin = new Thickness(40, 0, 0, 0);
					accountsSplitButtonContent.Width = 230;
					accountsSplitButton.Margin = new Thickness(5, 0, 5, 0);
					accountsSplitButton.Translation = new Vector3(0, 0, 0);

					break;
				case MUXC.NavigationViewDisplayMode.Expanded:

					AppTitleBar.Margin = new Thickness(40, 0, 0, 0);
					accountsSplitButtonContent.Width = 230;
					accountsSplitButton.Margin = new Thickness(5, 0, 5, 0);
					accountsSplitButton.Translation = new Vector3(0, 0, 0);

					break;
			}
		}
	}
}


