using System;

using VkNet;
using VkNet.Model;

namespace OxygenVK.AppSource.Authorization
{
	public static class Authorize
	{
		public static VkApi AuthorizeAsync(string token)
		{
			VkApi vkApi = new VkApi();
			vkApi.Authorize(new ApiAuthParams
			{
				AccessToken = token
			});
			if (vkApi.Token == null)
			{
				throw new Exception("class Authorize | Токен пуст");
			} 
			return vkApi;
		}
	}
}
