using OxygenVK.AppSource;
using OxygenVK.AppSource.Views;

using Windows.UI.Xaml.Controls;

namespace OxygenVK.Authorization
{
	public sealed partial class InWhichWindowDialog : ContentDialog
	{
		public Frame Frame { get; set; }
		public Parameter Parameter;

		public InWhichWindowDialog()
		{
			InitializeComponent();
		}

		private void SaveProfailInfo()
		{
			if (isSave.IsChecked == true)
			{
				ListOfAuthorizedUsers listOfAuthorizedUsers = new ListOfAuthorizedUsers();
				listOfAuthorizedUsers.SetListOfAuthorizedUsersAsync(Parameter.VkApi, Parameter.UserID);
				listOfAuthorizedUsers.InitializeList();
			}
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			SaveProfailInfo();
			new RootFrameNavigation(Frame, typeof(MainPage), Parameter);
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			SaveProfailInfo();
			new WindowGenerator(Parameter, typeof(MainPage));
		}
	}
}
