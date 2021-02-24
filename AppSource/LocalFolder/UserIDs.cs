using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OxygenVK.AppSource.LocalFolder
{
	public class UserIDs
	{
		private readonly LocalFile LocalFile;
		private readonly XDocument XDocument;
		private string Users { get; } = "Users";
		private string User { get; } = "User";

		public UserIDs()
		{
			LocalFile = new LocalFile(Filenames.UserIDs);
			string fileContent = LocalFile.Read().Result;

			if(fileContent == null || fileContent == string.Empty)
			{
				XDocument = new XDocument(new XElement(Users));
			}
			else
			{
				try
				{
					XDocument = XDocument.Parse(fileContent);
				}
				catch(Exception)
				{
					throw;
				}
			}
		}

		public List<long> GetUserIDs()
		{
			List<long> list = new List<long>();
			foreach(XElement item in XDocument.Root.Elements(User))
			{
				list.Add(Convert.ToInt64(item.Value));
			}
			return list;
		}

		public void Add(long id)
		{
			XDocument.Root.Add(new XElement(User, id));
			Save();
		}

		public void Delete(long id)
		{
			foreach(XElement item in XDocument.Root.Elements(User))
			{
				if(item.Value == id.ToString())
				{
					item.Remove();
					break;
				}
			}
			Save();
		}

		private void Save() => LocalFile.Write(XDocument.ToString());
	}
}
