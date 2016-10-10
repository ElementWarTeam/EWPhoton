using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


namespace Com.EW.MyGame
{
	public class GameManager : Photon.PunBehaviour
	{
		#region Public Variables

		static public GameManager Instance;

		[Tooltip ("The prefab to use for representing the player")]
		public GameObject playerPrefab;

		#endregion

		#region Photon Messages

		/// <summary>
		/// Called when the local player left the room. We need to load the launcher scene.
		/// </summary>
		public void OnLeftRoom ()
		{
			SceneManager.LoadScene (0);
		}

		public override void OnPhotonPlayerConnected (PhotonPlayer other)
		{
			Debug.Log ("OnPhotonPlayerConnected() " + other.name); // not seen if you're the player connecting


			if (PhotonNetwork.isMasterClient) {
				Debug.Log ("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected

				// When player number changes, we load different Arena
				LoadArena ();
			}
		}

		public override void OnPhotonPlayerDisconnected (PhotonPlayer other)
		{
			Debug.Log ("OnPhotonPlayerDisconnected() " + other.name); // seen when other disconnects


			if (PhotonNetwork.isMasterClient) {
				Debug.Log ("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected

				// When player number changes, we load different Arena
				LoadArena ();
			}
		}

		#endregion

		#region Public Methods

		public void Start ()
		{
			Instance = this;

			if (playerPrefab == null) {
				Debug.LogError ("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
			} else {
				Debug.Log ("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
				if (PlayerManager.LocalPlayerInstance == null) {
					Debug.Log ("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
					// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
					PhotonNetwork.Instantiate (this.playerPrefab.name, new Vector3 (0f, 0f, 0f), Quaternion.identity, 0);
				} else {
					Debug.Log ("Ignoring scene load for " + Application.loadedLevelName);
				}
			}

		}

		public void LeaveRoom ()
		{
			PhotonNetwork.LeaveRoom ();
		}

		#endregion

		#region Private Methods

		void LoadArena ()
		{
			if (!PhotonNetwork.isMasterClient) {
				Debug.LogError ("PhotonNetwork : Trying to Load a level but we are not the master Client");
			}
			Debug.Log ("PhotonNetwork : Loading Level : " + PhotonNetwork.room.playerCount);
			PhotonNetwork.LoadLevel ("Room for " + PhotonNetwork.room.playerCount);
		}

		#endregion
	}
}