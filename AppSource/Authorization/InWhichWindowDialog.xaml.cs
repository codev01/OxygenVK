
using OxygenVK.AppSource;
using OxygenVK.AppSource.Views;

using Windows.UI.Xaml.Controls;

namespace OxygenVK.Authorization
{
	public sealed partial class InWhichWindowDialog : ContentDialog
	{
		public Parameter Parameter { get; set; }
		public Frame Frame { get; set; }

		public InWhichWindowDialog()
		{
			InitializeComponent();
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			SaveProfailInfo();
			new RootFrameNavigation(Frame, typeof(MainPage), Parameter);
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			SaveProfailInfo();
			new WindowGenerator(new Parameter()
			{
				UserID = Parameter.UserID,
				VkApi = Parameter.VkApi
			});
		}

		private void SaveProfailInfo()
		{
			if (isSave.IsChecked == true)
			{
				new ListOfAuthorizedUsers().SetListOfAuthorizedUsers(Parameter.VkApi, Parameter.UserID);
			}
		}
	}
}
