using System;

using OxygenVK.AppSource.Views;
using OxygenVK.Authorization;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace OxygenVK.AppSource.Authorization.Controls
{
	public sealed partial class HorizontalUserCard : UserControl
	{
		public delegate void ClickButtonDelete(AuthorizedUserCardsAttachment authorizedUserCardsAttachment);
		public event ClickButtonDelete ClickDelete;

		public Frame Frame;
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
			ClickDelete?.Invoke(AuthorizedUserCardsAttachment);
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
			new RootFrameNavigation(Frame, typeof(MainPage), new Authorize(new ListOfAuthorizedUsers().GetUserToken(AuthorizedUserCardsAttachment.UserID.ToString()), true).VkApi);
		}

		private void NewWindow_Click(object sender, RoutedEventArgs e)
		{
			new WindowGenerator(new Authorize(new ListOfAuthorizedUsers().GetUserToken(AuthorizedUserCardsAttachment.UserID.ToString()), true).VkApi);
		}

	}
}
