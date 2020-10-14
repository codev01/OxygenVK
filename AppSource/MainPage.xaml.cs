using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using OxygenVK.AppSource.Authorization;
using OxygenVK.AppSource.Authorization.Controls;
using OxygenVK.AppSource.Views.Settings;
using OxygenVK.AppSource.Views.User;
using OxygenVK.Authorization;

using VkNet.Enums.SafetyEnums;

using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using MUXC = Microsoft.UI.Xaml.Controls;

namespace OxygenVK.AppSource
{
	public sealed partial class MainPage : Page
	{
		public delegate void BackNavigationEvent();
		public static event BackNavigationEvent OnBackNavigation;

		private Parameter Parameter;
		private bool paneIsOpen;
		private Enum displayMode;
		private bool isNVFirstLoaded;
		private readonly DispatcherTimer DispatcherTimer = new DispatcherTimer();

		public MainPage()
		{
			InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Enabled;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			Parameter = e.Parameter as Parameter;
		}

		private void Navigation_Loaded(object sender, RoutedEventArgs e)
		{
			isNVFirstLoaded = true;
			DispatcherTimer.Tick += DispatcherTimer_Tick;

			ListOfAuthorizedUsers.OnListUpdated += ListOfAuthorizedUsers_OnListUpdated;
			ListOfAuthorizedUsers.OnListNull += ListOfAuthorizedUsers_OnListNull;

			Window.Current.SetTitleBar(AppTitleBar);

			LoadNavigationContent();
			new ListOfAuthorizedUsers().GetUserData();
			accountsSplitButtonList_Add(ListOfAuthorizedUsers.listOfAuthorizedUsers);

			//contentFrame.Navigate(typeof(NewsPage), null, new DrillInNavigationTransitionInfo());
		}

		private async void LoadNavigationContent()
		{
			VkNet.Model.RequestParams.AccountSaveProfileInfoParams profileInfo = await Parameter.VkApi.Account.GetProfileInfoAsync();
			firstANDlastNameSplitButton.Text = profileInfo.FirstName + " " + profileInfo.LastName;

			foreach (VkNet.Model.Attachments.Photo photo in await Parameter.VkApi.Photo.GetAsync(new VkNet.Model.RequestParams.PhotoGetParams
			{
				AlbumId = PhotoAlbumType.Profile,
				Count = 1
			}))
			{
				personPictureSplitButton.ProfilePicture = new BitmapImage(new Uri(photo.Sizes.Last().Url.AbsoluteUri));
			}
			accountsSplitButtonProgressBar.Visibility = Visibility.Collapsed;
			accountsSplitButtonContent.Visibility = Visibility.Visible;
			accountsSplitButtonContent.Opacity = 1;
		}

		private void ListOfAuthorizedUsers_OnListUpdated()
		{
			_ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
			{
				accountsSplitButtonList_Add(ListOfAuthorizedUsers.listOfAuthorizedUsers);
			});
		}

		private void ListOfAuthorizedUsers_OnListNull()
		{
			listAccounts.Visibility = Visibility.Collapsed;
			accountsSplitButtonList.Items.Clear();
		}

		private void accountsSplitButtonList_Add(List<AuthorizedUserCardsAttachment> authorizedUserCardsAttachments)
		{
			listAccounts.Visibility = Visibility.Visible;

			accountsSplitButtonList.Items.Clear();
			authorizedUserCardsAttachments.Reverse();
			try
			{
				foreach (AuthorizedUserCardsAttachment item in authorizedUserCardsAttachments)
				{
					if (Parameter != null)
					{
						if (Parameter.UserID != item.UserID)
						{
							HorizontalUserCard horizontalUserCard = new HorizontalUserCard()
							{
								AuthorizedUserCardsAttachment = new AuthorizedUserCardsAttachment()
								{
									UserID = item.UserID,
									UserName = item.UserName,
									ScreenName = item.ScreenName,
									Token = item.Token,
									AvatarUrl = item.AvatarUrl
								},
								Margin = new Thickness(-12, 0, -12, 10),
								Parameter = Parameter,
								Frame = Frame
							};
							accountsSplitButtonList.Items.Add(horizontalUserCard);
						}
						else if (authorizedUserCardsAttachments.Count <= 1)
						{
							ListOfAuthorizedUsers_OnListNull();
						}
					}
				}
			}
			catch { }
			if (isNVFirstLoaded)
			{
				Navigation.IsPaneOpen = true;
				isNVFirstLoaded = false;
			}
		}

		private async void addAccountsButton_Click(object sender, RoutedEventArgs e)
		{
			OnBackNavigation.Invoke();
			await ApplicationViewSwitcher.TryShowAsStandaloneAsync(App.MainWindow);
		}

