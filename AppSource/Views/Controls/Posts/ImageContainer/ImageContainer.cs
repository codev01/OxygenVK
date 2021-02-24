using System;
using System.Collections.Generic;
using System.Linq;

using OxygenVK.AppSource.Controls;

using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace OxygenVK.AppSource.Views.Controls.Posts.ImageContainer
{
	public partial class ImageContainer : Canvas
	{
		private readonly DoubleAnimation HeightDoubleAnimation;
		private readonly Storyboard Storyboard;

		public ImageContainer()
		{
			CacheMode = new BitmapCache();

			Storyboard = new Storyboard();
			HeightDoubleAnimation = new DoubleAnimation();
			HeightDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
			HeightDoubleAnimation.EnableDependentAnimation = true;
			Storyboard.SetTarget(HeightDoubleAnimation, this);
			Storyboard.SetTargetProperty(HeightDoubleAnimation, "Height");
			Storyboard.SetTargetName(HeightDoubleAnimation, Name);
			Storyboard.Children.Add(HeightDoubleAnimation);
		}

		public IReadOnlyList<ContainerElement> Attachments
		{
			get => (IReadOnlyList<ContainerElement>)GetValue(AttachmentsProperty);
			set => SetValue(AttachmentsProperty, value);
		}

		public static readonly DependencyProperty AttachmentsProperty = DependencyProperty.Register(nameof(Attachments), typeof(IReadOnlyList<ContainerElement>), typeof(ImageContainer), new PropertyMetadata(null, OnAttachmentsChanged));

		private static void OnAttachmentsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			ImageContainer presenter = (ImageContainer)obj;
			presenter.ProcessAttachments();
		}

		private void ProcessAttachments()
		{
			Children.Clear();

			if(Attachments == null)
			{
				return;
			}

			foreach(ContainerElement i in Attachments)
			{
				CustomButton pictureBoxControl = new CustomButton();
				//pictureBoxControl.Background = new SolidColorBrush(Colors.Gray);
				pictureBoxControl.Width = i.Width;
				pictureBoxControl.Height = i.Height;
				pictureBoxControl.BorderContent = new CustomImage("https://sun9-68.userapi.com/impg/c854016/v854016422/231b9e/BgOUaiRGnYk.jpg?size=1024x1024&quality=96&proxy=1&sign=dd9de0c45aa23b7b08de43c422a12207&type=album");
				Children.Add(pictureBoxControl);
			}

			UpdateSize();
		}

		public void UpdateSize()
		{
			if(Attachments != null)
			{
				List<Rect> layout = RectangleLayoutHelper.CreateLayout(new Size(ActualWidth, 500), Attachments.Select(t => new Size(t.Width, t.Height)).ToList(), 2);

				Rect rect;
				for(int i = 0; i < layout.Count; i++)
				{
					rect = layout[i];

					CustomButton pictureBoxControl = Children[i] as CustomButton;
					pictureBoxControl.Height = rect.Height;
					pictureBoxControl.Width = rect.Width;
					pictureBoxControl.Margin = new Thickness(rect.Left, rect.Top, 0, 0);
				}

				double height = 0;
				if(layout.Count > 0)
				{
					Rect rectContainer;
					rectContainer = layout.Last();

					height = rectContainer.Top + rectContainer.Height;
				}

				try
				{
					HeightDoubleAnimation.From = Height;
					HeightDoubleAnimation.To = height;
					Storyboard.Begin();
				}
				catch
				{
					Height = height;
				}
			}
		}
	}
}
