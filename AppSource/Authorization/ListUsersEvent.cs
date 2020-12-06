using System.Collections.Generic;

namespace OxygenVK.AppSource.Authorization
{
	public class ListUsersEvent
	{
		public delegate void ListStartUpdate();
		public delegate void ListUpdated(List<LocalSettings.Attachments.UserSettingsAttachmentsValues> userSettingsAttachmentsValues);
	}
}
