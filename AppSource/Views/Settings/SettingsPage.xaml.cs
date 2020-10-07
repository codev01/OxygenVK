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
		}
		private void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e)
		{
			var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();
			ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

			if (selectedTheme != null)
			{
				ThemeHelper.RootTheme = App.GetEnum<ElementTheme>(selectedTheme);
			}
		}
	}
}
