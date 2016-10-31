using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class ObstacleScript : Photon.PunBehaviour, IPunObservable
	{

		public static float LiveTime = 10f;

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
			if (photonView.isMine == false && PhotonNetwork.connected == true) {
				return;
			}
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
			if (obj.CompareTag ("Element")) {
				Debug.Log ("Obstacle: an element hits me");
				ResistanceCount = 0;
				if (photonView.isMine == true && PhotonNetwork.connected == true) {
					PhotonView pv = obj.transform.GetComponent<PhotonView> ();
					pv.RPC ("TakeDamage", PhotonTargets.All, Constant.ObstacleCollisionDamage);
				}
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