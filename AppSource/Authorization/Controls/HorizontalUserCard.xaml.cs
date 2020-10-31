using System;

using OxygenVK.AppSource.Views;
using OxygenVK.Authorization;

using VkNet;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace OxygenVK.AppSource.Authorization.Controls
{
	public sealed partial class HorizontalUserCard : UserControl
	{
		public Frame Frame { get; set; }
		public AuthorizedUserCardsAttachment AuthorizedUserCardsAttachment { get; set; }

		public HorizontalUserCard()
		{
			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			avatar.ProfilePicture = new BitmapImage(new Uri(AuthorizedUserCardsAttachment.AvatarUrl));
			userName.Text = AuthorizedUserCardsAttachment.UserName;
		}

		private void deleteCard_Click(object sender, RoutedEventArgs e)
		{
			ListOfAuthorizedUsers listOfAuthorizedUsers = new ListOfAuthorizedUsers();
			listOfAuthorizedUsers.DeleteUserData(AuthorizedUserCardsAttachment.UserID);
			listOfAuthorizedUsers.InitializeList();
		}

		private void screenNameToolTip_Loaded(object sender, RoutedEventArgs e)
		{
			if (AuthorizedUserCardsAttachment.ScreenName == "")
			{
				screenNameToolTip.Visibility = Visibility.Collapsed;
			}
			else
			{
				screenNameToolTip.Content = AuthorizedUserCardsAttachment.ScreenName;
			}
		}

		private VkApi GetParameter()
		{
			return new Authorize(AuthorizedUserCardsAttachment.Token, true).VkApi;
		}

		private void ThisWindow_Click(object sender, RoutedEventArgs e)
		{
			new RootFrameNavigation(Frame, typeof(MainPage), GetParameter());
		}

		private void NewWindow_Click(object sender, RoutedEventArgs e)
		{
			new WindowGenerator(GetParameter(), typeof(MainPage));
		}
	}
}
