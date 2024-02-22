window.updateUrlWithoutNavigation = function (route, userId) {
    // Construct the URL based on the route and userId
    var url = userId !== null ? `/list/${route}/${userId}` : `/list/${route}`;

    // Update the URL without causing navigation
    history.replaceState({}, "", url);
};


