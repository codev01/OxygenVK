using VkNet;
using VkNet.Model;

namespace OxygenVK.AppSource.Authorization
{
	public class Authorize
	{
		public delegate void AuthorizationCompleted(Parameter parameter);
		public static event AuthorizationCompleted OnAuthorizationComleted;

		public VkApi VkApi = new VkApi();

		public Authorize(string token, bool isRe_authorization = false)
		{
			AuthorizeAsync(token, isRe_authorization);
		}
		private async void AuthorizeAsync(string token, bool isRe_authorization = false)
		{
			await VkApi.AuthorizeAsync(new ApiAuthParams
			{
				AccessToken = token
			});

			long userID = 0;
			foreach (User item in VkApi.Users.Get(new long[0]))
			{
				userID = item.Id;
			}

			new OxygenVK.Authorization.ListOfAuthorizedUsers().SetListOfAuthorizedUsers(VkApi, userID, isRe_authorization);

			if (!isRe_authorization)
			{
				OnAuthorizationComleted?.Invoke(new Parameter()
				{
					UserID = userID,
					VkApi = VkApi
				});
			}
		}
	}
}
