using System;
using System.Numerics;
using System.Threading.Tasks;

using OxygenVK.AppSource.Authorization.Controls;
using OxygenVK.AppSource.Views.Settings;
using OxygenVK.AppSource.Views.User;
using OxygenVK.Authorization;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

using MUXC = Microsoft.UI.Xaml.Controls;

namespace OxygenVK.AppSource
{
	public sealed partial class MainPage : Page
	{
		private bool paneIsOpen;
		private Enum displayMode;
		private readonly DispatcherTimer DispatcherTimer = new DispatcherTimer();

		public MainPage()
		{
			InitializeComponent();
		}

		private void Navigation_Loaded(object sender, RoutedEventArgs e)
		{
			//contentFrame.Navigate(typeof(NewsPage), null, new DrillInNavigationTransitionInfo());
			Window.Current.SetTitleBar(AppTitleBar);
			DispatcherTimer.Tick += DispatcherTimer_Tick;
			ListOfAuthorizedUsers.OnListUpdated += ListOfAuthorizedUsers_OnListUpdated; ;
			accauntsSplitButtonList_Add();
		}

		private void ListOfAuthorizedUsers_OnListUpdated()
		{
			accauntsSplitButtonList_Add();
		}

		private void accauntsSplitButtonList_Add()
		{
			accountsSplitButtonList.Items.Clear();
			if (ListOfAuthorizedUsers.listOfAuthorizedUsers.Count != 0)
			{
				listAccounts.Visibility = Visibility.Visible;
				ListOfAuthorizedUsers.listOfAuthorizedUsers.Reverse();
				foreach (AuthorizedUserCardsAttachment item in ListOfAuthorizedUsers.listOfAuthorizedUsers)
				{
					HorizontalUserCard horizontalUserCard = new HorizontalUserCard();
					horizontalUserCard.ClickDelete += HorizontalUserCard_ClickDelete;
					horizontalUserCard.Margin = new Thickness(-12, 0, -12, 10);
					horizontalUserCard.Frame = Frame;
					horizontalUserCard.AuthorizedUserCardsAttachment = new AuthorizedUserCardsAttachment
					{
						UserID = item.UserID,
						UserName = item.UserName,
						ScreenName = item.ScreenName,
						AvatarUrl = item.AvatarUrl
					};

					accountsSplitButtonList.Items.Add(horizontalUserCard);
					//accountsSplitButtonList.Items.Add(new AuthorizedUserCardsAttachment
					//{
					//	UserID = item.UserID,
					//	UserName = item.UserName,
					//	ScreenName = item.ScreenName,
					//	AvatarUrl = item.AvatarUrl
					//});
				}
			}
			else
			{
				listAccounts.Visibility = Visibility.Collapsed;
				//borderHintRecentlyLoggedIn.Visibility = Visibility.Collapsed;
				//cardAddButton.Content = "Войти в аккаунт";
				//cardAdd_Click(this, null);
			}
		}

		private void webAuthControl_Closing()
		{
			//webAuthControl.Opacity = 0;
			//webAuthControl.Visibility = Visibility.Collapsed;
		}

		private void addAccountsButton_Click(object sender, RoutedEventArgs e)
		{
			//webAuthControl.wv_Navigate();
			flyoutListAccount.Hide();
			listAccounts.Visibility = Visibility.Collapsed;
			//webAuthControl.Opacity = 1;
			//webAuthControl.Visibility = Visibility.Visible;
		}

		private void accountsSplitButton_Click(MUXC.SplitButton sender, MUXC.SplitButtonClickEventArgs args)
		{
			contentFrame.Navigate(typeof(UserPage), null, new DrillInNavigationTransitionInfo());
		}

		private void HorizontalUserCard_ClickDelete(AuthorizedUserCardsAttachment authorizedUserCardsAttachment)
		{
			foreach (HorizontalUserCard item in accountsSplitButtonList.Items)
			{
				if (item.AuthorizedUserCardsAttachment.UserID == authorizedUserCardsAttachment.UserID)
				{
					accountsSplitButtonList.Items.Remove(item);
					Task.Run(() =>
					{
						new ListOfAuthorizedUsers().DeleteUserData(item.AuthorizedUserCardsAttachment.UserID);
					});
				}
			}
			if (accountsSplitButtonList.Items.Count == 0)
			{
				addAccountsButton_Click(this, null);
				//borderHintRecentlyLoggedIn.Visibility = Visibility.Collapsed;
			}
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


