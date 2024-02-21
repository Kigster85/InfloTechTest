window.updateUrlWithoutNavigation = function (route, userId) {
    // Construct the URL based on the route and userId
    var url = userId ? `/list/${route}/${userId}` : `/list`;

    // Update the URL without causing navigation
    history.replaceState({}, "", url);
};

