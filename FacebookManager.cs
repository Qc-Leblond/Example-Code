using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;

/// <summary>
/// Utils function related to the facebook API
/// </summary>
public class FacebookManager {

    //CONSTRUCTOR
    public FacebookManager()
    {
        CallFBInit();
    }

    /// <summary> 
    /// Call once to initialize facebook. 
    /// </summary>
    private void CallFBInit() {
        FB.Init(delegate { Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn); }, //Check if the user is logged in
                delegate (bool isGameShown) { Debug.Log("Is game showing? " + isGameShown); });
    }

    /// <summary>
    /// Use to Share on facebook, will open a premade window
    /// </summary>
    /// <param name="link"> Shared link </param>
    /// <param name="linkName"> The name associated with the shared link </param>
    /// <param name="linkCaption"></param>
    /// <param name="linkDescription"> Description of the link. </param>
    /// <param name="picture"> The picture to show on the popup. </param>
    /// <param name="mediaSource"></param>
    public static void Share(string link = "", 
                      string linkName = "", 
                      string linkCaption = "", 
                      string linkDescription = "", 
                      string picture = "", 
                      string mediaSource = "")
    {
        FB.Feed(link: link,
            linkName: linkName,
            linkCaption: linkCaption,
            linkDescription: linkDescription,
            picture: picture,
            mediaSource: mediaSource,
            callback: delegate(FBResult result) { if (result.Error != null) Debug.LogWarning(result.Error); });
    }

    /// <summary>
    /// Used to connect to facebook
    /// </summary>
    public void LoginToFacebook() {
        if (!FB.IsLoggedIn) {
            FB.Login("user_games_activity,publish_action,public_profile,email,user_friends", 
                delegate(FBResult result) { //Callback when the API has returned a value
                    Debug.Log("Facebook User.ID " + FB.UserId + " connected.");
                    if (result.Error == null) GetLoginInfo();
                    else Debug.LogWarning("Facebook connection error: " + result.Error);
                }
            );
        }
    }

    /// <summary>
    /// Logout from facebook
    /// </summary>
    public void LogoutFromFacebook() {
        FB.Logout();
        SocialNetworkManager_Persistent.Instance.userInformation.facebook = null;
    }

    /// <summary>
    /// Send an http get request to get the profile info of the current user. Only Call after login
    /// </summary>
    private void GetLoginInfo() {
        FB.API("/me?fields=id,first_name,last_name,picture", Facebook.HttpMethod.GET, APICallback);
    }

    /// <summary>
    /// Called after getting the user information
    /// </summary>
    /// <param name="result"> The result ofthe callback </param>
    private void APICallback(FBResult result) {
        if (result.Error != null) {
            Debug.LogError(result.Error);
            return;
        }

        var data = JSONNode.Parse(result.Text);
        FacebookUser user = new FacebookUser(data["first_name"] + data["last_name"], data["id"].AsInt, null /*TODO get picture data from lin data["picture"]*/);
        SocialNetworkManager_Persistent.Instance.userInformation.facebook = user;
    }

    /// <summary>
    /// Check if the facebook user is logged in
    /// </summary>
    /// <returns> True if the user is logged in </returns>
    public static bool IsLoggedIn() { return FB.IsLoggedIn; }
}
