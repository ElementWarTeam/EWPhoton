using UnityEngine;
using System.Collections.Generic;

namespace Com.EW.MyGame
{
	public class PickupManager : Photon.PunBehaviour
	{

		static public PickupManager Instance;

		private Queue<GameObject> obstacleQueue;
		private Queue<GameObject> healthPackQueue;

		void Start ()
		{
			Instance = this;
			if (PhotonNetwork.isMasterClient) {
				InvokeRepeating ("GenerateRandomObstacle", 0f, Constant.ObstacleGenerateInterval);
				InvokeRepeating ("GenerateRandomBloodPack", 0f, Constant.HealthPackGenerateInterval);
			}
		}

		void Update ()
		{
			
		}

		void GenerateRandomObstacle ()
		{
			Vector2 point = randomPosition ();
			GameObject obj = PhotonNetwork.Instantiate ("Obstacle", point, Quaternion.identity, 0);
//			obstacleQueue.Enqueue (obj);
			DontDestroyOnLoad (obj);
		}

		void GenerateRandomBloodPack ()
		{
			Vector2 point = randomPosition ();
			GameObject obj = PhotonNetwork.Instantiate ("HealthPack", point, Quaternion.identity, 0);
//			healthPackQueue.Enqueue (obj);
			DontDestroyOnLoad (obj);
		}

		private Vector2 randomPosition ()
		{
			Vector2 position = new Vector2 (0.0f, 0.0f);
			position [0] = UnityEngine.Random.Range (-Constant.RANGE_X, Constant.RANGE_X);
			position [1] = UnityEngine.Random.Range (-Constant.RANGE_Y, Constant.RANGE_Y);
			return position;
		}

		[PunRPC]
		public void DestroyPickUp (GameObject obj)
		{
			if (PhotonNetwork.isMasterClient) {
				PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
				Destroy (gameObject);
			} else {
				obj.GetComponent <Renderer> ().enabled = false;
				obj.GetComponent <Collider2D> ().enabled = false;
			}
		}
	}
}
