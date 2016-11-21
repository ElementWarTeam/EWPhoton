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
			if (ResistanceCount == 0 || initiateTime + LiveTime <= Time.time) {
				if (PhotonNetwork.isMasterClient == true) {
					PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
					Destroy (gameObject);
				} else {
					HideSelf ();
				}
			}
		}

		void OnTriggerEnter2D (Collider2D obj)
		{
			PhotonView pv = obj.transform.GetComponent<PhotonView> ();
			if (obj.CompareTag ("Element")) {
				Debug.Log ("Obstacle: an element hits me");
				this.photonView.RPC ("TakeHit", PhotonTargets.All);
				if (photonView.isMine == true && PhotonNetwork.connected == true) {
					pv.RPC ("TakeDamage", PhotonTargets.All, Constant.ObstacleCollisionDamage);
				}
			}

			if (obj.CompareTag ("Bullet")) {
				this.photonView.RPC ("TakeHit", PhotonTargets.All);
			}

		}

		void beHitted ()
		{
			ResistanceCount -= 1;
		}

		void HideSelf ()
		{
			GetComponent <Renderer> ().enabled = false;
			GetComponent <Collider2D> ().enabled = false;
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

		[PunRPC]
		public void TakeHit ()
		{
			beHitted ();
		}

	}

}