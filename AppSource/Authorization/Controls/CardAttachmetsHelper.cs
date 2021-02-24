using OxygenVK.AppSource.LocaSettings.Attachments;
using OxygenVK.AppSource.Views.Settings;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Authorization.Controls
{
	public class CardAttachmetsHelper
	{
		private readonly Frame Frame;
		private readonly Settings Item;

		public CardAttachmetsHelper(Frame frame, Settings item)
		{
			Item = item;
			Frame = frame;
		}

		public UserCard AddNewUserCard() => new UserCard()
		{
			SettingsAttachments = GetSettingsAttachments(),
			Frame = Frame
		};

		public HorizontalUserCard AddNewHorizontalUserCard() => new HorizontalUserCard()
		{
			SettingsAttachments = GetSettingsAttachments(),
			Frame = Frame
		};

		private Settings GetSettingsAttachments() => new Settings()
		{
			UserDataAttachments = new UserData
			{
				IsPasswordProtected = Item.UserDataAttachments.IsPasswordProtected,
				UserID = Item.UserDataAttachments.UserID,
				UserName = Item.UserDataAttachments.UserName,
				ScreenName = Item.UserDataAttachments.ScreenName,
				Token = Item.UserDataAttachments.Token,
				AvatarURL = Item.UserDataAttachments.AvatarURL
			},
			ApplicationSettings = new ApplicationSettings
			{
				ElementTheme = ThemeHelper.RootTheme,
				ElementSoundPlayerState = ElementSoundPlayer.State,
				ElementSpatialAudioMode = ElementSoundPlayer.SpatialAudioMode
			}
		};
	}
}
