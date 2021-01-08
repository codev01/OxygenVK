using Windows.Storage;
using Windows.UI.Xaml;

namespace OxygenVK.AppSource.LocalSettings.Attachments
{
	public class ApplicationSettingsAttachments
	{
		public static long UserID { get; set; } = 0;

		public enum ApplicationSettingsIndex
		{
			ElementTheme,
			ElementSoundPlayerState,
			ElementSpatialAudioMode
		}

		public ElementTheme ElementTheme
		{
			get => elementTheme;
			set
			{
				if (value != elementTheme)
				{
					elementTheme = value;
					if (IsSet)
					{
						LocalSettings.SetSettingCell(ApplicationSettingsIndex.ElementTheme, UserID, value, SettingsCategory, ApplicationDataCompositeValue);
					}
				}
			}
		}
		public ElementSoundPlayerState ElementSoundPlayerState
		{
			get => elementSoundPlayerState;
			set
			{
				if (value != elementSoundPlayerState)
				{
					elementSoundPlayerState = value;
					if (IsSet)
					{
						LocalSettings.SetSettingCell(ApplicationSettingsIndex.ElementSoundPlayerState, UserID, value, SettingsCategory, ApplicationDataCompositeValue);
					}
				}
			}
		}
		public ElementSpatialAudioMode ElementSpatialAudioMode
		{
			get => elementSpatialAudioMode;
			set
			{
				if (value != elementSpatialAudioMode)
				{
					elementSpatialAudioMode = value;
					if (IsSet)
					{
						LocalSettings.SetSettingCell(ApplicationSettingsIndex.ElementSpatialAudioMode, UserID, value, SettingsCategory, ApplicationDataCompositeValue);
					}
				}
			}
		}

		public bool IsSet = false;
		public ApplicationDataCompositeValue ApplicationDataCompositeValue { get; set; }
		private LocalSettings.SettingsCategory SettingsCategory { get; } = LocalSettings.SettingsCategory.ApplicationSettings;

		private ElementTheme elementTheme = ElementTheme.Default;
		private ElementSoundPlayerState elementSoundPlayerState = ElementSoundPlayerState.Off;
		private ElementSpatialAudioMode elementSpatialAudioMode = ElementSpatialAudioMode.Off;
	}
}
