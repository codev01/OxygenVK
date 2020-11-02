using System.Collections.Generic;
using System.Linq;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OxygenVK.AppSource.Views.Controls
{
	public class ThumbsItem : Canvas
	{
		public IReadOnlyList<ThumbAttachment> Attachments
		{
			get => (IReadOnlyList<ThumbAttachment>)GetValue(AttachmentsProperty);
			set => SetValue(AttachmentsProperty, value);
		}

		public static readonly DependencyProperty AttachmentsProperty = DependencyProperty.Register(nameof(Attachments), typeof(IReadOnlyList<ThumbAttachment>), typeof(ThumbsItem), new PropertyMetadata(null, ThumbsItem.OnAttachmentsChanged));

		private static void OnAttachmentsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			ThumbsItem presenter = (ThumbsItem)obj;
			presenter.ProcessAttachments();
		}

		private void ProcessAttachments()
		{
			Children.Clear();

			if (Attachments == null)
			{
				return;
			}

			foreach (ThumbAttachment item in Attachments)
			{
				Button brd = new Button
				{
					Background = new SolidColorBrush(Windows.UI.Colors.Gray),
					CornerRadius = new CornerRadius(4)
				};

				Children.Add(brd);
			}

			UpdateMargin();
		}

		public void UpdateMargin()
		{
			if (Attachments == null)
			{
				return;
			}

			List<Rect> layout = RectangleLayoutHelper.CreateLayout(new Size(ActualWidth, 320), Attachments.Select(t => new Size(t.Width, t.Height)).ToList(), 4.0);

			for (int i = 0; i < layout.Count; i++)
			{
				Rect rect = layout[i];

				Thickness margin = new Thickness(rect.Left, rect.Top, 0.0, 0.0);

				Button brd = Children[i] as Button;
				brd.Height = rect.Height;
				brd.Width = rect.Width;
				brd.Margin = margin;
			}

			double num;
			if (layout.Count <= 0)
			{
				num = 0.0;
			}
			else
			{
				Rect rect = layout.Last<Rect>();
				double top = rect.Top;
				rect = layout.Last<Rect>();
				double height = rect.Height;
				num = top + height;
			}
			base.Height = num;
		}
	}
}
