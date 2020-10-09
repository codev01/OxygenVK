using System;
using System.Threading.Tasks;

using OxygenVK.AppSource.Authorization;

using VkNet;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OxygenVK.Authorization
{
	public partial class AuthorizationPage : Page
	{
		public AuthorizationPage()
		{
			InitializeComponent();
			Authorize.OnAuthorizationComleted += Authorization_OnAuthorizationComleted;
			ListOfAuthorizedUsers.OnListStartUpdate += ListOfAuthorizedUsers_OnListStartUpdate;
			ListOfAuthorizedUsers.OnListUpdated += ListOfAuthorizedUsers_OnListUpdated;
		}

		private async void Authorization_OnAuthorizationComleted(VkApi vkApi)
		{
			webAuthControl_Closing();
			InWhichWindowDialog dialog = new InWhichWindowDialog
			{
				VkApi = vkApi
			};
			await dialog.ShowAsync();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			new ListOfAuthorizedUsers();
			listUsersCard_GridView_Add();
		}

		private void ListOfAuthorizedUsers_OnListStartUpdate()
		{
			if (borderHintRecentlyLoggedIn.Visibility == Visibility.Collapsed)
			{
				listUpdatedProgress.Visibility = Visibility.Visible;
				listUpdatedProgress.IsActive = true;
			}
			listUpdatedProgress2.IsActive = true;
		}

		private void ListOfAuthorizedUsers_OnListUpdated()
		{
			listUpdatedProgress.IsActive = false;
			listUpdatedProgress.Visibility = Visibility.Collapsed;
			listUsersCard_GridView_Add();
			listUpdatedProgress2.IsActive = false;
		}

		private void cardAdd_Click(object sender, RoutedEventArgs e)
		{
			webAuthControl.wv_Navigate();
			webAuthControl.Opacity = 1;
			webAuthControl.Visibility = Visibility.Visible;
		}

		private void listUsersCard_GridView_Add()
		{
			listUsersCard_GridView.Items.Clear();
			if (ListOfAuthorizedUsers.listOfAuthorizedUsers.Count != 0)
			{
				cardAddButton.Content = "Войти в другой аккаунт";
				borderHintRecentlyLoggedIn.Visibility = Visibility.Visible;
				ListOfAuthorizedUsers.listOfAuthorizedUsers.Reverse();
				foreach (AuthorizedUserCardsAttachment item in ListOfAuthorizedUsers.listOfAuthorizedUsers)
				{
					listUsersCard_GridView.Items.Add(new AuthorizedUserCardsAttachment
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
				borderHintRecentlyLoggedIn.Visibility = Visibility.Collapsed;
				cardAddButton.Content = "Войти в аккаунт";
				cardAdd_Click(this, null);
			}
		}

		private void webAuthControl_Closing()
		{
			webAuthControl.Opacity = 0;
			webAuthControl.Visibility = Visibility.Collapsed;
		}

		private void UserCard_ClickDelete(AuthorizedUserCardsAttachment authorizedUserCardsAttachment)
		{
			foreach (AuthorizedUserCardsAttachment item in listUsersCard_GridView.Items)
			{
				if (item.UserID == authorizedUserCardsAttachment.UserID)
				{
					listUsersCard_GridView.Items.Remove(item);
					Task.Run(() =>
					{
						new ListOfAuthorizedUsers().DeleteUserData(item.UserID);
					});
				}
			}
			if (listUsersCard_GridView.Items.Count == 0)
			{
				cardAdd_Click(this, null);
				borderHintRecentlyLoggedIn.Visibility = Visibility.Collapsed;
			}
		}

		private void listUsersCard_GridView_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			hintRecentlyLoggedIn.Width = listUsersCard_GridView.ActualWidth - 54;
		}
	}
}
