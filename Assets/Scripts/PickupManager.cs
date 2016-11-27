using UnityEngine;
using System.Collections.Generic;

namespace Com.EW.MyGame
{
	public class PickupManager : Photon.PunBehaviour
	{

		static public PickupManager Instance;

		void Start ()
		{
			Instance = this;
			if (PhotonNetwork.isMasterClient) {
				InvokeRepeating ("GenerateRandomObstacle", 0f, Constant.ObstacleGenerateInterval);
				InvokeRepeating ("GenerateRandomBloodPack", 0f, Constant.HealthPackGenerateInterval);
				InvokeRepeating ("GenerateRandomSpeedUp", 0f, Constant.SpeedUpGenerateInterval);
				InvokeRepeating ("GenerateRandomPowerUp", 0f, Constant.PowerUpGenerateInterval);
				InvokeRepeating ("GenerateRandomBlackHole", 0f, Constant.BlackHoleInterval);
			}
		}

		void Update ()
		{
			
		}

		void GenerateRandomObstacle ()
		{
			GetComponent <PhotonView> ().RPC ("PunGeneratePickUp", PhotonTargets.All, "Obstacle");
//			Vector2 point = Vector2Extension.RandomPosition ();
//			GameObject obj = PhotonNetwork.Instantiate ("Obstacle", point, Quaternion.identity, 0);
//			DontDestroyOnLoad (obj);
		}

		void GenerateRandomBloodPack ()
		{
			GetComponent <PhotonView> ().RPC ("PunGeneratePickUp", PhotonTargets.All, "HealthPack");
//			Vector2 point = Vector2Extension.RandomPosition ();
//			GameObject obj = PhotonNetwork.Instantiate ("HealthPack", point, Quaternion.identity, 0);
//			DontDestroyOnLoad (obj);
		}

		void GenerateRandomSpeedUp ()
		{
			GetComponent <PhotonView> ().RPC ("PunGeneratePickUp", PhotonTargets.All, "SpeedUpPickUp");
//			Vector2 point = Vector2Extension.RandomPosition ();
//			GameObject obj = PhotonNetwork.Instantiate ("SpeedUpPickUp", point, Quaternion.identity, 0);
//			DontDestroyOnLoad (obj);
		}

		void GenerateRandomPowerUp ()
		{
			GetComponent <PhotonView> ().RPC ("PunGeneratePickUp", PhotonTargets.All, "PowerUpPickUp");
//			Vector2 point = Vector2Extension.RandomPosition ();
//			GameObject obj = PhotonNetwork.Instantiate ("PowerUpPickUp", point, Quaternion.identity, 0);
//			DontDestroyOnLoad (obj);
		}

		void GenerateRandomBlackHole ()
		{
			GetComponent <PhotonView> ().RPC ("PunGeneratePickUp", PhotonTargets.All, "BlackHole");
//			Vector2 point = Vector2Extension.RandomPosition ();
//			GameObject obj = PhotonNetwork.Instantiate ("BlackHole", point, Quaternion.identity, 0);
//			DontDestroyOnLoad (obj);
		}

		[PunRPC]
		public void PunGeneratePickUp (string prefebName)
		{
			Vector2 point = Vector2Extension.RandomPosition ();
			GameObject obj = PhotonNetwork.Instantiate (prefebName, point, Quaternion.identity, 0);
			DontDestroyOnLoad (obj);
		}
	}
}
