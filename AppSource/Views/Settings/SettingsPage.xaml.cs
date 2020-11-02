using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Views.Settings
{
	public sealed partial class SettingsPage : Page
	{
		public SettingsPage()
		{
			InitializeComponent();
			NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
		}
		private void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e)
		{
			string selectedTheme = ((RadioButton)sender)?.Tag?.ToString();

			if (selectedTheme != null)
			{
				ThemeHelper.RootTheme = App.GetEnum<ElementTheme>(selectedTheme);
			}
		}

		private void soundToggle_Toggled(object sender, RoutedEventArgs e)
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
		}

		private void spatialSoundBox_Checked(object sender, RoutedEventArgs e)
		{
			if (soundToggle.IsOn == true)
			{
				ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.On;
			}
		}

		private void spatialSoundBox_Unchecked(object sender, RoutedEventArgs e)
		{
			if (soundToggle.IsOn == true)
			{
				ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
			}
		}
	}
}
