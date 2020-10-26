using System;

using OxygenVK.AppSource.Views;
using OxygenVK.Authorization;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OxygenVK.AppSource
{
	public class WindowGenerator
	{
		public static int AuthorizationPageWindowID = 0;
		public static bool AuthorizationPageWindowOpened;

		private int newWindowID = 0;

		public WindowGenerator(object parameter, Type type)
		{
			if (parameter is LaunchActivatedEventArgs)
			{
				LaunchActivatedEventArgs e = parameter as LaunchActivatedEventArgs;

				if (!(Window.Current.Content is Frame frame))
				{
					frame = InitializationFrame();

					if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
					{
						//TODO: Загрузить состояние из ранее приостановленного приложения
					}
				}

				if (e.PrelaunchActivated == false)
				{
					if (frame.Content == null)
					{
						new RootFrameNavigation(frame, typeof(AuthorizationPage), e.Arguments);
					}

					Appearance();
					WindowActivate();
					DefinePage(frame);
				}
			}
			else
			{
				CreateNewWindow(parameter, type);
			}
		}

		private async void CreateNewWindow(object parameter, Type type)
		{
			CoreApplicationView newView = CoreApplication.CreateNewView();
			await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				Frame frame = InitializationFrame();

				new RootFrameNavigation(frame, type, parameter);

				Appearance();
				WindowActivate();
				DefinePage(frame);
			});
			await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newWindowID);
		}

		private Frame InitializationFrame()
		{
			Frame frame = new Frame();
			frame.NavigationFailed += OnNavigationFailed;
			Window.Current.Content = frame;
			return frame;
		}

		private void Appearance()
		{
			ApplicationViewTitleBar appViewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
			appViewTitleBar.ButtonBackgroundColor = Colors.Transparent;
			appViewTitleBar.InactiveForegroundColor = Colors.Transparent;
			appViewTitleBar.InactiveBackgroundColor = Colors.Transparent;
			appViewTitleBar.ButtonInactiveForegroundColor = Colors.Transparent;
			appViewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
			CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
			coreTitleBar.ExtendViewIntoTitleBar = true;
		}

		private void WindowActivate()
		{
			Window.Current.Activate();
		}

		private void DefinePage(Frame frame)
		{
			if (frame.SourcePageType.Name == "AuthorizationPage")
			{
				AuthorizationPageWindowID = ApplicationView.GetForCurrentView().Id;
				AuthorizationPageWindowOpened = true;
				SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += WindowGeneratorAuthorizationPage_CloseRequested;
			}

			newWindowID = ApplicationView.GetForCurrentView().Id;
		}

		private void WindowGeneratorAuthorizationPage_CloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
		{
			AuthorizationPageWindowOpened = false;
		}

		private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}
	}
}
