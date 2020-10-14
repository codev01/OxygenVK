using System;
using System.Collections.Generic;

using OxygenVK.AppSource;
using OxygenVK.AppSource.Authorization;
using OxygenVK.AppSource.Authorization.Controls;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OxygenVK.Authorization
{
	public partial class AuthorizationPage : Page
	{
		public AuthorizationPage()
		{
			InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Enabled;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			//Parameter = e.Parameter as Parameter;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			Authorize.OnAuthorizationComleted += Authorization_OnAuthorizationComleted;

			ListOfAuthorizedUsers.OnListStartUpdate += ListOfAuthorizedUsers_OnListStartUpdate;
			ListOfAuthorizedUsers.OnListUpdated += ListOfAuthorizedUsers_OnListUpdated;
			ListOfAuthorizedUsers.OnListNull += ListOfAuthorizedUsers_OnListNull;

			MainPage.OnBackNavigation += MainPage_OnBackNavigation;

			new ListOfAuthorizedUsers().GetUserData();
			listUsersCard_GridView_Add(ListOfAuthorizedUsers.listOfAuthorizedUsers);
		}

		private async void Authorization_OnAuthorizationComleted(Parameter parameter)
		{
			webAuthControl_Closing();
			InWhichWindowDialog dialog = new InWhichWindowDialog
			{
				Frame = Frame,
				Parameter = parameter
			};
			await dialog.ShowAsync();
		}

		private void ListOfAuthorizedUsers_OnListStartUpdate()
		{
			_ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
			{
				if (borderHintRecentlyLoggedIn.Visibility == Visibility.Collapsed)
				{
					listUpdatedProgress.Visibility = Visibility.Visible;
					listUpdatedProgress.IsActive = true;
				}
				listUpdatedProgress2.IsActive = true;
			});
		}

		private void ListOfAuthorizedUsers_OnListUpdated()
		{
			_ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
			{
				listUpdatedProgress.IsActive = false;
				listUpdatedProgress.Visibility = Visibility.Collapsed;
				listUsersCard_GridView_Add(ListOfAuthorizedUsers.listOfAuthorizedUsers);
				listUpdatedProgress2.IsActive = false;
			});
		}

		private void listUsersCard_GridView_Add(List<AuthorizedUserCardsAttachment> authorizedUserCardsAttachments)
		{
			cardAddButton.Content = "Войти в другой аккаунт";
			borderHintRecentlyLoggedIn.Visibility = Visibility.Visible;

			listUsersCard_GridView.Items.Clear();
			authorizedUserCardsAttachments.Reverse();
			try
			{
				foreach (AuthorizedUserCardsAttachment item in authorizedUserCardsAttachments)
				{
					UserCard userCard = new UserCard
					{
						Parameter = new Parameter() 
						{
							UserID = item.UserID
						},
						Frame = Frame,
						AuthorizedUserCardsAttachment = new AuthorizedUserCardsAttachment
						{
							UserID = item.UserID,
							UserName = item.UserName,
							ScreenName = item.ScreenName,
							Token = item.Token,
							AvatarUrl = item.AvatarUrl,
						}
					};
					listUsersCard_GridView.Items.Add(userCard);
				}
			}
			catch { }
		}

		private void MainPage_OnBackNavigation()
		{
			_ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				cardAdd_Click(this, null);
			});
		}

		private void cardAdd_Click(object sender, RoutedEventArgs e)
		{
			webAuthControl.wv_Navigate();
			webAuthControl.Opacity = 1;
			webAuthControl.Visibility = Visibility.Visible;
		}

		private void ListOfAuthorizedUsers_OnListNull()
		{
			webAuthControl_Closing();
			borderHintRecentlyLoggedIn.Visibility = Visibility.Collapsed;
			cardAddButton.Content = "Войти в аккаунт";
			listUsersCard_GridView.Items.Clear();
			cardAdd_Click(this, null);
		}

		private void webAuthControl_Closing()
		{
			webAuthControl.Opacity = 0;
			webAuthControl.Visibility = Visibility.Collapsed;
		}

		private void headerListPanel_SizeChanged(object sender, SizeChangedEventArgs e)
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
