using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using SimpleJSON;

namespace Mastodonte.ElectricUniverse.Leaderboard {

    /// <summary>
    /// Facebook access to the leaderboard
    /// </summary>
    public class FacebookFriendsLeaderboard : Leaderboard_accessAPI {

        /// <summary>
        /// List of user friends
        /// </summary>
        public List<FacebookUser> friendScoreList = new List<FacebookUser>();

        void Awake() {
            isLoaded = false;
        }

        /// <summary> 
        /// Function used to refresh the score. 
        /// </summary>
        public override void LoadScore() {
            isLoaded = false;
            friendScoreList.Clear();
            GetScore();
        }

        #region Get Score

        /// <summary> 
        /// Request the score of each of the user friends that plays the game and the current user score. 
        /// </summary>
        private void GetScore() {
            FB.API("/app/scores?fields=score,user.limit(30)", Facebook.HttpMethod.GET, ScoreCallback);
        }

        /// <summary> 
        /// Callback when the API request is done. 
        /// </summary>
        /// <param name="result"></param>
        private void ScoreCallback(FBResult result) {
            StartCoroutine(LoadScoreIntoData(result));
        }

        /// <summary> 
        /// Callback for the current user score. 
        /// </summary>
        /// <param name="result"></param>
        private void CurrentUserScoreCallback(FBResult result) {
            if (result.Error != null) {
                Debug.LogWarning(result.Error);
                return;
            }

            var data = JSONNode.Parse(result.Text);
            MetaManager_Persistent.Instance.metaData.savedHighscore.AddScore(data[0][0]["score"].AsInt);
            SendScore();
        }

        /// <summary> 
        /// Load the score returned by the facebook API. 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private IEnumerator LoadScoreIntoData(FBResult result) {
            if (result.Error != null) Debug.LogWarning(result.Error);
            var data = JSONNode.Parse(result.Text);
            for (int i = 0; i < data[0].Count; i++) {
                friendScoreList.Add(new FacebookUser(data[0][i]["user"]["name"],
                                    Int32.Parse(data[0][i]["user"]["id"]),
                                    null,
                                    data[0][i]["score"].AsInt));
            }

            SortFriendList();
            isLoaded = true;
            yield return null;
        }

        /// <summary> 
        /// Sort higher score first in the friendsList. 
        /// </summary>
        private void SortFriendList() {
            friendScoreList.Sort((x, y) => x.score.CompareTo(y.score));
            friendScoreList.Reverse();
        }

        #endregion

        #region Send Score

        /// <summary>
        /// Send the score to facebook API
        /// </summary>
        public void SendScore() {
            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("score", MetaManager_Persistent.Instance.metaData.savedHighscore.highestScore.ToString());
            if (FB.IsLoggedIn) FB.API("me/scores", Facebook.HttpMethod.POST, ScoreSendCallBack, query);
        }

        /// <summary>
        /// Callback when the score is completly sent
        /// </summary>
        /// <param name="result"> Result facebook </param>
        private void ScoreSendCallBack(FBResult result) {
            if (result.Error != null) Debug.LogWarning(result.Error);
        }

        #endregion
    }
}
