using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;

using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;

namespace OxygenVK.Authorization
{
	public class ListOfAuthorizedUsers
	{
		public delegate void ListUpdated();
		public static event ListUpdated OnListUpdated;

		public static List<AuthorizedUserCardsAttachment> listOfAuthorizedUsers = new List<AuthorizedUserCardsAttachment>();

		private string FileName { get; } = "Users.xml";
		private string User { get; } = "User";
		private string AttributeName { get; } = "Name";
		private string UserID { get; } = "UserID";
		private string Token { get; } = "Token";
		private string AvatarURL { get; } = "AvatarURL";
		private string ScreenName { get; } = "ScreenName";

		private string xml;
		private XDocument xDoc;

		public ListOfAuthorizedUsers()
		{
			FileStream();
			GetUserData();
		}

		private void FileStream(bool isSet = false, bool deleteFile = false)
		{
			IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain, null, null);
			if (isSet && deleteFile)
			{
				isoStore.DeleteFile(FileName);
			}
			else
			{
				if (isSet)
				{
					using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(FileName, FileMode.Create, isoStore))
					{
						using (StreamWriter writer = new StreamWriter(isoStream))
						{
							writer.Write(xml);
						}
					}
				}
				else
				{
					try
					{
						if (isoStore.FileExists(FileName))
						{
							using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(FileName, FileMode.Open, isoStore))
							{
								using (StreamReader reader = new StreamReader(isoStream))
								{
									xml = reader.ReadToEnd();
									if (xml == "")
									{
										SetEmtyFile();
									}
								}
							}
						}
						else
						{
							using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(FileName, FileMode.CreateNew, isoStore))
							{
								SetEmtyFile();
							}
						}
						xDoc = XDocument.Parse(xml);
					}
					catch
					{
						isoStore.DeleteFile(FileName);
					}
				}
			}
		}

		private void SetEmtyFile()
		{
			xml = "<?xml version=\"1.0\" encoding=\"utf - 8\"?>\n" +
								   "<Users>" +
								   "</Users>";
		}

		public void SetListOfAuthorizedUsers(VkApi vkApi)
		{
			string photoURL = null;
			foreach (VkNet.Model.Attachments.Photo photo in vkApi.Photo.Get(new VkNet.Model.RequestParams.PhotoGetParams
			{
				AlbumId = PhotoAlbumType.Profile,
				Count = 1
			}))
			{
				photoURL = photo.Sizes.Last().Url.AbsoluteUri;
			}
			long userID = 0;
			foreach (User item in vkApi.Users.Get(new long[0]))
			{
				userID = item.Id;
			}
			VkNet.Model.RequestParams.AccountSaveProfileInfoParams profileInfo = vkApi.Account.GetProfileInfo();

			DeleteUserData(userID);

			xDoc.Root.Add(new XElement(new XElement(User, new XAttribute(AttributeName, profileInfo.FirstName + " " + profileInfo.LastName),
						  new XElement(AvatarURL, photoURL),
						  new XElement(Token, vkApi.Token),
						  new XElement(UserID, userID),
						  new XElement(ScreenName, profileInfo.ScreenName))));

			xml = null;
			xml = xDoc.ToString();
			FileStream(true);

			listOfAuthorizedUsers.Clear();
			GetUserData();
			OnListUpdated.Invoke();
		}
		private void GetUserData()
		{
			foreach (XElement item in xDoc.Root.Elements(User))
			{
				listOfAuthorizedUsers.Add(new AuthorizedUserCardsAttachment
				{
					UserName = item.Attribute(AttributeName).Value,
					AvatarUrl = item.Element(AvatarURL).Value,
					ScreenName = item.Element(ScreenName).Value,
					UserID = Convert.ToInt64(item.Element(UserID).Value)
				});
			}
		}
		public void DeleteUserData(long userID)
		{
			try
			{
				foreach (XElement i in xDoc.Root.Elements(User))
				{
					if (i.Element(UserID).Value == userID.ToString())
					{
						i.Remove();
					}
				}
				if (xDoc.ToString() == "<Users />")
				{
					FileStream(true, true);
				}
				else
				{
					xml = null;
					xml = xDoc.ToString();
					FileStream(true);
				}
			}
			catch { }
		}
	}
}
