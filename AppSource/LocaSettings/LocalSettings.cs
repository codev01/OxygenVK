using System;

using OxygenVK.AppSource.LocalSettings.Attachments;

using Windows.Storage;
using Windows.UI.Xaml;

namespace OxygenVK.AppSource.LocalSettings
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
		public static void AddUserSettingsContainer(long containerName, SettingsAttachments settingsAttachments)
		{
			string strContainerName = containerName.ToString();

			UserDataAttachments uda = settingsAttachments.UserDataAttachments;
			ApplicationSettingsAttachments asa = settingsAttachments.ApplicationSettings;
			
			localSettings.CreateContainer(strContainerName, ApplicationDataCreateDisposition.Always);

			ApplicationDataCompositeValue settingsComposite = new ApplicationDataCompositeValue();

			SetSettingCell(UserDataAttachments.UserDataIndex.IsPasswordProtected, containerName, uda.IsPasswordProtected, SettingsCategory.UserData, settingsComposite);
			SetSettingCell(UserDataAttachments.UserDataIndex.UserID, containerName, uda.UserID, SettingsCategory.UserData, settingsComposite);
			SetSettingCell(UserDataAttachments.UserDataIndex.UserName, containerName, uda.UserName, SettingsCategory.UserData, settingsComposite);
			SetSettingCell(UserDataAttachments.UserDataIndex.Token, containerName, uda.Token, SettingsCategory.UserData, settingsComposite);
			SetSettingCell(UserDataAttachments.UserDataIndex.AvatarURL, containerName, uda.AvatarURL, SettingsCategory.UserData, settingsComposite);
			SetSettingCell(UserDataAttachments.UserDataIndex.ScreenName, containerName, uda.ScreenName, SettingsCategory.UserData, settingsComposite);

			SetSettingCell(ApplicationSettingsAttachments.ApplicationSettingsIndex.ElementTheme, containerName, asa.ElementTheme, SettingsCategory.ApplicationSettings, settingsComposite);
			SetSettingCell(ApplicationSettingsAttachments.ApplicationSettingsIndex.ElementSoundPlayerState, containerName, asa.ElementSoundPlayerState, SettingsCategory.ApplicationSettings, settingsComposite);
			SetSettingCell(ApplicationSettingsAttachments.ApplicationSettingsIndex.ElementSpatialAudioMode, containerName, asa.ElementSpatialAudioMode, SettingsCategory.ApplicationSettings, settingsComposite);
		}

		/// <param name="containerName">
		/// в данном случае именем контейнера выступает id пользователя
		/// </param>
		/// <returns>
		/// Все сохранённые данные пользователя
		/// </returns>
		public static SettingsAttachments GetUserSettingsContainer(long containerName)
		{
			string strContainerName = containerName.ToString();
			
			ApplicationDataCompositeValue userDataValue;
			ApplicationDataCompositeValue applicationSettingsValue;

			if (localSettings.Containers.ContainsKey(strContainerName))
			{
				userDataValue = (ApplicationDataCompositeValue)localSettings.Containers[strContainerName].Values[compositeValueNames.TheNameOfTheCompoundUserDataAttachment];
				applicationSettingsValue = (ApplicationDataCompositeValue)localSettings.Containers[strContainerName].Values[compositeValueNames.TheNameOfTheCompoundValueOfTheApplicationSettings];

				if (userDataValue != null && applicationSettingsValue != null)
				{
					UserDataAttachments userDataAttachments = new UserDataAttachments();
					ApplicationSettingsAttachments applicationSettingsAttachments = new ApplicationSettingsAttachments();
					userDataAttachments.IsPasswordProtected = GetSettingCell<bool>(UserDataAttachments.UserDataIndex.IsPasswordProtected, userDataValue);
					userDataAttachments.UserID = GetSettingCell<long>(UserDataAttachments.UserDataIndex.UserID, userDataValue);
					userDataAttachments.UserName = GetSettingCell<string>(UserDataAttachments.UserDataIndex.UserName, userDataValue);
					userDataAttachments.Token = GetSettingCell<string>(UserDataAttachments.UserDataIndex.Token, userDataValue);
					userDataAttachments.AvatarURL = GetSettingCell<string>(UserDataAttachments.UserDataIndex.AvatarURL, userDataValue);
					userDataAttachments.ScreenName = GetSettingCell<string>(UserDataAttachments.UserDataIndex.ScreenName, userDataValue);

					applicationSettingsAttachments.ApplicationDataCompositeValue = applicationSettingsValue;
					applicationSettingsAttachments.ElementTheme = GetSettingCell<ElementTheme>(ApplicationSettingsAttachments.ApplicationSettingsIndex.ElementTheme, applicationSettingsValue);
					applicationSettingsAttachments.ElementSoundPlayerState = GetSettingCell<ElementSoundPlayerState>(ApplicationSettingsAttachments.ApplicationSettingsIndex.ElementSoundPlayerState, applicationSettingsValue);
					applicationSettingsAttachments.ElementSpatialAudioMode = GetSettingCell<ElementSpatialAudioMode>(ApplicationSettingsAttachments.ApplicationSettingsIndex.ElementSpatialAudioMode, applicationSettingsValue);
				
					return new SettingsAttachments
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
			settingsComposite[cellName.ToString()] = objSetting == null ? "" : ComplianceCheck(objSetting) ? objSetting : (int)objSetting;
			localSettings.Containers[containerName.ToString()].Values[GetSettingsCategory(settingsCategory)] = settingsComposite;
		}

		private static T GetSettingCell<T>(object cellName, ApplicationDataCompositeValue settingsComposite)
		{
			return (T)settingsComposite[cellName.ToString()];
		}

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
			switch (settingsCategory)
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
			bool isMeetsTheRequirements = false;
			switch (val)
			{
				case sbyte:
					isMeetsTheRequirements = true;
					break;
				case short:
					isMeetsTheRequirements = true;
					break;
				case ushort:
					isMeetsTheRequirements = true;
					break;
				case int:
					isMeetsTheRequirements = true;
					break;
				case uint:
					isMeetsTheRequirements = true;
					break;
				case long:
					isMeetsTheRequirements = true;
					break;
				case ulong:
					isMeetsTheRequirements = true;
					break;
				case float:
					isMeetsTheRequirements = true;
					break;
				case double:
					isMeetsTheRequirements = true;
					break;
				case bool:
					isMeetsTheRequirements = true;
					break;
				case char:
					isMeetsTheRequirements = true;
					break;
				case string:
					isMeetsTheRequirements = true;
					break;
				case DateTime:
					isMeetsTheRequirements = true;
					break;
				case TimeSpan:
					isMeetsTheRequirements = true;
					break;
				case System.Drawing.Point:
					isMeetsTheRequirements = true;
					break;
				case Windows.Foundation.Rect:
					isMeetsTheRequirements = true;
					break;
				case ApplicationDataCompositeValue:
					isMeetsTheRequirements = true;
					break;
				case null:
					//throw new NullReferenceException("LocalSettings.GetType() return null");
					break;
			}
			return isMeetsTheRequirements;
		}
	}
}
