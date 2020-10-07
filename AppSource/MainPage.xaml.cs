using System;
using System.Numerics;

using OxygenVK.AppSource.Views.Settings;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

using MUXC = Microsoft.UI.Xaml.Controls;

namespace OxygenVK.AppSource
{
	public sealed partial class MainPage : Page
	{
		public Enum displayMode;
		public bool paneIsOpen;
		public static double ContainerAdapterWidth { get; set; }

		public MainPage()
		{
			InitializeComponent();
			Window.Current.SetTitleBar(AppTitleBar);
		}
		private void Navigation_Loaded(object sender, RoutedEventArgs e)
		{
			//contentFrame.Navigate(typeof(NewsPage), null, new DrillInNavigationTransitionInfo());

		}

		private void contentFrame_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			try
			{
			}
			catch
			{
				//
			}
		}

		private void Navigation_SelectionChanged(MUXC.NavigationView sender, MUXC.NavigationViewSelectionChangedEventArgs args)
		{
			if (args.IsSettingsSelected)
			{
				contentFrame.Navigate(typeof(SettingsPage), null, new DrillInNavigationTransitionInfo());
			}
			switch (args.SelectedItemContainer.Tag.ToString())
			{
				case "news":
					//contentFrame.Navigate(typeof(NewsPage), null, new DrillInNavigationTransitionInfo());
					break;
				case "test":
					break;
			}
		}

		private void Navigation_DisplayModeChanged(MUXC.NavigationView sender, MUXC.NavigationViewDisplayModeChangedEventArgs args)
		{
			displayMode = args.DisplayMode;
			AppNameTextBlock_Margin(displayMode, paneIsOpen);
		}

		private void Navigation_PaneClosing(MUXC.NavigationView sender, MUXC.NavigationViewPaneClosingEventArgs args)
		{
			accaunts.Visibility = Visibility.Collapsed;
			accaunts.Opacity = 0;
			paneIsOpen = false;
			AppNameTextBlock_Margin(displayMode, paneIsOpen);
		}

		private void Navigation_PaneOpening(MUXC.NavigationView sender, object args)
		{
			accaunts.Visibility = Visibility.Visible;
			accaunts.Opacity = 1;
			paneIsOpen = true;
			AppNameTextBlock_Margin(displayMode, paneIsOpen);
		}

		public void AppNameTextBlock_Margin(Enum displayMode, bool paneIsOpen)
		{
			switch (displayMode)
			{
				case MUXC.NavigationViewDisplayMode.Minimal:
					if (paneIsOpen)
					{
						AppTitleBar.Margin = new Thickness(320, 0, 0, 0);
						AppNameTextBlock.Visibility = Visibility.Collapsed;
						AppNameTextBlock.Opacity = 0;
						accaunts.Margin = new Thickness(0, 4, 10, -4);
						accauntBorder.Width = 196;
					}
					else
					{
						AppTitleBar.Margin = new Thickness(80, 0, 0, 0);
						AppNameTextBlock.Visibility = Visibility.Visible;
						AppNameTextBlock.Opacity = 1;
						AppNameTextBlock.Translation = new Vector3(0, 0, 0);
						accaunts.Margin = new Thickness(0, 0, 10, 0);
						accauntBorder.Width = 230;
					}
					break;
				case MUXC.NavigationViewDisplayMode.Compact:
					AppTitleBar.Margin = new Thickness(40, 0, 0, 0);
					if (paneIsOpen)
					{
						AppNameTextBlock.Translation = new Vector3(0, 0, 0);
					}
					else
					{
						AppNameTextBlock.Translation = new Vector3(20, 0, 0);
					}
					break;
				case MUXC.NavigationViewDisplayMode.Expanded:

					AppTitleBar.Margin = new Thickness(40, 0, 0, 0);
					AppNameTextBlock.Visibility = Visibility.Visible;
					AppNameTextBlock.Opacity = 1;
					if (paneIsOpen)
					{
						AppNameTextBlock.Translation = new Vector3(0, 0, 0);
					}
					else
					{
						AppNameTextBlock.Translation = new Vector3(20, 0, 0);
					}
					break;
			}
		}
	}
}
