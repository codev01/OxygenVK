using OxygenVK.AppSource.Views.Settings;

using VkNet;

using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Authorization.DialogBoxes
{
	public sealed partial class VerificationDialog : ContentDialog
	{
		public string DecryptionToken { get; set; }
		public VkApi Vkapi { get; set; }

		public VerificationDialog()
		{
			InitializeComponent();
			RequestedTheme = ThemeHelper.RootTheme;
		}

		private void PrimaryButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			if(Pass.Password.Length == 0)
			{
				InvalidPass.Text = "Чтобы продолжить, нужно заполнить \nполе";
				InvalidPass.Visibility = Windows.UI.Xaml.Visibility.Visible;
			}
			else if(Pass.Password.Length > 0 && Pass.Password.Length < 4)
			{
				InvalidPass.Text = "ПИН-код должен содержать не менее 4 \nсимволов";
				InvalidPass.Visibility = Windows.UI.Xaml.Visibility.Visible;
			}
			else if(Pass.Password.Length >= 4)
			{
				try
				{
					DecryptionToken = Authorize.AuthorizeAsync(Crypto.DecryptStringAES(DecryptionToken, Pass.Password)).Token;
					Hide();
				}
				catch
				{
					InvalidPass.Text = "Неверный ПИН-код";
					InvalidPass.Visibility = Windows.UI.Xaml.Visibility.Visible;
				}
			}
		}

		private void SecondaryButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => Hide();
	}
}
