
var createState = function () {
    return "SessionValueMakeItABitLongerascfgasdvfhasfygsadjfbsjdf";
};

var createNonce = function () {
    return "NonceValueasfcqwecascsadfasd123";
};

var signIn = function () {

    var redirectUri = "https://localhost:44394/Home/SignIn";
    var responseType = "id_token token";
    var scope = "openid ApiOne";

    var authUrl = `/connect/authorize/callback?client_id=client_id_js?redirect_uri=${encodeURIComponent(redirectUri)}?response_type=${encodeURIComponent(responseType)}?scope=${encodeURIComponent(scope)}?nonce=${createNonce()}?state=${createState()}`;

    var returnUrl = encodeURI(authUrl);

    console.log(authUrl);
    console.log(returnUrl);

    window.location.href = "https://localhost:44338/Auth/Login?ReturnUrl=" + returnUrl;
}