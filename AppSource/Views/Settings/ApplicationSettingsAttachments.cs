using Windows.UI.Xaml;

namespace OxygenVK.AppSource.LocaSettings.Attachments
{
	public class ApplicationSettingsAttachments
	{
		public ElementTheme ElementTheme { get; set; } = ElementTheme.Default;
		public ElementSoundPlayerState ElementSoundPlayerState { get; set; } = ElementSoundPlayerState.Off;
		public ElementSpatialAudioMode ElementSpatialAudioMode { get; set; } = ElementSpatialAudioMode.Off;
	}
}
