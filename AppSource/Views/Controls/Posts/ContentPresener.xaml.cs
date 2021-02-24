using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Views.Controls.Posts
{
	public sealed partial class ContentPresener : UserControl
	{
		public ContentPresener()
		{
			InitializeComponent();
		}

		private void ContentPresener_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
		{
			ImageContainer.UpdateSize();
		}
	}
}
