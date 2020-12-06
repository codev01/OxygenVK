using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OxygenVK.AppSource.LocalFolder;
using OxygenVK.AppSource.LocalSettings.Attachments;

using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;

using static OxygenVK.AppSource.Authorization.ListUsersEvent;

namespace OxygenVK.AppSource.Authorization
{
	public class WorkWithUserData
	{
		public static event ListStartUpdate OnListStartUpdate;
		public static event ListUpdated OnListUpdated;

		public static List<UserSettingsAttachmentsValues> UserSettingsAttachmentsValues = new List<UserSettingsAttachmentsValues>();

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

		public async void Virification(VkApi vkApi)
		{
			OnListStartUpdate.Invoke();

			foreach (User item in await vkApi.Users.GetAsync(new long[0]))
			{
				UserID = item.Id;
			}
			DeleteUserData(UserID);

			string photoURL = null;
			foreach (VkNet.Model.Attachments.Photo photo in await vkApi.Photo.GetAsync(new VkNet.Model.RequestParams.PhotoGetParams
			{
				AlbumId = PhotoAlbumType.Profile,
				Count = 1
			}))
			{
				photoURL = photo.Sizes.Last().Url.AbsoluteUri;
			}
			VkNet.Model.RequestParams.AccountSaveProfileInfoParams profileInfo = await vkApi.Account.GetProfileInfoAsync();

			Add(profileInfo.FirstName + " " + profileInfo.LastName, photoURL, vkApi.Token, UserID, profileInfo.ScreenName);
		}

		private void Add(string userName, string photoURL, string token, long userID, string screenName)
		{
			UserIDs.Add(userID);
			LocalSettings.LocalSettings.AddUserContainerSettings(
				userID.ToString(),
				new UserSettingsAttachmentsValues
				{
					UserName = userName,
					AvatarURL = photoURL,
					Token = token,
					UserID = userID,
					ScreenName = screenName
				});
			GetUsersData();
		}

		private void GetUsersData()
		{
			UserSettingsAttachmentsValues.Clear();

			foreach (long item in UserIDs.GetUserIDs())
			{
				UserSettingsAttachmentsValues.Add(LocalSettings.LocalSettings.GetUserContainerSettings(item.ToString()));
			}

			OnListUpdated.Invoke(UserSettingsAttachmentsValues);
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
