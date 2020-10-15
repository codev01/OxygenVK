using System;

using OxygenVK.AppSource.Views;

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
		private int newWindowID = 0;

		public WindowGenerator(Parameter parameter, Type type = null)
		{
			Initialize(parameter, type);
		}

		private async void Initialize(object parameter, Type type)
		{
			if (type == null)
			{
				type = typeof(MainPage);
			}

			CoreApplicationView newView = CoreApplication.CreateNewView();
			await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
		    {
			   Frame frame = new Frame();

			   new RootFrameNavigation(frame, type, parameter);

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

			   newWindowID = ApplicationView.GetForCurrentView().Id;
		    });
			MainPage.ListWindowID = newWindowID;
			NewWindowShowAsync(newWindowID);
		}

		public async void NewWindowShowAsync(int viewID)
		{
			await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newWindowID);
		}
	}
}
