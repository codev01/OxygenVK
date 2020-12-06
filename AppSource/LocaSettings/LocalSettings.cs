using OxygenVK.AppSource.LocalSettings.Attachments;

using Windows.Storage;

namespace OxygenVK.AppSource.LocalSettings
{
	public class LocalSettings
	{
		private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

		/// <summary>
		/// Добавляет новый контейнер в настройки
		/// </summary>
		/// <param name="containerName">
		/// "containerName" в данном случае именем контейнера выступает id пользователя
		/// </param>
		/// <param name="userSettingsAttachmentsValues">
		/// Вложения для сохранения
		/// </param>
		public static void AddUserContainerSettings(string containerName, UserSettingsAttachmentsValues userSettingsAttachmentsValues)
		{
			localSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);
			localSettings.Containers[containerName].Values[UserSettingsAttachments.UserID] = userSettingsAttachmentsValues.UserID;
			localSettings.Containers[containerName].Values[UserSettingsAttachments.UserName] = userSettingsAttachmentsValues.UserName;
			localSettings.Containers[containerName].Values[UserSettingsAttachments.Token] = userSettingsAttachmentsValues.Token;
			localSettings.Containers[containerName].Values[UserSettingsAttachments.AvatarURL] = userSettingsAttachmentsValues.AvatarURL;
			localSettings.Containers[containerName].Values[UserSettingsAttachments.ScreenName] = userSettingsAttachmentsValues.ScreenName;
		}

		/// <param name="containerName">
		/// "containerName" в данном случае именем контейнера выступает id пользователя
		/// </param>
		/// <returns>
		/// Все сохранённые данные пользователя
		/// </returns>
		public static UserSettingsAttachmentsValues GetUserContainerSettings(string containerName)
		{
			UserSettingsAttachmentsValues userSettingsAttachmentsValues = new UserSettingsAttachmentsValues();
			if (localSettings.Containers.ContainsKey(containerName))
			{
				userSettingsAttachmentsValues.UserID = (long)localSettings.Containers[containerName].Values[UserSettingsAttachments.UserID];
				userSettingsAttachmentsValues.UserName = (string)localSettings.Containers[containerName].Values[UserSettingsAttachments.UserName];
				userSettingsAttachmentsValues.Token = (string)localSettings.Containers[containerName].Values[UserSettingsAttachments.Token];
				userSettingsAttachmentsValues.AvatarURL = (string)localSettings.Containers[containerName].Values[UserSettingsAttachments.AvatarURL];
				userSettingsAttachmentsValues.ScreenName = (string)localSettings.Containers[containerName].Values[UserSettingsAttachments.ScreenName];	
			}
			else
			{
				throw new System.Exception("Контейнера с таким именем нет");
			}
			return userSettingsAttachmentsValues;
		}

		/// <summary>
		/// Удалят контейнер с сохранёнными данными пользователя
		/// </summary>
		/// <param name="containerName">
		/// "containerName" в данном случае именем контейнера выступает id пользователя
		/// </param>
		public static void DeleteUserContainerSettings(string containerName)
		{
			localSettings.DeleteContainer(containerName);
		}
	}
}
