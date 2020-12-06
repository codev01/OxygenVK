namespace OxygenVK.AppSource.LocalSettings.Attachments
{
	public class UserSettingsAttachments
	{
		public static string UserName { get; } = "UserName";
		public static string UserID { get; } = "UserID";
		public static string Token { get; } = "Token";
		public static string AvatarURL { get; } = "AvatarURL";
		public static string ScreenName { get; } = "ScreenName";
	}

	public class UserSettingsAttachmentsValues
	{
		public string UserName { get; set; }
		public long UserID { get; set; }
		public string Token { get; set; }
		public string AvatarURL { get; set; }
		public string ScreenName { get; set; }
	}
}
