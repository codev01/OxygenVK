using System;

using OxygenVK.AppSource.LocalSettings.Attachments;
using OxygenVK.AppSource.Views;

using VkNet;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace OxygenVK.AppSource.Authorization.Controls
{
	public sealed partial class HorizontalUserCard : UserControl
	{
		public Frame Frame { get; set; }
		public UserSettingsAttachmentsValues UserSettingsAttachmentsValues { get; set; }

		public HorizontalUserCard()
		{
			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			avatar.ProfilePicture = new BitmapImage(new Uri(UserSettingsAttachmentsValues.AvatarURL));
			userName.Text = UserSettingsAttachmentsValues.UserName;
		}

		private void deleteCard_Click(object sender, RoutedEventArgs e)
		{
			WorkWithUserData workWithUserData = new WorkWithUserData();
			workWithUserData.DeleteUserData(UserSettingsAttachmentsValues.UserID);
			workWithUserData.UpdateList();
		}

		private void screenNameToolTip_Loaded(object sender, RoutedEventArgs e)
		{
			if (UserSettingsAttachmentsValues.ScreenName == null)
			{
				screenNameToolTip.Visibility = Visibility.Collapsed;
			}
			else
			{
				screenNameToolTip.Content = UserSettingsAttachmentsValues.ScreenName;
			}
		}

		private VkApi GetParameter()
		{
			return new Authorize(UserSettingsAttachmentsValues.Token, true).VkApi;
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
