using OxygenVK.AppSource.LocaSettings.Attachments;

using System;


using Windows.Storage;
using Windows.UI.Xaml;

namespace OxygenVK.AppSource.LocaSettings
{
	public class LocalSettings
	{
		private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
		private static readonly CompositeValueNames compositeValueNames = new CompositeValueNames();

		public enum SettingsCategory
		{
			UserData,
			ApplicationSettings
		}

		/// <summary>
		/// Добавляет новый контейнер в настройки
		/// </summary>
		/// <param name="containerName">
		/// в данном случае именем контейнера выступает id пользователя
		/// </param>
		/// <param name="settingsAttachments">
		/// Вложения для сохранения
		/// </param>
		public static void AddUserSettingsContainer(long containerName, Settings settingsAttachments)
		{
			string strContainerName = containerName.ToString();

			UserData uda = settingsAttachments.UserDataAttachments;
			ApplicationSettings asa = settingsAttachments.ApplicationSettings;

			localSettings.CreateContainer(strContainerName, ApplicationDataCreateDisposition.Always);

			ApplicationDataCompositeValue settingsComposite = new ApplicationDataCompositeValue();

			SetSettingCell(UserData.UserDataIndex.IsPasswordProtected, containerName, uda.IsPasswordProtected, SettingsCategory.UserData, settingsComposite);
			SetSettingCell(UserData.UserDataIndex.UserID, containerName, uda.UserID, SettingsCategory.UserData, settingsComposite);
			SetSettingCell(UserData.UserDataIndex.UserName, containerName, uda.UserName, SettingsCategory.UserData, settingsComposite);
			SetSettingCell(UserData.UserDataIndex.Token, containerName, uda.Token, SettingsCategory.UserData, settingsComposite);
			SetSettingCell(UserData.UserDataIndex.AvatarURL, containerName, uda.AvatarURL, SettingsCategory.UserData, settingsComposite);
			SetSettingCell(UserData.UserDataIndex.ScreenName, containerName, uda.ScreenName, SettingsCategory.UserData, settingsComposite);

			SetSettingCell(ApplicationSettings.ApplicationSettingsIndex.ElementTheme, containerName, asa.ElementTheme, SettingsCategory.ApplicationSettings, settingsComposite);
			SetSettingCell(ApplicationSettings.ApplicationSettingsIndex.ElementSoundPlayerState, containerName, asa.ElementSoundPlayerState, SettingsCategory.ApplicationSettings, settingsComposite);
			SetSettingCell(ApplicationSettings.ApplicationSettingsIndex.ElementSpatialAudioMode, containerName, asa.ElementSpatialAudioMode, SettingsCategory.ApplicationSettings, settingsComposite);
		}

		/// <param name="containerName">
		/// в данном случае именем контейнера выступает id пользователя
		/// </param>
		/// <returns>
		/// Все сохранённые данные пользователя
		/// </returns>
		public static Settings GetUserSettingsContainer(long containerName)
		{
			string strContainerName = containerName.ToString();

			ApplicationDataCompositeValue userDataValue;
			ApplicationDataCompositeValue applicationSettingsValue;

			if(localSettings.Containers.ContainsKey(strContainerName))
			{
				userDataValue = (ApplicationDataCompositeValue)localSettings.Containers[strContainerName].Values[compositeValueNames.TheNameOfTheCompoundUserDataAttachment];
				applicationSettingsValue = (ApplicationDataCompositeValue)localSettings.Containers[strContainerName].Values[compositeValueNames.TheNameOfTheCompoundValueOfTheApplicationSettings];

				if(userDataValue != null && applicationSettingsValue != null)
				{
					UserData userDataAttachments = new UserData();
					ApplicationSettings applicationSettingsAttachments = new ApplicationSettings();
					userDataAttachments.IsPasswordProtected = GetSettingCell<bool>(UserData.UserDataIndex.IsPasswordProtected, userDataValue);
					userDataAttachments.UserID = GetSettingCell<long>(UserData.UserDataIndex.UserID, userDataValue);
					userDataAttachments.UserName = GetSettingCell<string>(UserData.UserDataIndex.UserName, userDataValue);
					userDataAttachments.Token = GetSettingCell<string>(UserData.UserDataIndex.Token, userDataValue);
					userDataAttachments.AvatarURL = GetSettingCell<string>(UserData.UserDataIndex.AvatarURL, userDataValue);
					userDataAttachments.ScreenName = GetSettingCell<string>(UserData.UserDataIndex.ScreenName, userDataValue);

					applicationSettingsAttachments.ApplicationDataCompositeValue = applicationSettingsValue;
					applicationSettingsAttachments.ElementTheme = GetSettingCell<ElementTheme>(ApplicationSettings.ApplicationSettingsIndex.ElementTheme, applicationSettingsValue);
					applicationSettingsAttachments.ElementSoundPlayerState = GetSettingCell<ElementSoundPlayerState>(ApplicationSettings.ApplicationSettingsIndex.ElementSoundPlayerState, applicationSettingsValue);
					applicationSettingsAttachments.ElementSpatialAudioMode = GetSettingCell<ElementSpatialAudioMode>(ApplicationSettings.ApplicationSettingsIndex.ElementSpatialAudioMode, applicationSettingsValue);

					return new Settings
					{
						UserDataAttachments = userDataAttachments,
						ApplicationSettings = applicationSettingsAttachments
					};
				}
				else
				{
					throw new Exception("В составном значении нет данных");
				}
			}
			else
			{
				throw new Exception("Контейнера с таким именем нет");
			}
		}

