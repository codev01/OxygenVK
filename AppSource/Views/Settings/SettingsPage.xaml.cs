
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OxygenVK.AppSource.Views.Settings
{
	public sealed partial class SettingsPage : Page
	{
		private Parameter Parameter;

		public SettingsPage()
		{
			InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Enabled;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			Parameter = e?.Parameter as Parameter;
			ThemeHelper.RootTheme = App.GetEnum<ElementTheme>(Parameter.ApplicationSettings.ElementTheme.ToString());
			ElementSoundPlayer.State = Parameter.ApplicationSettings.ElementSoundPlayerState;
			ElementSoundPlayer.SpatialAudioMode = Parameter.ApplicationSettings.ElementSpatialAudioMode;
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
