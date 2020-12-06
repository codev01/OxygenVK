using System;
using System.Collections.Generic;

using OxygenVK.AppSource.Authorization;
using OxygenVK.AppSource.Authorization.Controls;
using OxygenVK.AppSource.LocalSettings.Attachments;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OxygenVK.Authorization
{
	public partial class AuthorizationPage : Page
	{
		public static bool ThePageIsUsedInNavigation;
		private bool thePageIsUsedInNavigation;

		public AuthorizationPage()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			Frame.Navigating += Frame_Navigating;
			ThePageIsUsedInNavigation = false;
			thePageIsUsedInNavigation = false;

			WorkWithUserData.OnListStartUpdate += WorkWithUserData_OnListStartUpdate;
			WorkWithUserData.OnListUpdated += WorkWithUserData_OnListUpdated;

			new WorkWithUserData().UpdateList();
		}

		private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			ThePageIsUsedInNavigation = true;
			thePageIsUsedInNavigation = true;
		}

		private void ListUsersCard_GridView_Add(List<UserSettingsAttachmentsValues> authorizedUserCardsAttachments)
		{
			cardAddButton.Content = "Войти в другой аккаунт";
			borderHintRecentlyLoggedIn.Visibility = Visibility.Visible;
			listUsersCard_GridView.Items.Clear();

			if (authorizedUserCardsAttachments.Count != 0)
			{
				foreach (UserSettingsAttachmentsValues item in authorizedUserCardsAttachments)
				{
					UserCard userCard = new UserCard
					{
						UserSettingsAttachmentsValues = new UserSettingsAttachmentsValues
						{
							UserID = item.UserID,
							UserName = item.UserName,
							ScreenName = item.ScreenName,
							Token = item.Token,
							 AvatarURL = item.AvatarURL,
						},
						Frame = Frame
					};
					listUsersCard_GridView.Items.Add(userCard);
				}
			}
			else
			{
				webAuthControl_Closing();
				borderHintRecentlyLoggedIn.Visibility = Visibility.Collapsed;
				cardAddButton.Content = "Войти в аккаунт";
				listUsersCard_GridView.Items.Clear();
				cardAdd_Click(this, null);
			}
		}

		private void WorkWithUserData_OnListStartUpdate()
		{
			_ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				listUpdatedProgress_SetVisibility(Visibility.Visible);
			});
		}

		private void WorkWithUserData_OnListUpdated(List<AppSource.LocalSettings.Attachments.UserSettingsAttachmentsValues> authorizedUserCardsAttachments)
		{
			_ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				listUpdatedProgress_SetVisibility(Visibility.Collapsed);
				ListUsersCard_GridView_Add(authorizedUserCardsAttachments);
			});
		}

		private void listUpdatedProgress_SetVisibility(Visibility visibility)
		{
			if (visibility == Visibility.Visible)
			{
				if (WorkWithUserData.UserSettingsAttachmentsValues.Count == 0)
				{
					listUpdatedProgress.Visibility = Visibility.Visible;
					listUpdatedProgress.IsActive = true;
				}
				else
				{
					listUpdatedProgress2.Visibility = Visibility.Visible;
					listUpdatedProgress2.IsActive = true;
				}
			}
			if (visibility == Visibility.Collapsed)
			{
				listUpdatedProgress.Visibility = Visibility.Collapsed;
				listUpdatedProgress2.Visibility = Visibility.Collapsed;
				listUpdatedProgress.IsActive = false;
				listUpdatedProgress2.IsActive = false;
			}
		}

		private void cardAdd_Click(object sender, RoutedEventArgs e)
		{
			webAuthControl.wv_Navigate();
			webAuthControl.Opacity = 1;
			webAuthControl.Visibility = Visibility.Visible;
		}

		private void webAuthControl_Closing()
		{
			webAuthControl.Opacity = 0;
			webAuthControl.Visibility = Visibility.Collapsed;
		}

		private void webAuthControl_OnAuthCompleted(string token)
		{
			if (!thePageIsUsedInNavigation)
			{
				_ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
				{
					webAuthControl_Closing();
					InWhichWindowDialog dialog = new InWhichWindowDialog
					{
						Frame = Frame,
						Token = token
					};
					await dialog.ShowAsync();
				});
			}
		}

		private void headerListPanel_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.NewSize.Width < 655)
			{
				hintRecentlyLoggedIn.Width = 444 - 48;
			}
			else
			{
				hintRecentlyLoggedIn.Width = 500;
			}
		}
	}
}
