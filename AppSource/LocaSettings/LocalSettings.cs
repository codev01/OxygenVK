using System;

using OxygenVK.AppSource.LocalSettings.Attachments;
using OxygenVK.AppSource.LocaSettings.Attachments;

using Windows.Storage;
using Windows.UI.Xaml;

namespace OxygenVK.AppSource.LocalSettings
{
	public class LocalSettings
	{
		private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
		private static readonly CompositeValueNames CompositeValueNames = new CompositeValueNames();

		/// <summary>
		/// Добавляет новый контейнер в настройки
		/// </summary>
		/// <param name="containerName">
		/// "containerName" в данном случае именем контейнера выступает id пользователя
		/// </param>
		/// <param name="settingsAttachments">
		/// Вложения для сохранения
		/// </param>
		public static void AddUserSettingsContainer(string containerName, SettingsAttachments settingsAttachments)
		{
			UserDataAttachmentNames userDataAttachmentNames = CompositeValueNames.UserDataAttachmentNames;
			ApplicationSettingsAttachmentNames applicationSettingsNames = CompositeValueNames.ApplicationSettingsAttachmentNames;
			ApplicationDataCompositeValue userDataAttachmentsComposite = new ApplicationDataCompositeValue();
			ApplicationDataCompositeValue applicationSettingsComposite = new ApplicationDataCompositeValue();

			UserDataAttachments userDataAttachments = settingsAttachments.UserDataAttachments;
			ApplicationSettingsAttachments applicationSettingsAttachments = settingsAttachments.ApplicationSettings;

			localSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);

			userDataAttachmentsComposite[userDataAttachmentNames.IsPasswordProtected] = userDataAttachments.IsPasswordProtected;
			userDataAttachmentsComposite[userDataAttachmentNames.UserID] = userDataAttachments.UserID;
			userDataAttachmentsComposite[userDataAttachmentNames.UserName] = userDataAttachments.UserName;
			userDataAttachmentsComposite[userDataAttachmentNames.Token] = userDataAttachments.Token;
			userDataAttachmentsComposite[userDataAttachmentNames.AvatarURL] = userDataAttachments.AvatarURL;
			userDataAttachmentsComposite[userDataAttachmentNames.ScreenName] = userDataAttachments.ScreenName;

			applicationSettingsComposite[applicationSettingsNames.ElementTheme] = (int)applicationSettingsAttachments.ElementTheme;
			applicationSettingsComposite[applicationSettingsNames.ElementSoundPlayerState] = (int)applicationSettingsAttachments.ElementSoundPlayerState;
			applicationSettingsComposite[applicationSettingsNames.ElementSpatialAudioMode] = (int)applicationSettingsAttachments.ElementSpatialAudioMode;

			localSettings.Containers[containerName].Values[CompositeValueNames.TheNameOfTheCompoundUserDataAttachment] = userDataAttachmentsComposite;
			localSettings.Containers[containerName].Values[CompositeValueNames.TheNameOfTheCompoundValueOfTheApplicationSettings] = applicationSettingsComposite;
		}

		/// <param name="containerName">
		/// "containerName" в данном случае именем контейнера выступает id пользователя
		/// </param>
		/// <returns>
		/// Все сохранённые данные пользователя
		/// </returns>
		public static SettingsAttachments GetSettingsFromUserContainer(string containerName)
		{
			UserDataAttachmentNames userDataAttachmentNames = CompositeValueNames.UserDataAttachmentNames;
			ApplicationSettingsAttachmentNames applicationSettingsNames = CompositeValueNames.ApplicationSettingsAttachmentNames;

			ApplicationDataCompositeValue userDataValue;
			ApplicationDataCompositeValue applicationSettingsValue;

			SettingsAttachments settingsAttachments = new SettingsAttachments();
			UserDataAttachments userDataAttachments = new UserDataAttachments();
			ApplicationSettingsAttachments applicationSettings = new ApplicationSettingsAttachments();

			if (!localSettings.Containers.ContainsKey(containerName))
			{
				throw new Exception("Контейнера с таким именем нет");
			}
			else
			{
				userDataValue = (ApplicationDataCompositeValue)localSettings.Containers[containerName].Values[CompositeValueNames.TheNameOfTheCompoundUserDataAttachment];
				applicationSettingsValue = (ApplicationDataCompositeValue)localSettings.Containers[containerName].Values[CompositeValueNames.TheNameOfTheCompoundValueOfTheApplicationSettings];

				if (userDataValue == null || applicationSettingsValue == null)
				{
					throw new Exception("В составном значении нет данных");
				}
				else
				{
					userDataAttachments.IsPasswordProtected = (bool)userDataValue[userDataAttachmentNames.IsPasswordProtected];
					userDataAttachments.UserID = (long)userDataValue[userDataAttachmentNames.UserID];
					userDataAttachments.UserName = (string)userDataValue[userDataAttachmentNames.UserName];
					userDataAttachments.Token = (string)userDataValue[userDataAttachmentNames.Token];
					userDataAttachments.AvatarURL = (string)userDataValue[userDataAttachmentNames.AvatarURL];
					userDataAttachments.ScreenName = (string)userDataValue[userDataAttachmentNames.ScreenName];

					applicationSettings.ElementTheme = (ElementTheme)applicationSettingsValue[applicationSettingsNames.ElementTheme];
					applicationSettings.ElementSoundPlayerState = (ElementSoundPlayerState)applicationSettingsValue[applicationSettingsNames.ElementSoundPlayerState];
					applicationSettings.ElementSpatialAudioMode = (ElementSpatialAudioMode)applicationSettingsValue[applicationSettingsNames.ElementSpatialAudioMode];

					settingsAttachments.UserDataAttachments = userDataAttachments;
					settingsAttachments.ApplicationSettings = applicationSettings;
					return settingsAttachments;
				}
			}
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
