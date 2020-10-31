using OxygenVK.AppSource;
using OxygenVK.AppSource.Authorization;
using OxygenVK.AppSource.Views;

using VkNet;

using Windows.UI.Xaml.Controls;

namespace OxygenVK.Authorization
{
	public sealed partial class InWhichWindowDialog : ContentDialog
	{
		public Frame Frame { get; set; }
		public string Token;

		public InWhichWindowDialog()
		{
			InitializeComponent();
		}

		private VkApi GetParameter()
		{
			Authorize authorize;

			if (isSave.IsChecked.Value)
			{
				authorize = new Authorize(Token);
			}
			else
			{
				authorize = new Authorize(Token, true);
			}

			return authorize.VkApi;
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			new RootFrameNavigation(Frame, typeof(MainPage), GetParameter());
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			new WindowGenerator(GetParameter(), typeof(MainPage));
		}
	}
}
