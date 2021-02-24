using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace OxygenVK.AppSource.Controls
{
	public sealed partial class CustomImage : UserControl
	{
		private BitmapImage BitmapImage { get; set; }

		public CustomImage() => InitializeComponent();
		public CustomImage(string sourceUri)
		{
			InitializeComponent();
			BitmapImage = new BitmapImage(new Uri(sourceUri));
			BitmapImage.DecodePixelHeight = (int)Height;
			BitmapImage.DecodePixelWidth = (int)Width;
			Img.Stretch = Stretch.UniformToFill;
		}

		private void CustomImage_Loaded(object sender, RoutedEventArgs e)
		{
			if(BitmapImage != null)
			{
				Img.Source = BitmapImage;
			}
		}
	}
}
