using System.Collections.Generic;
using System.Linq;

using OxygenVK.AppSource.Views.Controls.Posts.Attachments;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Views.Controls.Posts.ImageContainer
{
	public class CanvasContainer : Canvas
	{
		public CanvasContainer()
		{
			SizeChanged += CanvasContainer_SizeChanged;
		}

		private void CanvasContainer_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			UpdateMargin();
		}

		public IReadOnlyList<ImageContainerAttachment> Attachments
		{
			get => (IReadOnlyList<ImageContainerAttachment>)GetValue(AttachmentsProperty);
			set => SetValue(AttachmentsProperty, value);
		}

		public static readonly DependencyProperty AttachmentsProperty = DependencyProperty.Register(nameof(Attachments), typeof(IReadOnlyList<ImageContainerAttachment>), typeof(CanvasContainer), new PropertyMetadata(null, OnAttachmentsChanged));

		private static void OnAttachmentsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			CanvasContainer presenter = (CanvasContainer)obj;
			presenter.ProcessAttachments();
		}

		private void ProcessAttachments()
		{
			Children.Clear();

			if (Attachments == null)
			{
				return;
			}

			foreach (ImageContainerAttachment item in Attachments)
			{
				PictureBoxControl pictureBoxControl = new PictureBoxControl();

				Children.Add(pictureBoxControl);
			}

			UpdateMargin();
		}

		public void UpdateMargin()
		{
			if (Attachments == null)
			{
				return;
			}

			List<Rect> layout = RectangleLayoutHelper.CreateLayout(new Size(ActualWidth, ActualWidth), Attachments.Select(t => new Size(t.Width, t.Height)).ToList(), 2);

			for (int i = 0; i < layout.Count; i++)
			{
				Rect rect = layout[i];

				Thickness margin = new Thickness(rect.Left, rect.Top, 0.0, 0.0);

				PictureBoxControl pictureBoxControl = Children[i] as PictureBoxControl;
				pictureBoxControl.Height = rect.Height;
				pictureBoxControl.Width = rect.Width;
				pictureBoxControl.Margin = margin;
			}


			double num;
			if (layout.Count <= 0)
			{
				num = 0.0;
			}
			else
			{
				Rect rect = layout.Last();
				double top = rect.Top;
				rect = layout.Last();
				double height = rect.Height;
				num = top + height;
			}
			Height = num;
		}
	}
}
