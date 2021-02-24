using System.Collections.Generic;
using System.Linq;

using Windows.Foundation;

namespace OxygenVK.AppSource.Views.Controls.Posts.ImageContainer
{
	public class RectangleLayoutHelper
	{
		public static List<Rect> CreateLayout(Size parentRect, List<Size> childrenRects, double marginBetween)
		{
			List<ContainerElement> thumbAttachments = ConvertSizesToThumbAttachments(childrenRects);

			LayoutManager.ProcessThumbnails(parentRect.Width, parentRect.Height, thumbAttachments, marginBetween);

			return ConvertProcessedThumbsToRects(thumbAttachments, marginBetween, parentRect.Width);
		}

		private static List<Rect> ConvertProcessedThumbsToRects(List<ContainerElement> thumbs, double marginBetween, double width)
		{
			List<Rect> rectList = new List<Rect>(thumbs.Count);
			double widthOfRow = CalculateWidthOfRow(thumbs, marginBetween);

			double num1 = 0;
			double num2 = width / 2 - widthOfRow / 2;
			double num3 = num2;
			for(int i = 0; i < thumbs.Count; ++i)
			{
				ContainerElement thumb = thumbs[i];
				rectList.Add(new Rect(num3, num1, thumb.CalcWidth, thumb.CalcHeight));
				if(!thumb.LastColumn && !thumb.LastRow)
				{
					num3 += thumb.CalcWidth + marginBetween;
				}
				else if(thumb.LastRow)
				{
					num1 += thumb.CalcHeight + marginBetween;
				}
				else if(thumb.LastColumn)
				{
					num3 = num2;
					num1 += thumb.CalcHeight + marginBetween;
				}
			}
			return rectList;
		}

		private static double CalculateWidthOfRow(List<ContainerElement> thumbs, double marginBetween)
		{
			double num = 0;
			foreach(ContainerElement thumb in thumbs)
			{
				num += thumb.CalcWidth;
				num += marginBetween;
				if(!thumb.LastRow)
				{
					if(thumb.LastColumn)
					{
						break;
					}
				}
				else
				{
					break;
				}
			}
			if(num > 0)
			{
				num -= marginBetween;
			}
			return num;
		}

		private static List<ContainerElement> ConvertSizesToThumbAttachments(List<Size> childrenRects) => 
			Enumerable.Select(childrenRects, r => new ContainerElement()
			{
				Height = r.Height > 0 ? r.Height : 100,
				Width = r.Width > 0 ? r.Width : 100
			}).ToList();
	}
}
