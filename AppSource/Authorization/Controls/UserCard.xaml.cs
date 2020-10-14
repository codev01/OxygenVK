using System;

using OxygenVK.AppSource.Views;
using OxygenVK.Authorization;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace OxygenVK.AppSource.Authorization.Controls
{
	public sealed partial class UserCard : UserControl
	{
		public Parameter Parameter { get; set; }
		public Frame Frame { get; set; }
		public AuthorizedUserCardsAttachment AuthorizedUserCardsAttachment { get; set; }

		public UserCard()
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
			new ListOfAuthorizedUsers().DeleteUserData(AuthorizedUserCardsAttachment.UserID);
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

		private void ThisWindow_Click(object sender, RoutedEventArgs e)
		{
			new RootFrameNavigation(Frame, typeof(MainPage), new Authorize(AuthorizedUserCardsAttachment.Token, true).VkApi);
		}

		private void NewWindow_Click(object sender, RoutedEventArgs e)
		{
			Parameter.VkApi = new Authorize(AuthorizedUserCardsAttachment.Token, true).VkApi;
			new WindowGenerator(Parameter);
		}
	}
}
