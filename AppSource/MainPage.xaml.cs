using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

using OxygenVK.AppSource.Authorization.Controls;
using OxygenVK.AppSource.Views.Settings;
using OxygenVK.Authorization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

using MUXC = Microsoft.UI.Xaml.Controls;

namespace OxygenVK.AppSource
{
	public sealed partial class MainPage : Page
	{
		public Enum displayMode;
		public bool paneIsOpen;
		public static double ContainerAdapterWidth { get; set; }

		private DispatcherTimer timer;

		public MainPage()
		{
			InitializeComponent();
			Window.Current.SetTitleBar(AppTitleBar);
		}
		private void Navigation_Loaded(object sender, RoutedEventArgs e)
		{
			//contentFrame.Navigate(typeof(NewsPage), null, new DrillInNavigationTransitionInfo());
			accauntsSplitButtonList_Add();
		}

		private void accauntsSplitButtonList_Add()
		{
			accauntsSplitButtonList.Items.Clear();
			if (ListOfAuthorizedUsers.listOfAuthorizedUsers.Count != 0)
			{
				ListOfAuthorizedUsers.listOfAuthorizedUsers.Reverse();
				foreach (AuthorizedUserCardsAttachment item in ListOfAuthorizedUsers.listOfAuthorizedUsers)
				{
					accauntsSplitButtonList.Items.Add(new AuthorizedUserCardsAttachment
					{
						UserID = item.UserID,
						UserName = item.UserName,
						ScreenName = item.ScreenName,
						AvatarUrl = item.AvatarUrl
					});
				}
			}
			else
			{
				//borderHintRecentlyLoggedIn.Visibility = Visibility.Collapsed;
				//cardAddButton.Content = "Войти в аккаунт";
				//cardAdd_Click(this, null);
			}
		}

		private void HorizontalUserCard_ClickDelete(AuthorizedUserCardsAttachment authorizedUserCardsAttachment)
		{
			foreach (AuthorizedUserCardsAttachment item in accauntsSplitButtonList.Items)
			{
				if (item.UserID == authorizedUserCardsAttachment.UserID)
				{
					accauntsSplitButtonList.Items.Remove(item);
					Task.Run(() =>
					{
						new ListOfAuthorizedUsers().DeleteUserData(item.UserID);
					});
				}
			}
			if (accauntsSplitButtonList.Items.Count == 0)
			{
				//cardAdd_Click(this, null);
				//borderHintRecentlyLoggedIn.Visibility = Visibility.Collapsed;
			}
		}

		private void contentFrame_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			try
			{
			}
			catch
			{
				//
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
			accauntsSplitButton.Visibility = Visibility.Collapsed;
			accauntsSplitButton.Opacity = 0;

			paneIsOpen = false;
			AppNameTextBlock_Margin(displayMode, paneIsOpen);
		}

		private void Navigation_PaneOpening(MUXC.NavigationView sender, object args)
		{
			accauntsSplitButton.Opacity = 1;
			accauntsSplitButton.Visibility = Visibility.Visible;
			paneIsOpen = true;
			AppNameTextBlock_Margin(displayMode, paneIsOpen);
		}

		public void AppNameTextBlock_Margin(Enum displayMode, bool paneIsOpen)
		{
			switch (displayMode)
			{
				case MUXC.NavigationViewDisplayMode.Minimal:
					if (paneIsOpen)
					{
						AppTitleBar.Margin = new Thickness(320, 0, 0, 0);
						AppNameTextBlock.Visibility = Visibility.Collapsed;
						AppNameTextBlock.Opacity = 0;
						accauntsSplitButtonContent.Width = 189;
					}
					else
					{
						AppTitleBar.Margin = new Thickness(80, 0, 0, 0);
						AppNameTextBlock.Visibility = Visibility.Visible;
						AppNameTextBlock.Opacity = 1;
						AppNameTextBlock.Translation = new Vector3(0, 0, 0);
					}
					break;
				case MUXC.NavigationViewDisplayMode.Compact:
					accauntsSplitButtonContent.Width = 229;
					AppTitleBar.Margin = new Thickness(40, 0, 0, 0);
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
					accauntsSplitButtonContent.Width = 229;
					AppTitleBar.Margin = new Thickness(40, 0, 0, 0);
					AppNameTextBlock.Visibility = Visibility.Visible;
					AppNameTextBlock.Opacity = 1;
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
