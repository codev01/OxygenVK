using OxygenVK.AppSource.LocaSettings.Attachments;

using System;


using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace OxygenVK.AppSource.Authorization.Controls
{
	public sealed partial class UserCard : UserControl
	{
		public Frame Frame { get; set; }
		public Settings SettingsAttachments { get; set; }

		private CardControlHelper CardControlHelper;

		public UserCard() => InitializeComponent();

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			BitmapImage bitmapImage = new BitmapImage(new Uri(SettingsAttachments.UserDataAttachments.AvatarURL));
			if(SettingsAttachments.UserDataAttachments.AvatarURL != null)
			{
				bitmapImage.DecodePixelHeight = 100;
				bitmapImage.DecodePixelWidth = 100;

				avatar.ProfilePicture = bitmapImage;
			}
			userName.Text = SettingsAttachments.UserDataAttachments.UserName;

			CardControlHelper = new CardControlHelper(Frame, SettingsAttachments);
		}

		private void DeleteCard_Click(object sender, RoutedEventArgs e) => CardControlHelper.DeleteCard_Click();

		private void ScreenNameToolTip_Loaded(object sender, RoutedEventArgs e)
		{
			if(SettingsAttachments.UserDataAttachments.ScreenName == "")
			{
				screenNameToolTip.Visibility = Visibility.Collapsed;
			}
			else
			{
				screenNameToolTip.Content = SettingsAttachments.UserDataAttachments.ScreenName;
			}
		}

		private void ThisWindow_Click(object sender, RoutedEventArgs e) => CardControlHelper.ThisWindow_Click();

		private void NewWindow_Click(object sender, RoutedEventArgs e) => CardControlHelper.NewWindow_Click();
	}
}