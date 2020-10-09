using System.Threading.Tasks;

using OxygenVK.AppSource;
using OxygenVK.AppSource.Views;

using VkNet;

using Windows.UI.Xaml.Controls;

namespace OxygenVK.Authorization
{
	public sealed partial class InWhichWindowDialog : ContentDialog
	{
		public VkApi VkApi { get; set; }
		public InWhichWindowDialog()
		{
			InitializeComponent();
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			Task.Run(SaveProfailInfo);
			new RootFrameNavigation(VkApi);
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			Task.Run(SaveProfailInfo);
			new WindowGenerator(VkApi);
		}

		private void SaveProfailInfo()
		{
			_ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				if (isSave.IsChecked == true)
				{
					new ListOfAuthorizedUsers().SetListOfAuthorizedUsers(VkApi);
				}
			});
		}
	}
}
