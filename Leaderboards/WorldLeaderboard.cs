using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;

namespace Mastodonte.ElectricUniverse.Leaderboard {

    /// <summary>
    /// Google play access to the world leaderboard
    /// </summary>
    public class WorldLeaderboard : Leaderboard_accessAPI {

        public List<GoogleplayUser> users { get; private set; }

        void Awake() {
            this.isLoaded = false;
        }

        /// <summary>
        /// Load Score from the google play API
        /// </summary>
        /// <param name="centeredAroundPlayer"> True if centered around the player, false if the top of the leaderboard</param>
        /// <param name="playerLoaded"> The number of player scores to get  around the chosen position </param>
        public void LoadScores(bool centeredAroundPlayer, int playerLoaded) {
            this.isLoaded = false;
            PlayGamesPlatform.Instance.LoadScores("CgkIsNTDz_0DEAIQBg",
                                                  centeredAroundPlayer ? LeaderboardStart.PlayerCentered : LeaderboardStart.TopScores,
                                                  playerLoaded,
                                                  LeaderboardCollection.Social,
                                                  LeaderboardTimeSpan.AllTime,
                                                  (data) => { LoadScoreIntoData(data); });
        }

        /// <summary>
        /// Make a list with every friends score
        /// </summary>
        /// <param name="data"></param>
        private void LoadScoreIntoData(LeaderboardScoreData data) {
            IScore[] scores = data.Scores;
            List<string> usersID = new List<string>();
            this.users = new List<GoogleplayUser>();

            foreach (IScore score in scores) {
                usersID.Add(score.userID);
            }

            //Get user info for each score
            Social.LoadUsers(usersID.ToArray(), (users) => {
                for (int i = 0; i < scores.Length; i++) {
                    IUserProfile user = FindUser(users, scores[i].userID);
                    this.users.Add(new GoogleplayUser(user.userName,
                                                  Int32.Parse(user.id),
                                                  Sprite.Create(user.image, new Rect(0, 0, user.image.width, user.image.height), new Vector2(0.5f, 0.5f)),
                                                  (int)scores[i].value));
                }

                this.isLoaded = true;
            });
        }

        /// <summary>
        /// Find the user profile from the given ID
        /// </summary>
        /// <param name="users"> List of users </param>
        /// <param name="ID"> ID of the wanted user </param>
        /// <returns> The IUserProfile related to the ID </returns>
        private IUserProfile FindUser(IUserProfile[] users, string ID) {
            foreach (IUserProfile user in users) {
                if (user.id == ID) return user;
            }
            return null;
        }
    }
}
