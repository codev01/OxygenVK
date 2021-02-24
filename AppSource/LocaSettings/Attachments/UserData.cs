namespace OxygenVK.AppSource.LocaSettings.Attachments
{
	public class UserData
	{
		public enum UserDataIndex
		{
			IsPasswordProtected,
			UserID,
			Token,
			UserName,
			AvatarURL,
			ScreenName
		}

		public bool IsPasswordProtected { get; set; } = false;
		public long UserID { get; set; }
		public string Token { get; set; }
		public string UserName { get; set; }
		public string AvatarURL { get; set; }
		public string ScreenName { get; set; }
	}
}
