using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Views.Controls
{
	public sealed partial class ItemPostControl : UserControl
	{
		public ItemPostControl()
		{
			this.InitializeComponent();
		}

		private void StackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this._thumbItem.UpdateMargin();
		}
	}
}
