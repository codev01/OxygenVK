using System;
using System.Collections.Generic;

using OxygenVK.AppSource.Views.Controls.Posts.ImageContainer;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace OxygenVK.AppSource.Views.Controls.Posts
{
	public sealed partial class ListPostsControl : UserControl
	{
		public ListPostsControl() => InitializeComponent();

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			for(int i = 0; i < 1000; i++)
			{
				Post post = new Post();
				List<ContainerElement> containerElements = new List<ContainerElement>();

				for(int j = 0; j < 10; j++)
				{
					containerElements.Add(new ContainerElement
					{
						Height = 100,
						Width = 160
					});
				}

				post.OwnerName = "Rashid";
				post.Attachments = containerElements;

				lv.Items.Add(post);
			}
		}

		private void RootGridHead_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
		{
			AnimationSize.To = e.NewSize.Height + 30;
			StoryboardAnimation.Begin();
		}
	}
}
