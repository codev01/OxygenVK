
using System;

using OxygenVK.AppSource.Authorization.DialogBoxes;
using OxygenVK.AppSource.LocaSettings;
using OxygenVK.AppSource.LocaSettings.Attachments;
using OxygenVK.AppSource.Views;

using VkNet;
using VkNet.Model;

using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Authorization.Controls
{
	public class CardControlHelper
	{
		private readonly Frame Frame;
		private readonly Settings SettingsAttachments;

		private string Token;

		public CardControlHelper(Frame frame, Settings settingsAttachments)
		{
			Frame = frame;
			SettingsAttachments = settingsAttachments;
			Token = settingsAttachments.UserDataAttachments.Token;
		}

		public void DeleteCard_Click()
		{
			WorkWithUserData workWithUserData = new WorkWithUserData();
			workWithUserData.DeleteUserData(SettingsAttachments.UserDataAttachments.UserID);
			workWithUserData.UpdateList();
		}

		private Parameter GetParameter()
		{
			VkApi vkApi = Authorize.AuthorizeAsync(Token);
			return new Parameter
			{
				ApplicationSettings = LocalSettings.GetUserSettingsContainer(GetUserID()).ApplicationSettings,
				VkApi = vkApi
			};

			long GetUserID()
			{
				foreach(User item in vkApi.Users.Get(new long[0]))
				{
					return item.Id;
				}
				return 0;
			}
		}

		public void ThisWindow_Click()
		{
			if(SettingsAttachments.UserDataAttachments.IsPasswordProtected)
			{
				WhichWindow = InWhichWindow.This;
				Verification();
			}
			else
			{
				NextThis();
			}
		}

		public void NewWindow_Click()
		{
			if(SettingsAttachments.UserDataAttachments.IsPasswordProtected)
			{
				WhichWindow = InWhichWindow.New;
				Verification();
			}
			else
			{
				NextNew();
			}
		}

		private void NextThis() => new RootFrameNavigation(Frame, typeof(MainPage), GetParameter());

		private void NextNew() => new WindowGenerator(GetParameter(), typeof(MainPage));

		private InWhichWindow WhichWindow;
		private enum InWhichWindow
		{
			New,
			This
		}
		private VerificationDialog VerificationDialog;
		private async void Verification()
		{
			VerificationDialog = new VerificationDialog()
			{
				DecryptionToken = SettingsAttachments.UserDataAttachments.Token
			};
			VerificationDialog.Closed += VerificationDialog_Closed;
			await VerificationDialog.ShowAsync();
		}

		private void VerificationDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
		{
			if(SettingsAttachments.UserDataAttachments.Token != VerificationDialog.DecryptionToken)
			{
				Token = VerificationDialog.DecryptionToken;
				if(WhichWindow == InWhichWindow.This)
				{
					NextThis();
				}
				if(WhichWindow == InWhichWindow.New)
				{
					NextNew();
				}
			}
		}
	}
}
