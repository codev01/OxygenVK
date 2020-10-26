using System.Collections.Generic;
using OxygenVK.Authorization;

namespace OxygenVK.AppSource.Authorization
{
	public class ListUsersEvent
	{
		public delegate void ListStartUpdate();
		public delegate void ListUpdated(List<AuthorizedUserCardsAttachment> authorizedUserCardsAttachments);
	}
}
