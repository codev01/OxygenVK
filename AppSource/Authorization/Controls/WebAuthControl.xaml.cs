using System;
using System.Linq;

using OxygenVK.Authorization;

using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Authorization.Controls
{
	public sealed partial class WebAuthControl : UserControl
	{
		public delegate void ClosingWebAuthControl();
		public event ClosingWebAuthControl Closing;
		private readonly string uri = "https://oauth.vk.com/authorize?client_id=6121396&scope=215989462&redirect_uri=https://oauth.vk.com/blank.html&display=page&response_type=token&revoke=1";

		public WebAuthControl()
		{
			InitializeComponent();
		}

		private async void wv_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
		{
			Expectation.Opacity = 0;
			Expectation.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
			try
			{
				string token = wv.Source.Fragment.
					Substring(1).Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries).
					Select(p => p.Split('=')).
					FirstOrDefault(p => p.Length == 2 && p[0] == "access_token")?[1] ??
					throw new Exception("Access token not found");

				if (token != "")
				{
					wv.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
					Expectation.Opacity = 1;
					Expectation.Visibility = Windows.UI.Xaml.Visibility.Visible;
					await WebView.ClearTemporaryWebDataAsync();
					Expectation.Opacity = 1;
					Expectation.Visibility = Windows.UI.Xaml.Visibility.Visible;
					new Authorize(token);
				}
			}
			catch
			{
				try
				{
					string[] error = args.Uri.Fragment.Split(new char[] { '#', '=' });
					if (error[1] == "error")
					{
						wv.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
						Expectation.Opacity = 1;
						Expectation.Visibility = Windows.UI.Xaml.Visibility.Visible;
						await WebView.ClearTemporaryWebDataAsync();
						Closing?.Invoke();
					}
				}
				catch { }
			}
			wv.Visibility = Windows.UI.Xaml.Visibility.Visible;
		}

		public void wv_Navigate()
		{
			Expectation.Opacity = 1;
			Expectation.Visibility = Windows.UI.Xaml.Visibility.Visible;
			wv.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
			wv.Navigate(new Uri(uri));
		}

		private void Rectangle_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			Closing?.Invoke();
		}

		private async void wv_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
		{
			AuthFailedDialog dialog = new AuthFailedDialog();
			ContentDialogResult result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				wv_Navigate();
			}
		}
	}
}
