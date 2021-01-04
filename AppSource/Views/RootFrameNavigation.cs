using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace OxygenVK.AppSource.Views
{
	public class RootFrameNavigation
	{
		/// <summary>
		/// Использует указынный фрейм для навигации
		/// </summary>
		/// <param name="frame">
		/// Фрейм, в котором должна произойти навигация
		/// </param>
		/// <param name="type"> 
		/// Страница, на которою должен произойти переход
		/// </param>
		/// <param name="parameter">
		/// Параметры, которые должны передаться указанной странице
		/// </param>
		public RootFrameNavigation(Frame frame, Type type, object parameter)
		{
			frame.Navigate(type, parameter, new SlideNavigationTransitionInfo()
			{ 
				Effect = SlideNavigationTransitionEffect.FromRight
			});
		}
	}
}
