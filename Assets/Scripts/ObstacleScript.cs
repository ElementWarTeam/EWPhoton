using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class ObstacleScript : Photon.PunBehaviour, IPunObservable
	{

		public static float LiveTime = 20f;

		public int ResistanceCount = 2;

		private float initiateTime = 0f;

		// Use this for initialization
		void Start ()
		{
			initiateTime = Time.time;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (ResistanceCount == 0) {
				PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
				Destroy (gameObject);
			}
			if (initiateTime + LiveTime <= Time.time) {
				PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
				Destroy (gameObject);
			}
		}

		void OnTriggerEnter2D (Collider2D obj)
		{
			// if player collide with obstacle

			if (obj.CompareTag ("Element")) {
				Debug.Log ("Obstacle: an element hits me");
//				obj.GetComponent<PlayerInfo> ().health -= Constant.ObstacleCollisionDamage;
				ResistanceCount = 0;
				obj.transform.GetComponent<PhotonView> ().RPC ("TakeDamage", PhotonTargets.All, Constant.ObstacleCollisionDamage);
			}

			if (obj.CompareTag ("Bullet")) {
				beHitted ();
			}

		}

		void beHitted ()
		{
			ResistanceCount -= 1;
		}

		#region IPunObservable implementation

		void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting) {
				// We own this player: send the others our data
				stream.SendNext (initiateTime);
				stream.SendNext (ResistanceCount);
			} else {
				// Network player, receive data
				this.initiateTime = (float)stream.ReceiveNext ();
				this.ResistanceCount = (int)stream.ReceiveNext ();
			}
		}

		#endregion

	}

}