		/// <summary>
		/// Изменяет настройки точечно
		/// </summary>
		/// <param name="cellName">Имя ячейки. Рекомендуется использовать тип перечисления (enum)</param>
		/// <param name="containerName">в данном случае именем контейнера выступает id пользователя</param>
		/// <param name="objSetting">
		/// Объект, который нужно сохранить.
		/// В качестве сохраняемых параметров приложения можно использовать только следующие типы данных:
		/// UInt8 , Int16 , UInt16 , Int32 , UInt32 , Int64 , UInt64 , Single , Double, Bool, Char16 , String, DateTime, TimeSpan, Point, Size, Rect, ApplicationDataCompositeValue. Набор связанных параметров приложения, которые необходимо сериализовать и десериализовать.
		/// </param>
		/// <param name="settingsCategory">Категория параметра | (какой категории пренадлежит сохраняемый параметр) | перечисление LocalSettings.SettingsCategory</param>
		/// <param name="settingsComposite">Составное значение параметра (если изменяемых/сохраняемых параметров несколько нужно использовать только один объект ApplicationDataCompositeValue на всех)</param>
		public static void SetSettingCell(object cellName, long containerName, object objSetting, SettingsCategory settingsCategory, ApplicationDataCompositeValue settingsComposite)
		{
			settingsComposite[cellName.ToString()] = objSetting == null ? string.Empty : ComplianceCheck(objSetting) ? objSetting : (int)objSetting;
			localSettings.Containers[containerName.ToString()].Values[GetSettingsCategory(settingsCategory)] = settingsComposite;
		}

		private static T GetSettingCell<T>(object cellName, ApplicationDataCompositeValue settingsComposite) => (T)settingsComposite[cellName.ToString()];

		/// <summary>
		/// Удалят контейнер с сохранёнными данными пользователя
		/// </summary>
		/// <param name="containerName">
		/// "containerName" в данном случае именем контейнера выступает id пользователя
		/// </param>
		public static void DeleteUserContainerSettings(long containerName)
		{
			string strContainerName = containerName.ToString();
			localSettings.DeleteContainer(strContainerName);
		}

		private static string GetSettingsCategory(SettingsCategory settingsCategory)
		{
			switch(settingsCategory)
			{
				case SettingsCategory.UserData:
					return compositeValueNames.TheNameOfTheCompoundUserDataAttachment;
				case SettingsCategory.ApplicationSettings:
					return compositeValueNames.TheNameOfTheCompoundValueOfTheApplicationSettings;
			}
			return null;
		}

		private static bool ComplianceCheck(object val)
		{
			bool isMeetsTheRequirements = true;
			switch(val)
			{
				case sbyte:
					break;
				case short:
					break;
				case ushort:
					break;
				case int:
					break;
				case uint:
					break;
				case long:
					break;
				case ulong:
					break;
				case float:
					break;
				case double:
					break;
				case bool:
					break;
				case char:
					break;
				case string:
					break;
				case DateTime:
					break;
				case TimeSpan:
					break;
				case System.Drawing.Point:
					break;
				case Windows.Foundation.Rect:
					break;
				case ApplicationDataCompositeValue:
					break;
				case null:
					throw new NullReferenceException("LocalSettings.GetType() return null");

				default:
					isMeetsTheRequirements = false;
					break;
			}
			return isMeetsTheRequirements;
		}
	}
}
