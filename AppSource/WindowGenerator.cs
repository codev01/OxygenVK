using System;

using OxygenVK.AppSource.Views;

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
		public WindowGenerator(Parameter parameter)
		{
			Initialize(parameter);
		}
		private async void Initialize(object parameter)
		{
			CoreApplicationView newView = CoreApplication.CreateNewView();
			int newViewId = 0;
			await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				Frame frame = new Frame();

				new RootFrameNavigation(frame, typeof(MainPage), parameter);

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

	public class Parameter
	{
		public VkApi VkApi;
		public long UserID;
	}
}
