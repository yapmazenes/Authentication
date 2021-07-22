var config = {
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
    authority: "https://localhost:44338/",
    client_id: "client_id_js",
    redirect_uri: "https://localhost:44394/Home/SignIn",
    response_type: "id_token token",
    scope: "openid api.scope ApiOne ApiTwo"
};

var userManager = new Oidc.UserManager(config);

var signIn = function () {

    userManager.signinRedirect();
};

userManager.getUser().then(user => {

    console.log("user: ", user);
    if (user) {
        axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
    }
});

var callApi = function () {
    axios.get('https://localhost:44326/secret')
        .then(res => {
            console.log(res);
        });
};

var refreshing = false;

axios.interceptors.response.use(function (response) {
    return response;
},
    function (error) {
        console.log("axios error: ", error.response);

        var axiosConfig = error.response.config

        //if error response is 401, try to refresh token
        if (error.response.status === 401) {
            console.log("axios error 401");

            //if already refreshing don't make another request
            if (!refreshing) {
                console.log("starting token refresh");
                refreshing = true;

                //do the refresh
                return userManager.signinSilent().then(user => {
                    console.log("userManager.signinSilent(): ", user);

                    axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
                    axiosConfig.headers["Authorization"] = "Bearer " + user.access_token;

                    //retry the http request
                    return axios(axiosConfig);
                });
            }

        }

        return Promise.reject(error);
    }
);