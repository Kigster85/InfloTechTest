namespace NewUserManagement.Client.Static;

public static class API_EndPoints
{


#if DEBUG
    // Use this path in debug mode
    internal const string ServerBaseUrl = "https://localhost:5167";
#else
    // Use this production path in release mode
    internal const string ServerBaseUrl = "https://www.yourdomain.com";
#endif

    // Update the endpoint URLs based on the new Identity table names
    public readonly static string s_user = $"{ServerBaseUrl}/api/user"; // Endpoint for retrieving users
    internal readonly static string s_userById = $"{ServerBaseUrl}/api/user/{{userId}}"; // Endpoint for retrieving user by ID

    // You may add more endpoint URLs as needed for other API operations
}
