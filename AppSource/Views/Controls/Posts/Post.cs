using System.Collections.Generic;

using OxygenVK.AppSource.Views.Controls.Posts.ImageContainer;

namespace OxygenVK.AppSource.Views.Controls.Posts
{
	public class Post
	{
		public string OwnerName { get; set; }
		public List<ContainerElement> Attachments { get; set; }
	}
}
