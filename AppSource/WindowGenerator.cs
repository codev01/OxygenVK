using System;

using VkNet;

using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource
{
	internal class WindowGenerator
	{
		public WindowGenerator(VkApi vkApi)
		{
			Initialize(vkApi);
		}
		private async void Initialize(VkApi vkApi)
		{
			CoreApplicationView newView = CoreApplication.CreateNewView();
			int newViewId = 0;
			await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				Frame frame = new Frame();
				frame.Navigate(typeof(MainPage), null);

				Window.Current.Content = frame;

				ApplicationViewTitleBar appViewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
				appViewTitleBar.ButtonBackgroundColor = Colors.Transparent;
				appViewTitleBar.InactiveForegroundColor = Colors.Transparent;
				appViewTitleBar.InactiveBackgroundColor = Colors.Transparent;
				appViewTitleBar.ButtonInactiveForegroundColor = Colors.Transparent;
				appViewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

				CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
				coreTitleBar.ExtendViewIntoTitleBar = true;

				Window.Current.Activate();

				newViewId = ApplicationView.GetForCurrentView().Id;
			});
			bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
		}
	}
}
