using VkNet;
using VkNet.Model;

namespace OxygenVK.Authorization
{
	public class Authorization
	{
		public delegate void AuthorizationCompleted(VkApi vkApi);
		public static event AuthorizationCompleted OnAuthorizationComleted;

		public Authorization(string token)
		{
			VkApi vkApi = new VkApi();

			vkApi.AuthorizeAsync(new ApiAuthParams
			{
				AccessToken = token
			});
			OnAuthorizationComleted?.Invoke(vkApi);
		}
	}
}
