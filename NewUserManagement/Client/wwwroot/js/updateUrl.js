window.updateUrlWithoutNavigation = function (route, logId, userId) {
    // Get the current pathname
    var pathname = window.location.pathname;

    // Check if the pathname contains "/logs/" or "/list/"
    var logsIndex = pathname.indexOf("/logs/");
    var listIndex = pathname.indexOf("/list/");

    // Determine the base path
    var basePath = "";
    if (logsIndex !== -1) {
        basePath = "/logs/";
    } else if (listIndex !== -1) {
        basePath = "/list/";
    }

    // Construct the URL based on the base path and route
    var url = `${basePath}${route}`;

    // If logId is provided, append it to the URL
    if (logId !== undefined) {
        url += `/${logId}`;
    }

    // If userId is provided, append it to the URL
    if (userId !== undefined) {
        url += `/${userId}`;
    }

    // Update the URL without causing navigation
    history.replaceState({}, "", url);
};
