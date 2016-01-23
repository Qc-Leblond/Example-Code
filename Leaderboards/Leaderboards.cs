using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mastodonte.ElectricUniverse.Leaderboard {

    /// <summary>
    /// Leaderboards access
    /// </summary>
    public class Leaderboards {

        #region Construction

        private FriendsLeaderboard friendsLeaderboard;
        private WorldLeaderboard worldLeaderboard;

        public Leaderboards(GameObject gameObject) {
            if (friendsLeaderboard == null) {
                friendsLeaderboard = new FriendsLeaderboard(gameObject);
            }
            if (worldLeaderboard == null) {
                worldLeaderboard = gameObject.AddComponent<WorldLeaderboard>();
            } 
        }

        #endregion

        #region Friends Leaderboard
        /// <summary>
        /// Get the informations of all friends on the games leaderboard from facebook and googleplay
        /// </summary>
        /// <returns> A list of Friend's Highscore </returns>
        static List<SocialNetworkUser_data> GetFriendsLeaderboard() {
            if (!Leaderboards.IsFriendsLeaderboardLoaded()) {
                Debug.LogWarning("Trying to access the friends leaderboard without loading it first or waiting for it to be loaded");
                return null;
            }
            return MetaManager_Persistent.Instance
                                         .socialNetwork
                                         .leaderboards
                                         .friendsLeaderboard
                                         .GetCombinedScore();
        }

        /// <summary>
        /// Check if the friends leaderboard is loaded from the facebook and google play servers
        /// </summary>
        /// <returns></returns>
        static bool IsFriendsLeaderboardLoaded() {
            return MetaManager_Persistent.Instance
                                         .socialNetwork
                                         .leaderboards
                                         .friendsLeaderboard
                                         .IsLoaded();
        }

        /// <summary>
        /// Refresh the current friends leaderboard data
        /// </summary>
        static void LoadFriendsLeaderboard() {
            MetaManager_Persistent.Instance
                                  .socialNetwork
                                  .leaderboards
                                  .friendsLeaderboard
                                  .LoadScore();
        }

        #endregion

        #region World Leaderboard

        /// <summary>
        /// Get the informations of the world leaderboard from google play
        /// </summary>
        /// <returns> A list of world Highscores </returns>
        static List<SocialNetworkUser_data> GetWorldLeaderboard() {
            if (!Leaderboards.IsWorldLeaderboardLoaded()) {
                Debug.LogWarning("Trying to access the world leaderboard without loading it first or waiting for it to be loaded");
                return null;
            }

            List<GoogleplayUser> users = MetaManager_Persistent.Instance
                                                               .socialNetwork
                                                               .leaderboards
                                                               .worldLeaderboard
                                                               .users;

            return SocialNetworkUser_data.AsSocialNetworkUser(users);
        }

        /// <summary>
        /// Check if the world leaderboard is loaded from google play servers
        /// </summary>
        /// <returns></returns>
        static bool IsWorldLeaderboardLoaded() {
            return MetaManager_Persistent.Instance
                                         .socialNetwork
                                         .leaderboards
                                         .worldLeaderboard
                                         .isLoaded;
        }

        /// <summary>
        /// Refresh the current world leaderboard data
        /// </summary>
        static void LoadWorldLeaderboard(bool centeredAroundPlayer, int playerLoadedCount) {
            MetaManager_Persistent.Instance
                                  .socialNetwork
                                  .leaderboards
                                  .worldLeaderboard
                                  .LoadScores(centeredAroundPlayer, playerLoadedCount);
        }

        #endregion

        #region Current Player Score

        /// <summary>
        /// Send the players highscore to google play and facebook
        /// </summary>
        static void SendScore() {
            Leaderboards leaderboards = MetaManager_Persistent.Instance.socialNetwork.leaderboards;
            leaderboards.friendsLeaderboard.facebook.SendScore();
            leaderboards.friendsLeaderboard.googlePlay.SendScore();
        }

        /// <summary>
        /// Makes sure that the highscore saved on the game is the right one by checking facebook and google play then sending the score if it's higher
        /// </summary>
        static void UpdateScore() {

        }

        #endregion
    }
}