using System;

using OxygenVK.AppSource.Views;
using OxygenVK.Authorization;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Authorization.Controls
{
	public sealed partial class HorizontalUserCard : UserControl
	{
		public delegate void ClickButtonDelete(AuthorizedUserCardsAttachment authorizedUserCardsAttachment);
		public event ClickButtonDelete ClickDelete;

		public HorizontalUserCard()
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
			new RootFrameNavigation(new Authorize(new ListOfAuthorizedUsers().GetUserToken(userID.Text), true).VkApi);
		}

		private void NewWindow_Click(object sender, RoutedEventArgs e)
		{
			new WindowGenerator(new Authorize(new ListOfAuthorizedUsers().GetUserToken(userID.Text), true).VkApi);
		}
	}
}
