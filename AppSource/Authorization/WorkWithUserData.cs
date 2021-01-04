using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OxygenVK.AppSource.LocalFolder;
using OxygenVK.AppSource.LocalSettings.Attachments;
using OxygenVK.AppSource.LocaSettings.Attachments;

using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

using static OxygenVK.AppSource.Authorization.ListUsersEvent;

namespace OxygenVK.AppSource.Authorization
{
	public class WorkWithUserData
	{
		public static event ListStartUpdate OnListStartUpdate;
		public static event ListUpdated OnListUpdated;

		public static List<SettingsAttachments> SettingsAttachments = new List<SettingsAttachments>();

		private readonly UserIDs UserIDs;
		private long UserID = 0;

		public WorkWithUserData()
		{
			UserIDs = Task.Run(() =>
			{
				return new UserIDs();
			}).Result;
		}

		public void UpdateList()
		{
			GetUsersData();
		}

		public async void ReceivingDataAndSave(VkApi VkApi, string pinCode = null)
		{
			OnListStartUpdate.Invoke();

			foreach (User item in await VkApi.Users.GetAsync(new long[0]))
			{
				UserID = item.Id;
			}

			DeleteUserData(UserID);

			string photoURL = null;
			foreach (Photo photo in await VkApi.Photo.GetAsync(new PhotoGetParams
			{
				AlbumId = PhotoAlbumType.Profile,
				Count = 1
			}))
			{
				photoURL = photo.Sizes.Last().Url.AbsoluteUri;
			}
			AccountSaveProfileInfoParams profileInfo = await VkApi.Account.GetProfileInfoAsync();

			Add(new SettingsAttachments
			{
				ApplicationSettings = new ApplicationSettingsAttachments(),
				UserDataAttachments = new UserDataAttachments
				{
					IsPasswordProtected = pinCode != null ? true : false,
					Token = pinCode != null ? Crypto.EncryptStringAES(VkApi.Token, pinCode) : VkApi.Token,
					UserID = UserID,
					AvatarURL = photoURL,
					ScreenName = profileInfo.ScreenName,
					UserName = profileInfo.FirstName + " " + profileInfo.LastName
				}
			});
		}

		private void Add(SettingsAttachments settingsAttachments)
		{
			UserIDs.Add(UserID);
			LocalSettings.LocalSettings.AddUserSettingsContainer(UserID.ToString(), settingsAttachments);
			GetUsersData();
		}

		private void GetUsersData()
		{
			SettingsAttachments.Clear();

			foreach (long item in UserIDs.GetUserIDs())
			{
				SettingsAttachments.Add(LocalSettings.LocalSettings.GetSettingsFromUserContainer(item.ToString()));
			}

			OnListUpdated.Invoke(SettingsAttachments);
		}

		public void DeleteUserData(long userID)
		{
			foreach (long item in UserIDs.GetUserIDs())
			{
				if (item == userID)
				{
					UserIDs.Delete(userID);
					LocalSettings.LocalSettings.DeleteUserContainerSettings(userID.ToString());
				}
			}
		}
	}
}
