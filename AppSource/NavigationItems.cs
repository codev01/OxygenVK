using System.Collections.Generic;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OxygenVK.AppSource
{
	public class NavigationItems
	{
		public short Index { get; set; }
		public string Content { get; set; }
		public string Tag { get; set; }
		public IconElement Icon { get; set; }
	}

	public class NavigationItemsList
	{
		public IList<NavigationItems> NavigationItems { get; } = new List<NavigationItems>();

		public NavigationItemsList()
		{
			DefaultOrder();
		}

		private void AddItem(short index, string content, string tag, string iconGlyph)
		{
			NavigationItems.Add(new NavigationItems
			{
				Index = index,
				Content = content,
				Tag = tag,
				Icon = GetIcon(iconGlyph)
			});
		}

		private IconElement GetIcon(string imagePath)
		{
			return new FontIcon()
			{
				FontFamily = new FontFamily("Segoe MDL2 Assets"),
				Glyph = imagePath,
			};
		}

		private void DefaultOrder()
		{
			AddItem(0, "Новости", "news", "\xE737");
			AddItem(1, "Мессенджер", "messenger", "\xE15F");
			AddItem(2, "Друзья", "friends", "\xE125");
			AddItem(3, "Сообщества", "communities", "\xE902");
			AddItem(4, "Фотографии", "photos", "\xEB9F");
			AddItem(5, "Музыка", "music", "\xE189");
			AddItem(6, "Видео", "videos", "\xE8B2");
			AddItem(7, "Клипы", "clips", "\xE12A");
		}
	}
}
