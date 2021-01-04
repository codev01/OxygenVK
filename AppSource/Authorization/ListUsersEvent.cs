using System.Collections.Generic;

using OxygenVK.AppSource.LocalSettings.Attachments;

namespace OxygenVK.AppSource.Authorization
{
	public class ListUsersEvent
	{
		public delegate void ListStartUpdate();
		public delegate void ListUpdated(List<SettingsAttachments> userSettingsAttachmentsValues);
	}
}
