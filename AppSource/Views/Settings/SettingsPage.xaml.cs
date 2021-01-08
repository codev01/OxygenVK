using OxygenVK.AppSource.LocalSettings.Attachments;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OxygenVK.AppSource.Views.Settings
{
	public sealed partial class SettingsPage : Page
	{
		private Parameter Parameter;
		private long UserID = 0;

		public SettingsPage()
		{
			InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Enabled;
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			Parameter = e?.Parameter as Parameter;
			Parameter.ApplicationSettings.IsSet = true;
			if (UserID == 0)
			{
				foreach (VkNet.Model.User item in await Parameter.VkApi.Users.GetAsync(new long[0]))
				{
					ApplicationSettingsAttachments.UserID = item.Id;
				}
			}

			switch (Parameter.ApplicationSettings.ElementTheme)
			{
				case ElementTheme.Light:
					ThemeTag1.IsChecked = true;
					break;
				case ElementTheme.Dark:
					ThemeTag2.IsChecked = true;
					break;
				case ElementTheme.Default:
					ThemeTag3.IsChecked = true;
					break;
			}

			switch (Parameter.ApplicationSettings.ElementSoundPlayerState)
			{
				case ElementSoundPlayerState.Off:
					soundToggle.IsOn = false;
					break;
				case ElementSoundPlayerState.On:
					soundToggle.IsOn = true;
					break;
			}

			switch (Parameter.ApplicationSettings.ElementSpatialAudioMode)
			{
				case ElementSpatialAudioMode.Off:
					spatialSoundBox.IsChecked = false;
					break;
				case ElementSpatialAudioMode.On:
					spatialSoundBox.IsChecked = true;
					break;
			}

			//ThemeHelper.RootTheme = App.GetEnum<ElementTheme>(Parameter.ApplicationSettings.ElementTheme.ToString());
			//ElementSoundPlayer.State = Parameter.ApplicationSettings.ElementSoundPlayerState;
			//ElementSoundPlayer.SpatialAudioMode = Parameter.ApplicationSettings.ElementSpatialAudioMode;
		}

		private void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e)
		{
			string selectedTheme = ((RadioButton)sender)?.Tag?.ToString();

			if (selectedTheme != null)
			{
				ThemeHelper.RootTheme = App.GetEnum<ElementTheme>(selectedTheme);
				Parameter.ApplicationSettings.ElementTheme = ThemeHelper.RootTheme;
			}
		}

		private void SoundToggle_Toggled(object sender, RoutedEventArgs e)
		{
			if (soundToggle.IsOn == true)
			{
				spatialSoundBox.IsEnabled = true;
				ElementSoundPlayer.State = ElementSoundPlayerState.On;
			}
			else
			{
				spatialSoundBox.IsEnabled = false;
				spatialSoundBox.IsChecked = false;

				ElementSoundPlayer.State = ElementSoundPlayerState.Off;
				ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
			}
			Parameter.ApplicationSettings.ElementSoundPlayerState = ElementSoundPlayer.State;
			Parameter.ApplicationSettings.ElementSpatialAudioMode = ElementSoundPlayer.SpatialAudioMode;
		}

		private void SpatialSoundBox_Checked(object sender, RoutedEventArgs e)
		{
			if (soundToggle.IsOn == true)
			{
				ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.On;
				Parameter.ApplicationSettings.ElementSpatialAudioMode = ElementSoundPlayer.SpatialAudioMode;
			}
		}

		private void SpatialSoundBox_Unchecked(object sender, RoutedEventArgs e)
		{
			if (soundToggle.IsOn == true)
			{
				ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
				Parameter.ApplicationSettings.ElementSpatialAudioMode = ElementSoundPlayer.SpatialAudioMode;
			}
		}
	}
}
