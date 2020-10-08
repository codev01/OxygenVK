using System;
using System.Threading.Tasks;

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
			Authorization.OnAuthorizationComleted += Authorization_OnAuthorizationComleted;
			ListOfAuthorizedUsers.OnListUpdated += ListOfAuthorizedUsers_OnListUpdated;
		}

		private void Authorization_OnAuthorizationComleted(VkApi vkApi)
		{
			OpenDialog();
			VkApi t = vkApi;
		}

		private async void OpenDialog()
		{
			InWhichWindowDialog dialog = new InWhichWindowDialog();
			await dialog.ShowAsync();
			webAuthControl_Closing();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			new ListOfAuthorizedUsers();
			listUsersCard_GridView_Add();
		}

		private void ListOfAuthorizedUsers_OnListUpdated()
		{
			listUsersCard_GridView_Add();
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
				listUsersCard_GridView.Visibility = Visibility.Visible;
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
				listUsersCard_GridView.Visibility = Visibility.Collapsed;
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
			if (listUsersCard_GridView.Items.Count != 0)
			{
				hintRecentlyLoggedIn.Width = e.NewSize.Width;
			}
		}
	}
}
