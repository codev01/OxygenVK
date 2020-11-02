﻿using System.Collections.Generic;
using System.Linq;

using Windows.Foundation;

namespace OxygenVK.AppSource.Views.Controls
{
	public class RectangleLayoutHelper
	{
		public static List<Rect> CreateLayout(Size parentRect, List<Size> childrenRects, double marginBetween)
		{
			List<ThumbAttachment> thumbAttachments = ConvertSizesToThumbAttachments(childrenRects);
		
			ThumbnailLayoutManager.ProcessThumbnails(@parentRect.Width, @parentRect.Height, thumbAttachments, marginBetween);
			
			return ConvertProcessedThumbsToRects(thumbAttachments, marginBetween, @parentRect.Width);
		}

		private static List<Rect> ConvertProcessedThumbsToRects(List<ThumbAttachment> thumbs, double marginBetween, double width)
		{
			List<Rect> rectList = new List<Rect>(thumbs.Count);
			double num1 = 0.0;
			double widthOfRow = CalculateWidthOfRow(thumbs, marginBetween);
			double num2 = width / 2.0 - widthOfRow / 2.0;
			double num3 = num2;
			for (int index = 0; index < thumbs.Count; ++index)
			{
				ThumbAttachment thumb = thumbs[index];
				rectList.Add(new Rect(num3, num1, thumb.CalcWidth, thumb.CalcHeight));
				if (!thumb.LastColumn && !thumb.LastRow)
				{
					num3 += thumb.CalcWidth + marginBetween;
				}
				else if (thumb.LastRow)
				{
					num1 += thumb.CalcHeight + marginBetween;
				}
				else if (thumb.LastColumn)
				{
					num3 = num2;
					num1 += thumb.CalcHeight + marginBetween;
				}
			}
			return rectList;
		}

		private static double CalculateWidthOfRow(List<ThumbAttachment> thumbs, double marginBetween)
		{
			double num = 0.0;
			foreach (ThumbAttachment thumb in thumbs)
			{
				num += thumb.CalcWidth;
				num += marginBetween;
				if (!thumb.LastRow)
				{
					if (thumb.LastColumn)
					{
						break;
					}
				}
				else
				{
					break;
				}
			}
			if (num > 0.0)
			{
				num -= marginBetween;
			}

			return num;
		}

		private static List<ThumbAttachment> ConvertSizesToThumbAttachments(List<Size> childrenRects)
		{
			return Enumerable.Select(childrenRects, r => new ThumbAttachment()
			{
				Height = @r.Height > 0.0 ? @r.Height : 100.0,
				Width = @r.Width > 0.0 ? @r.Width : 100.0
			}).ToList();
		}
	}
}