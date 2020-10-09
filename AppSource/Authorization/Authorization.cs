using VkNet;
using VkNet.Model;

namespace OxygenVK.Authorization
{
	public class Authorization
	{
		public delegate void AuthorizationCompleted(VkApi vkApi);
		public static event AuthorizationCompleted OnAuthorizationComleted;

		public VkApi VkApi;

		public Authorization(string token, bool re_authorization = false)
		{
			VkApi = new VkApi();

			VkApi.AuthorizeAsync(new ApiAuthParams
			{
				AccessToken = token
			});
			if (!re_authorization)
			{
				OnAuthorizationComleted?.Invoke(VkApi);
			}
		}
	}
}
