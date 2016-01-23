using UnityEngine;
using System.Collections;

/// <summary>
/// The main access to the leaderboards
/// </summary>
public class Leaderboard_accessAPI : MonoBehaviour {

    /// <summary>
    /// Check if the friends leaderboard is loaded
    /// </summary>
    public bool isLoaded { get; protected set; }

    /// <summary>
    /// Load score to the friends list
    /// </summary>
    public virtual void LoadScore() {}

    /// <summary>
    /// Send the player score to the servers
    /// </summary>
    public virtual void SendScore() {}

    /// <summary>
    /// Get the picture related to a network account
    /// </summary>
    /// <returns> A picture </returns>
    protected virtual Sprite GetUserRelatedPicture() { return null; }
}
