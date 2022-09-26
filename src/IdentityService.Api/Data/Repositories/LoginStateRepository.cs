using Dapr.Client;
using IdentityService.Api.Data.Models;

namespace IdentityService.Api.Data.Repositories
{
    public class LoginStateRepository : ILoginStateRepository
    {
        private const string LOGIN_STATE_DAPR_STORE_NAME = "statestore";
        private readonly DaprClient _daprClient;

        public LoginStateRepository(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }
        public async Task<LoginStateModel> GetUserLoginStateAsync(string username)
        {
            var stateEntry = await _daprClient.GetStateEntryAsync<LoginStateModel>(LOGIN_STATE_DAPR_STORE_NAME, username);
            return stateEntry.Value;
        }

        public async Task SaveUserLoginStateAsync(LoginStateModel userState)
        {
            await _daprClient.SaveStateAsync<LoginStateModel>(LOGIN_STATE_DAPR_STORE_NAME, userState.Username, userState);
        }
    }
}
