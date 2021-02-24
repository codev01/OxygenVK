using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace OxygenVK.AppSource.Controls
{
	public sealed partial class CustomButton : UserControl
	{
		public object BorderContent
		{
			get;
			set;
		}

		public CustomButton() => InitializeComponent();
  
		private void RootBorder_PointerPressed(object sender, PointerRoutedEventArgs e) => VisualStateManager.GoToState(this, "Pressed", true);

		private void RootBorder_PointerEntered(object sender, PointerRoutedEventArgs e) => VisualStateManager.GoToState(this, "PointerOver", true);

		private void RootBorder_PointerExited(object sender, PointerRoutedEventArgs e) => VisualStateManager.GoToState(this, "Normal", true);

		private void RootBorder_PointerReleased(object sender, PointerRoutedEventArgs e) => VisualStateManager.GoToState(this, "Normal", true);
	}
}
