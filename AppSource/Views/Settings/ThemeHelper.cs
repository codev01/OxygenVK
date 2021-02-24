using System;

using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace OxygenVK.AppSource.Views.Settings
{
	public static class ThemeHelper
	{
		private const string SelectedAppThemeKey = "SelectedAppTheme";
		private static Window CurrentApplicationWindow;
		private static UISettings uiSettings;
		public static ElementTheme ActualTheme
		{
			get
			{
				if(Window.Current.Content is FrameworkElement rootElement)
				{
					if(rootElement.RequestedTheme != ElementTheme.Default)
					{
						return rootElement.RequestedTheme;
					}
				}

				return App.GetEnum<ElementTheme>(Application.Current.RequestedTheme.ToString());
			}
		}

		public static ElementTheme RootTheme
		{
			get
			{
				if(Window.Current.Content is FrameworkElement rootElement)
				{
					return rootElement.RequestedTheme;
				}

				return ElementTheme.Default;
			}
			set
			{
				if(Window.Current.Content is FrameworkElement rootElement)
				{
					rootElement.RequestedTheme = value;
				}

				ApplicationData.Current.LocalSettings.Values[SelectedAppThemeKey] = value.ToString();
				UpdateSystemCaptionButtonColors();
			}
		}

		public static void Initialize()
		{
			CurrentApplicationWindow = Window.Current;
			string savedTheme = ApplicationData.Current.LocalSettings.Values[SelectedAppThemeKey]?.ToString();

			if(savedTheme != null)
			{
				RootTheme = App.GetEnum<ElementTheme>(savedTheme);
			}

			uiSettings = new UISettings();
			uiSettings.ColorValuesChanged += UiSettings_ColorValuesChanged;
		}

		private static async void UiSettings_ColorValuesChanged(UISettings sender, object args)
		{
			if(CurrentApplicationWindow != null)
			{
				await CurrentApplicationWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
						 {
							 UpdateSystemCaptionButtonColors();
						 });
			}
		}

		public static bool IsDarkTheme()
		{
			if(RootTheme == ElementTheme.Default)
			{
				return Application.Current.RequestedTheme == ApplicationTheme.Dark;
			}
			return RootTheme == ElementTheme.Dark;
		}

		public static void UpdateSystemCaptionButtonColors()
		{
			ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

			if(IsDarkTheme())
			{
				titleBar.ButtonForegroundColor = Colors.White;
			}
			else
			{
				titleBar.ButtonForegroundColor = Colors.Black;
			}
		}
	}
}
