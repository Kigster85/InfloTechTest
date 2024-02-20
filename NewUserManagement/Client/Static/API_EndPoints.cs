namespace NewUserManagement.Client.Static;

internal static class API_EndPoints
{
#if DEBUG
    //Use this path//
    internal const string ServerBaseUrl = "https://localhost:7036";

#else
    //Use this Prod Path//
    internal const string ServerBaseUrl = "https://www.yourdomain.com";
#endif

    internal readonly static string s_user = $"{ServerBaseUrl}/api/user";
    internal readonly static string s_userById = $"{ServerBaseUrl}/api/user"; // Assuming the endpoint is /api/user/{id}

    //internal readonly static string s_signIn = $"{ServerBaseUrl}/api/signin";


}
