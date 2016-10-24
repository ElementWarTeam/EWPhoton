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

		// [Tooltip ("The prefab to use for representing the player")]
		// public GameObject playerPrefab;

		#endregion

		#region Photon Messages

		/// <summary>
		/// Called when the local player left the room. We need to load the launcher scene.
		/// </summary>
		public override void OnLeftRoom ()
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

		void GenerateRandomObstacle ()
		{
			Vector2 point = new Vector2 (0.0f, 0.0f);
			point [0] = UnityEngine.Random.Range (-10.0f, 10.0f);
			point [1] = UnityEngine.Random.Range (-5.0f, 5.0f);
			PhotonNetwork.Instantiate ("Obstacle", point, Quaternion.identity, 0);
		}

		void GenerateRandomBloodPack ()
		{
			Vector2 point = new Vector2 (0.0f, 0.0f);
			point [0] = UnityEngine.Random.Range (-Constant.boundary_x, Constant.boundary_x);
			point [1] = UnityEngine.Random.Range (-Constant.boundary_y, Constant.boundary_y);
			PhotonNetwork.Instantiate ("HealthPack", point, Quaternion.identity, 0);
		}

		public void Start ()
		{
			Instance = this;
			if (PhotonNetwork.isMasterClient) {
				InvokeRepeating ("GenerateRandomObstacle", 1.0f, 2.0f);
				InvokeRepeating ("GenerateRandomBloodPack", 1.0f, 5.0f);
			}
			if (PlayerManager.LocalPlayerInstance == null) {
				Debug.Log ("We are Instantiating LocalPlayer from " + Application.loadedLevel);
				PhotonNetwork.Instantiate (PlayerManager.LocalPlayerType, new Vector3 (0f, 0f, 0f), Quaternion.identity, 0);
			} else {
				Debug.Log ("Ignoring scene load for " + Application.loadedLevel);
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
