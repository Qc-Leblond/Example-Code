using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

/// <summary>
/// The current player social network information
/// </summary>
public class SocialNetworkPlayerUser {
    /// <summary>
    /// Is the player connected to facebook
    /// </summary>
    public bool facebookConnected { get; private set; }

    /// <summary>
    /// Is the player connected to google play
    /// </summary>
	public bool googleplayConnected {get; private set;}

    /// <summary>
    /// the connected player facebook information
    /// </summary>
    public FacebookUser facebook {
        get {
            return facebook;
        }
        set {
            facebookConnected = (value != null);
            facebook = value;
        }
    }

    /// <summary>
    /// the connected player google play information
    /// </summary>
    public GoogleplayUser googleplay {
        get {
            return googleplay;
        }
        set {
            googleplayConnected = (value != null);
            googleplay = value;
        }
    }

//----------------------------------------------------------------------------------------------------------------------

    public SocialNetworkPlayerUser() {
        facebookConnected = false;
        googleplayConnected = false;
        facebook = null;
        googleplay = null;
    }
}

/// <summary>
/// Data structure of a facebook user
/// </summary>
public class FacebookUser : SocialNetworkUser_data { 
    public FacebookUser(string name, int ID, Sprite picture) : base(name, ID, picture) {}
    public FacebookUser(string name, int ID, Sprite picture, int score) : base(name, ID, picture, score) { }

    //TODO
    public override Sprite GetRelatedPicture() {
        return new Sprite();
    }
}

/// <summary>
/// Data structure of a google play user
/// </summary>
public class GoogleplayUser : SocialNetworkUser_data {
    public GoogleplayUser(string name, int ID, Sprite picture) : base(name, ID, picture) { }
    public GoogleplayUser(string name, int ID, Sprite picture, int score) : base(name, ID, picture, score) { }

    //TODO
    public override Sprite GetRelatedPicture() {
        return new Sprite();
    }
}

/// <summary>
/// Data structure of a social network user
/// </summary>
public class SocialNetworkUser_data {
    /// <summary>
    /// User name
    /// </summary>
    public string name { get; private set; }

    /// <summary>
    /// User ID
    /// </summary>
    public int ID { get; private set; }

    /// <summary>
    /// User picture link
    /// </summary>
    public Sprite picture{get; private set;}

    /// <summary>
    /// The score related to the profile. 
    /// ONLY USE for friends, the score used for the current player should only be from the saved data in the MetaManager.
    /// </summary>
    public int score { get; private set; }

    public SocialNetworkUser_data(string _name, int _ID, Sprite _picture) {
        name = _name;
        ID = _ID;
        picture = _picture;
        score = 0;
    }

    public SocialNetworkUser_data(string _name, int _ID, Sprite _picture, int _score) {
        name = _name;
        ID = _ID;
        picture = _picture;
        score = _score;
    }

    public virtual Sprite GetRelatedPicture() { return new Sprite(); }

    /// <summary>
    /// Cast Facebook user to social network user
    /// </summary>
    /// <param name="users"></param>
    /// <returns></returns>
    public static List<SocialNetworkUser_data> AsSocialNetworkUser(List<FacebookUser> users) {
        List<SocialNetworkUser_data> data = new List<SocialNetworkUser_data>();
        for (int i = 0; i < users.Count; i++) {
            data.Add(users[i]);
        }
        return data;
    }

    /// <summary>
    /// Cast Googleplay user to social network user
    /// </summary>
    /// <param name="users"></param>
    /// <returns></returns>
    public static List<SocialNetworkUser_data> AsSocialNetworkUser(List<GoogleplayUser> users) {
        List<SocialNetworkUser_data> data = new List<SocialNetworkUser_data>();
        for (int i = 0; i < users.Count; i++) {
            data.Add(users[i]);
        }
        return data;
    }
}