using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Facebook;

namespace Mastodonte.ElectricUniverse.Leaderboard {

    /// <summary>
    /// The combined friends leaderboard of facebook and google play
    /// </summary>
    public class FriendsLeaderboard {
        /// <summary>
        /// Facebook Friends Leaderboard
        /// </summary>
        public FacebookFriendsLeaderboard facebook;

        /// <summary>
        /// Google play friends Leaderboard
        /// </summary>
        public GoogleplayFriendsLeaderboard googlePlay;

        public FriendsLeaderboard(GameObject gameObject) {
            gameObject.AddComponent<FacebookFriendsLeaderboard>();
            gameObject.AddComponent<GoogleplayFriendsLeaderboard>();
        }

//--------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Get the combined score of facebook and google play
        /// </summary>
        /// <returns> Array of Friends Highscore </returns>
        public List<SocialNetworkUser_data> GetCombinedScore() {
            //TODO Combine facebook and google play (removing same person)
            List<FacebookUser> facebookData = facebook.friendScoreList;
            List<GoogleplayUser> googleplayData = googlePlay.friendsScore;

            //If the list is empty, so he has no facebook friends or is not connected
            if (facebookData.Count == 0 && googleplayData.Count == 0) return null;
            if (facebookData.Count == 0) return SocialNetworkUser_data.AsSocialNetworkUser(googleplayData);
            if (googleplayData.Count == 0) return SocialNetworkUser_data.AsSocialNetworkUser(facebookData);

            List<SocialNetworkUser_data> data = new List<SocialNetworkUser_data>();

            //Check if same person
            foreach (FacebookUser user in facebookData) {
                googleplayData.RemoveAll(x => x.name == user.name && x.score == user.score);
            }

            //Add them to same list
            foreach (FacebookUser fUser in facebookData) {
                data.Add(fUser);
            }
            foreach (GoogleplayUser gUser in googleplayData) {
                data.Add(gUser);
            }
            

            return data;
        }

        /// <summary>
        /// Check if the scores are loaded from facebook and google play
        /// </summary>
        /// <returns> True if the score is loaded </returns>
        public bool IsLoaded() {
            return facebook.isLoaded && googlePlay.isLoaded;
        }

        /// <summary>
        /// Refreshes the score from facebook and googleplay
        /// </summary>
        public void LoadScore() {
            facebook.LoadScore();
            googlePlay.LoadScore();
        }
    }
}
