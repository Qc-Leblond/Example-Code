using UnityEngine;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using System;

namespace Mastodonte.ElectricUniverse.Leaderboard {
    /// <summary>
    /// Google play access to the friends leaderboard
    /// </summary>
    public class GoogleplayFriendsLeaderboard : Leaderboard_accessAPI {

        /// <summary>
        /// List of Friend's score
        /// </summary>
        public List<GoogleplayUser> friendsScore { get; private set; }

        /// <summary>
        /// The static id of the main score board
        /// </summary>
        const string ID_BOARD = "CgkIsNTDz_0DEAIQBg";

        void Awake() {
            isLoaded = false;
        }

        public override void LoadScore() {
            this.isLoaded = false;
            PlayGamesPlatform.Instance.LoadScores(ID_BOARD,
                                                  LeaderboardStart.TopScores,
                                                  100,
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
            this.friendsScore = new List<GoogleplayUser>();

            foreach (IScore score in scores) {
                usersID.Add(score.userID);
            }

            //Get user info for each score
            Social.LoadUsers(usersID.ToArray(), (users) => {
                for (int i = 0; i < scores.Length; i++) {
                    IUserProfile user = FindUser(users, scores[i].userID);
                    if (user.isFriend) {
                        this.friendsScore.Add(new GoogleplayUser(user.userName,
                                                                 Int32.Parse(user.id),
                                                                 Sprite.Create(user.image, new Rect(0, 0, user.image.width, user.image.height), new Vector2(0.5f, 0.5f)),
                                                                 (int)scores[i].value));
                    }
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

        #region Utils

        /// <summary>
        /// Sends the score to the Google Play platform
        /// </summary>
        public override void SendScore() {
            Social.ReportScore(MetaManager_Persistent.Instance.metaData.savedHighscore.highestScore, "CgkIsNTDz_0DEAIQBg", (bool success) => {
                Debug.Log(success ? "The score was sent successfuly to Google Play" : "The score has failed to reach Google Play");
            });
        }

        #endregion

    }
}