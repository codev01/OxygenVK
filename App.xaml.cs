using System;
using System.Reflection;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using OxygenVK.AppSource;
using OxygenVK.AppSource.Views;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OxygenVK
{
	public sealed partial class App : Application
	{
		private Frame RootFrame;
		public App()
		{
			AppCenter.Start("3c3e5a5d-e0bf-4620-a497-60b4b7fdf590", typeof(Analytics), typeof(Crashes));

			InitializeComponent();
			Suspending += OnSuspending;
			RootFrameNavigation.OnFrameNavigation += UserCard_OnFrameNavigation;
		}

		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			if (!(Window.Current.Content is Frame rootFrame))
			{
				rootFrame = new Frame();

				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					//TODO: Загрузить состояние из ранее приостановленного приложения
				}

				Window.Current.Content = rootFrame;
			}

			if (e.PrelaunchActivated == false)
			{
				if (rootFrame.Content == null)
				{
					rootFrame.Navigate(typeof(Authorization.AuthorizationPage), e.Arguments);
				}

				ApplicationViewTitleBar appViewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
				appViewTitleBar.ButtonBackgroundColor = Colors.Transparent;
				appViewTitleBar.InactiveForegroundColor = Colors.Transparent;
				appViewTitleBar.InactiveBackgroundColor = Colors.Transparent;
				appViewTitleBar.ButtonInactiveForegroundColor = Colors.Transparent;
				appViewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
				CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
				coreTitleBar.ExtendViewIntoTitleBar = true;

				Window.Current.Activate();
				RootFrame = rootFrame;
			}
		}

		private void UserCard_OnFrameNavigation(VkNet.VkApi vkApi)
		{
			RootFrame.Navigate(typeof(MainPage));
		}

		private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Сохранить состояние приложения и остановить все фоновые операции
			deferral.Complete();
		}

		public static TEnum GetEnum<TEnum>(string text) where TEnum : struct
		{
			if (!typeof(TEnum).GetTypeInfo().IsEnum)
			{
				throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
			}
			return (TEnum)Enum.Parse(typeof(TEnum), text);
		}
	}
}
