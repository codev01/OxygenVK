using OxygenVK.Authorization;

using VkNet;
using VkNet.Model;

namespace OxygenVK.AppSource.Authorization
{
	public class Authorize
	{
		public VkApi VkApi = new VkApi();

		public Authorize(string token, bool re_Authorize = false) => AuthorizeAsync(token, re_Authorize);

		private async void AuthorizeAsync(string token, bool re_Authorize)
		{
		    await VkApi.AuthorizeAsync(new ApiAuthParams
			{
				AccessToken = token
			});

			if (!re_Authorize)
			{
				ListOfAuthorizedUsers listOfAuthorizedUsers = new ListOfAuthorizedUsers();

				listOfAuthorizedUsers.SetListOfAuthorizedUsersAsync(VkApi);
			}
		}
	}
}
