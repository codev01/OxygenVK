using OxygenVK.AppSource.LocalSettings.Attachments;
using OxygenVK.AppSource.LocalSettings.Attachments;
using OxygenVK.AppSource.Views.Settings;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Authorization.Controls
{
	public class CardAttachmetsHelper
	{
		private readonly Frame Frame;
		private readonly SettingsAttachments Item;

		public CardAttachmetsHelper(Frame frame, SettingsAttachments item)
		{
			Item = item;
			Frame = frame;
		}

		public UserCard AddNewUserCard()
		{
			return new UserCard()
			{
				SettingsAttachments = GetSettingsAttachments(),
				Frame = Frame
			};
		}

		public HorizontalUserCard AddNewHorizontalUserCard()
		{
			return new HorizontalUserCard()
			{
				SettingsAttachments = GetSettingsAttachments(),
				Frame = Frame
			};
		}

		private SettingsAttachments GetSettingsAttachments()
		{
			return new SettingsAttachments()
			{
				UserDataAttachments = new UserDataAttachments
				{
					IsPasswordProtected = Item.UserDataAttachments.IsPasswordProtected,
					UserID = Item.UserDataAttachments.UserID,
					UserName = Item.UserDataAttachments.UserName,
					ScreenName = Item.UserDataAttachments.ScreenName,
					Token = Item.UserDataAttachments.Token,
					AvatarURL = Item.UserDataAttachments.AvatarURL
				},
				ApplicationSettings = new ApplicationSettingsAttachments
				{
					ElementTheme = ThemeHelper.RootTheme,
					ElementSoundPlayerState = ElementSoundPlayer.State,
					ElementSpatialAudioMode = ElementSoundPlayer.SpatialAudioMode
				}
			};
		}
	}
}
