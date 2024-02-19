using System.Net.Http.Json;
using NewUserManagement.Client.Static;
using NewUserManagement.Shared.Models;


namespace NewUserManagement.Client.Services
{
    internal sealed class InMemoryDatabaseCache
    {
        private readonly HttpClient _httpClient;

        public InMemoryDatabaseCache(HttpClient httpClient)
        {
            _httpClient = httpClient;
            OnUsersDataChanged += delegate { }; // Initializing with an empty delegate
        }

        private List<User>? _users = null;
        internal List<User> Users
        {
            get
            {
                return _users ?? new List<User>();
            }
            set
            {
                _users = value;
                NotifyUsersDataChanged();
            }
        }
        private bool _gettingUsersFromDatabaseAndCaching = false;
        internal async Task GetUsersFromDatabaseAndCache(List<User>? users)
        {
            if (_gettingUsersFromDatabaseAndCaching == false)
            {
                _gettingUsersFromDatabaseAndCaching = true;
                _users = await _httpClient.GetFromJsonAsync<List<User>>(API_EndPoints.s_user);
                _gettingUsersFromDatabaseAndCaching = false;
            }
        }

        internal event Action? OnUsersDataChanged;

        private void NotifyUsersDataChanged() => OnUsersDataChanged?.Invoke();  

    }
}
