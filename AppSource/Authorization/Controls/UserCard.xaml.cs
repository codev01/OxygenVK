using System;

using OxygenVK.AppSource;
using OxygenVK.AppSource.Views;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OxygenVK.Authorization.Controls
{
	public sealed partial class UserCard : UserControl
	{
		public delegate void ClickButtonDelete(AuthorizedUserCardsAttachment authorizedUserCardsAttachment);
		public event ClickButtonDelete ClickDelete;

		public UserCard()
		{
			InitializeComponent();
		}

		private void deleteCard_Click(object sender, RoutedEventArgs e)
		{
			ClickDelete?.Invoke(new AuthorizedUserCardsAttachment
			{
				UserID = Convert.ToInt64(userID.Text)
			});
		}

		private void screenNameToolTip_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (screenNameToolTip.Content.ToString() == "")
				{
					screenNameToolTip.Visibility = Visibility.Collapsed;
				}
			}
			catch
			{
				screenNameToolTip.Visibility = Visibility.Collapsed;
			}
		}

		private void ThisWindow_Click(object sender, RoutedEventArgs e)
		{
			new RootFrameNavigation(new Authorization(new ListOfAuthorizedUsers().GetUserToken(userID.Text), true).VkApi);
		}

		private void NewWindow_Click(object sender, RoutedEventArgs e)
		{
			new WindowGenerator(new Authorization(new ListOfAuthorizedUsers().GetUserToken(userID.Text), true).VkApi);
		}
	}
}
