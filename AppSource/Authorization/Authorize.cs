using OxygenVK.Authorization;

using VkNet;
using VkNet.Model;

namespace OxygenVK.AppSource.Authorization
{
	public class Authorize
	{
		public delegate void AuthorizationCompleted(Parameter parameter);
		public static event AuthorizationCompleted OnAuthorizationCompleted;

		public VkApi VkApi = new VkApi();

		public Authorize(string token, bool re_Authorize = false)
		{
			AuthorizeAsync(token, re_Authorize);
		}

		private async void AuthorizeAsync(string token, bool re_Authorize)
		{
			await VkApi.AuthorizeAsync(new ApiAuthParams
			{
				AccessToken = token
			});

			if (!re_Authorize)
			{
				long userID = 0;
				foreach (User item in await VkApi.Users.GetAsync(new long[0]))
				{
					userID = item.Id;
				}

				ListOfAuthorizedUsers listOfAuthorizedUsers = new ListOfAuthorizedUsers();

				foreach (AuthorizedUserCardsAttachment item in ListOfAuthorizedUsers.listOfAuthorizedUsers)
				{
					if (item.UserID == userID)
					{
						listOfAuthorizedUsers.DeleteUserData(userID);
					}
				}
				listOfAuthorizedUsers.SetListOfAuthorizedUsersAsync(VkApi, userID);
				listOfAuthorizedUsers.InitializeList();

				OnAuthorizationCompleted?.Invoke(new Parameter()
				{
					UserID = userID,
					VkApi = VkApi
				});
			}
		}
	}
}
