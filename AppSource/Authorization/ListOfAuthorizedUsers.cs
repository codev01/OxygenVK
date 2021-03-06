﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;

using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;

using static OxygenVK.AppSource.Authorization.ListUsersEvent;

namespace OxygenVK.Authorization
{
	public class ListOfAuthorizedUsers
	{
		public static event ListStartUpdate OnListStartUpdate;
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
		}

		public void InitializeList()
		{
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
						//isoStore.DeleteFile(FileName);
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

		public async void SetListOfAuthorizedUsersAsync(VkApi vkApi)
		{
			OnListStartUpdate.Invoke();

			long userID = 0;
			foreach (User item in await vkApi.Users.GetAsync(new long[0]))
			{
				userID = item.Id;
			}
			DeleteUserData(userID);

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

			xDoc.Root.Add(new XElement(new XElement(User, new XAttribute(AttributeName, profileInfo.FirstName + " " + profileInfo.LastName),
						  new XElement(AvatarURL, photoURL),
						  new XElement(Token, vkApi.Token),
						  new XElement(UserID, userID),
						  new XElement(ScreenName, profileInfo.ScreenName))));

			xml = null;
			xml = xDoc.ToString();
			FileStream(true);
			GetUserData();
		}

		private void GetUserData()
		{
			listOfAuthorizedUsers.Clear();
			foreach (XElement item in xDoc.Root.Elements(User))
			{
				listOfAuthorizedUsers.Add(new AuthorizedUserCardsAttachment
				{
					UserName = item.Attribute(AttributeName).Value,
					AvatarUrl = item.Element(AvatarURL).Value,
					ScreenName = item.Element(ScreenName).Value,
					Token = item.Element(Token).Value,
					UserID = Convert.ToInt64(item.Element(UserID).Value)
				});
			}
			OnListUpdated.Invoke(listOfAuthorizedUsers);
		}

		public void DeleteUserData(long userID)
		{
			foreach (XElement i in xDoc.Root.Elements(User))
			{
				if (i.Element(UserID).Value == userID.ToString())
				{
					i.Remove();
				}
			}

			List<AuthorizedUserCardsAttachment> items = new List<AuthorizedUserCardsAttachment>();
			foreach (AuthorizedUserCardsAttachment item in listOfAuthorizedUsers)
			{
				if (item.UserID == userID)
				{
					items.Add(item);
				}
			}
			foreach (AuthorizedUserCardsAttachment item in items)
			{
				listOfAuthorizedUsers.Remove(item);
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
	}
}
