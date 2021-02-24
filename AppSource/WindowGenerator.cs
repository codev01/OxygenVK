
using System;

using OxygenVK.AppSource.Authorization;
using OxygenVK.AppSource.Views;

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
			if(parameter is LaunchActivatedEventArgs)
			{
				LaunchActivatedEventArgs e = parameter as LaunchActivatedEventArgs;

				if(Window.Current.Content is not Frame frame)
				{
					frame = InitializationFrame();

					if(e.PreviousExecutionState == ApplicationExecutionState.Terminated)
					{
						//TODO: Загрузить состояние из ранее приостановленного приложения
					}
				}

				if(e.PrelaunchActivated == false)
				{
					if(frame.Content == null)
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
			await newView.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
			{
				Frame frame = InitializationFrame();

				new RootFrameNavigation(frame, type, parameter);

				Appearance();
				WindowActivate();
				DefinePage(frame);
				_ = ApplicationViewSwitcher.TryShowAsStandaloneAsync(newWindowID, ViewSizePreference.Default);
			});
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
			SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += WindowGeneratorAuthorizationPage_CloseRequested;
		}

		private void DefinePage(Frame frame)
		{
			ApplicationView applicationView = ApplicationView.GetForCurrentView();
			if(frame.SourcePageType.Name == "AuthorizationPage")
			{
				AuthorizationPageWindowID = applicationView.Id;
				AuthorizationPageWindowOpened = true;
			}
			newWindowID = applicationView.Id;
		}

		private void WindowGeneratorAuthorizationPage_CloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
		{
			new WindowNamesHelper(ApplicationView.GetForCurrentView().Title).RemoveNameWindow();
			AuthorizationPageWindowOpened = false;
		}

		private void OnNavigationFailed(object sender, NavigationFailedEventArgs e) => throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
	}
}
