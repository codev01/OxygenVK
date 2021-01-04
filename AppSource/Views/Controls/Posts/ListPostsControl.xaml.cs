using System.Collections.Generic;

using OxygenVK.AppSource.Views.Controls.Posts.Attachments;

using Windows.UI.Xaml.Controls;

namespace VK.Controls
{
	public sealed partial class ListPostsControl : UserControl
	{
		public ListPostsControl()
		{
			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			for (int i = 0; i < 10; i++)
			{
				PostsAttachments postsAttachments = new PostsAttachments();
				List<ImageContainerAttachment> containerAttachments = new List<ImageContainerAttachment>();

				for (int j = 0; j < i + 2; j++)
				{
					containerAttachments.Add(new ImageContainerAttachment { Height = 100, Width = 160 });
				}

				postsAttachments.Name = "Rashid";
				postsAttachments.Attachments = containerAttachments;
				lv.Items.Add(postsAttachments);
			}
		}
	}
}
