using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System;

/// <summary>
/// Utils Function related to the Google Play API
/// </summary>
public class GooglePlayManager {

    //Constructor
    public GooglePlayManager() {
        Init();
    }

    /// <summary>
    /// Initialize Google Play
    /// </summary>
    private void Init() {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
    }

    /// <summary>
    /// Connect the player to google play
    /// </summary>
    public void LoginToGooglePlay() {
        Social.localUser.Authenticate((bool success) => {
            Debug.Log(success ? "User successfuly connected to Google Play" : "User failed to connect to Google Play");
        });
    }

    /// <summary>
    /// Get user info
    /// </summary>
    private void GetUserInfo() {
        PlayGamesPlatform instance= PlayGamesPlatform.Instance;
        GoogleplayUser user = new GoogleplayUser(instance.GetUserDisplayName(), 
                                                 Int32.Parse(instance.GetUserId()),
                                                 null/*TODO get image from url -> instance.GetUserImageUrl()*/);
        SocialNetworkManager_Persistent.Instance.userInformation.googleplay = user;
    }

    /// <summary>
    /// Disconnect the player from google play
    /// </summary>
    public void LogoutFromGooglePlay() {
        PlayGamesPlatform.Instance.SignOut();
        SocialNetworkManager_Persistent.Instance.userInformation.googleplay = null;
    }
}
