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
//				InvokeRepeating ("GenerateRandomBloodPack", 0f, Constant.HealthPackGenerateInterval);
//				InvokeRepeating ("GenerateRandomSpeedUp", 0f, Constant.SpeedUpGenerateInterval);
//				InvokeRepeating ("GenerateRandomPowerUp", 0f, Constant.PowerUpGenerateInterval);
//				InvokeRepeating ("GenerateRandomBlackHole", 0f, Constant.BlackHoleInterval);
			}
		}

		void Update ()
		{
			
		}

		void GenerateRandomObstacle ()
		{
			Vector2 point = Vector2Extension.RandomPosition ();
			GameObject obj = PhotonNetwork.Instantiate ("Obstacle", point, Quaternion.identity, 0);
			DontDestroyOnLoad (obj);
		}

		void GenerateRandomBloodPack ()
		{
			Vector2 point = Vector2Extension.RandomPosition ();
			GameObject obj = PhotonNetwork.Instantiate ("HealthPack", point, Quaternion.identity, 0);
			DontDestroyOnLoad (obj);
		}

		void GenerateRandomSpeedUp ()
		{
			Vector2 point = Vector2Extension.RandomPosition ();
			GameObject obj = PhotonNetwork.Instantiate ("SpeedUpPickUp", point, Quaternion.identity, 0);
			DontDestroyOnLoad (obj);
		}

		void GenerateRandomPowerUp ()
		{
			Vector2 point = Vector2Extension.RandomPosition ();
			GameObject obj = PhotonNetwork.Instantiate ("PowerUpPickUp", point, Quaternion.identity, 0);
			DontDestroyOnLoad (obj);
		}

		void GenerateRandomBlackHole ()
		{
			Vector2 point = Vector2Extension.RandomPosition ();
			GameObject obj = PhotonNetwork.Instantiate ("BlackHole", point, Quaternion.identity, 0);
			DontDestroyOnLoad (obj);
		}

		//		[PunRPC]
		//		public void DestroyPickUp (GameObject obj)
		//		{
		//			if (PhotonNetwork.isMasterClient) {
		//				PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
		//				Destroy (gameObject);
		//			} else {
		//				obj.GetComponent <Renderer> ().enabled = false;
		//				obj.GetComponent <Collider2D> ().enabled = false;
		//			}
		//		}
	}
}
