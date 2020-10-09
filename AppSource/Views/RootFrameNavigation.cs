using VkNet;

namespace OxygenVK.AppSource.Views
{
	internal class RootFrameNavigation
	{
		public delegate void FrameNavigation(VkApi vkApi);
		public static event FrameNavigation OnFrameNavigation;

		public RootFrameNavigation(VkApi vkApi)
		{
			OnFrameNavigation.Invoke(vkApi);
		}
	}
}
