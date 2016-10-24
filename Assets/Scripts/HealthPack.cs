using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class HealthPack : Photon.PunBehaviour, IPunObservable
	{

		public static float LiveTime = 20f;

		private float initiateTime = 0f;

		// Use this for initialization
		void Start ()
		{
			initiateTime = Time.time;
		}

		// Update is called once per frame
		void Update ()
		{
			if (initiateTime + LiveTime <= Time.time) {
				PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
				Destroy (gameObject);
			}
		}

		void OnTriggerEnter2D (Collider2D obj)
		{
			// if player collide with obstacle
			if (obj.CompareTag ("Element")) {
				Debug.Log ("HealthPack: an element hits me");
				PlayerInfo playerHitted = obj.GetComponent<PlayerInfo> ();
				playerHitted.health += Constant.HealthPackRecover;
				if (playerHitted.health > playerHitted.initialHealth) {
					playerHitted.health = playerHitted.initialHealth;
				}
				PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
				Destroy (gameObject);
			}

		}

		#region IPunObservable implementation

		void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting) {
				// We own this player: send the others our data
				stream.SendNext (initiateTime);
			} else {
				// Network player, receive data
				this.initiateTime = (float)stream.ReceiveNext ();
			}
		}

		#endregion
	}
}