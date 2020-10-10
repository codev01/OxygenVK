using System;

using VkNet;

using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Views
{
	internal class RootFrameNavigation
	{
		public RootFrameNavigation(Frame frame, Type type, object parameter)
		{
			frame.Navigate(type, parameter);
		}
	}
}
