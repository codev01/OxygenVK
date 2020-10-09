using VkNet;
using VkNet.Model;

namespace OxygenVK.AppSource.Authorization
{
	public class Authorize
	{
		public delegate void AuthorizationCompleted(VkApi vkApi);
		public static event AuthorizationCompleted OnAuthorizationComleted;

		public VkApi VkApi;

		public Authorize(string token, bool re_authorization = false)
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
