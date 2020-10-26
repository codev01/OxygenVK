using System;
using System.Reflection;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using OxygenVK.AppSource;
using OxygenVK.Authorization;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace OxygenVK
{
	public sealed partial class App : Application
	{
		public App()
		{
			AppCenter.Start("3c3e5a5d-e0bf-4620-a497-60b4b7fdf590", typeof(Analytics), typeof(Crashes));

			InitializeComponent();
			Suspending += OnSuspending;
		}

		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			new WindowGenerator(e, typeof(AuthorizationPage));
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