		private void accountsSplitButton_Click(MUXC.SplitButton sender, MUXC.SplitButtonClickEventArgs args)
		{
			contentFrame.Navigate(typeof(UserPage), null, new DrillInNavigationTransitionInfo());
		}

		private void Navigation_SelectionChanged(MUXC.NavigationView sender, MUXC.NavigationViewSelectionChangedEventArgs args)
		{
			if (args.IsSettingsSelected)
			{
				contentFrame.Navigate(typeof(SettingsPage), null, new DrillInNavigationTransitionInfo());
			}
			switch (args.SelectedItemContainer.Tag.ToString())
			{
				case "news":
					//contentFrame.Navigate(typeof(NewsPage), null, new DrillInNavigationTransitionInfo());
					break;
				case "test":
					break;
			}
		}

		private void Navigation_DisplayModeChanged(MUXC.NavigationView sender, MUXC.NavigationViewDisplayModeChangedEventArgs args)
		{
			displayMode = args.DisplayMode;
			AppNameTextBlock_Margin(displayMode, paneIsOpen);
		}

		private void Navigation_PaneClosing(MUXC.NavigationView sender, MUXC.NavigationViewPaneClosingEventArgs args)
		{
			accountsSplitButton.Visibility = Visibility.Collapsed;
			accountsSplitButton.Opacity = 0;

			paneIsOpen = false;
			AppNameTextBlock_Margin(displayMode, paneIsOpen);
		}

		private void Navigation_PaneOpening(MUXC.NavigationView sender, object args)
		{
			accountsSplitButton.Opacity = 1;
			accountsSplitButton.Visibility = Visibility.Visible;
			paneIsOpen = true;
			AppNameTextBlock_Margin(displayMode, paneIsOpen);
		}

		private void Navigation_PaneOpened(MUXC.NavigationView sender, object args)
		{
			AppNameTextBlock.Visibility = Visibility.Visible;
			AppNameTextBlock.Opacity = 1;
		}


		private void DispatcherTimer_Tick(object sender, object e)
		{
			AppNameTextBlock.Visibility = Visibility.Visible;
			AppNameTextBlock.Opacity = 1;
			DispatcherTimer.Stop();
		}

		private void Navigation_PaneClosed(MUXC.NavigationView sender, object args)
		{
			DispatcherTimer.Interval = new TimeSpan(2000000);
			DispatcherTimer.Start();
		}

		public void AppNameTextBlock_Margin(Enum displayMode, bool paneIsOpen)
		{
			switch (displayMode)
			{
				case MUXC.NavigationViewDisplayMode.Minimal:
					if (paneIsOpen)
					{
						AppTitleBar.Margin = new Thickness(80, 0, -70, 0);
						AppNameTextBlock.Visibility = Visibility.Collapsed;
						AppNameTextBlock.Opacity = 0;
						accountsSplitButtonContent.Width = 270;
						accountsSplitButton.Margin = new Thickness(5, 48, -70, 0);
						accountsSplitButton.Translation = new Vector3(-80, 0, 0);
					}
					else
					{
						AppTitleBar.Margin = new Thickness(80, 0, 0, 0);
						AppNameTextBlock.Visibility = Visibility.Collapsed;
						AppNameTextBlock.Opacity = 0;
						DispatcherTimer.Interval = new TimeSpan(2000000);
						DispatcherTimer.Start();
						AppNameTextBlock.Translation = new Vector3(0, 0, 0);
					}
					break;
				case MUXC.NavigationViewDisplayMode.Compact:
					AppTitleBar.Margin = new Thickness(40, 0, 0, 0);
					accountsSplitButtonContent.Width = 230;
					accountsSplitButton.Margin = new Thickness(5, 0, 10, 0);
					accountsSplitButton.Translation = new Vector3(0, 0, 0);
					if (paneIsOpen)
					{
						AppNameTextBlock.Translation = new Vector3(0, 0, 0);
					}
					else
					{
						AppNameTextBlock.Translation = new Vector3(20, 0, 0);
					}
					break;
				case MUXC.NavigationViewDisplayMode.Expanded:
					AppTitleBar.Margin = new Thickness(40, 0, 0, 0);
					AppNameTextBlock.Visibility = Visibility.Visible;
					AppNameTextBlock.Opacity = 1;
					accountsSplitButtonContent.Width = 230;
					if (paneIsOpen)
					{
						AppNameTextBlock.Translation = new Vector3(0, 0, 0);
					}
					else
					{
						AppNameTextBlock.Translation = new Vector3(20, 0, 0);
					}
					break;
			}
		}

	}
}


