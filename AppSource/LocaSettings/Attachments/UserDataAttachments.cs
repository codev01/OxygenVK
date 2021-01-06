namespace OxygenVK.AppSource.LocalSettings.Attachments
{
	public class UserDataAttachments
	{
		public bool IsPasswordProtected { get; set; } = false;
		public string UserName { get; set; }
		public long UserID { get; set; }
		public string Token { get; set; }
		public string AvatarURL { get; set; }
		public string ScreenName { get; set; }
	}
}
