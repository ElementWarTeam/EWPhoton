using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class FireBall : Photon.PunBehaviour, IPunObservable
	{

		public AudioClip shootAudio;
		public AudioClip hitAudio;

		public float damage;
		public float continousDamage;

		private PlayerInfo owner;
		private bool shouldBeDestroied = false;
		private AudioSource audioSource;
		private float initiateTime = 0f;

		void Start ()
		{
			audioSource = GetComponent<AudioSource> ();
			audioSource.PlayOneShot (shootAudio);
			initiateTime = Time.time;
		}

		void OnTriggerEnter2D (Collider2D obj)
		{
			// Hit obj 
			Debug.Log ("FireBall: " + owner.name + "'s fireball hits " + obj.name);

			if (obj.CompareTag ("Element")) {
				if (!obj.GetComponent<PlayerInfo> ().Equals (owner)) {
					HideSelf ();
					if (photonView.isMine == true && PhotonNetwork.connected == true) {
						PhotonView pv = obj.transform.GetComponent<PhotonView> ();
						pv.RPC ("TakeDamage", PhotonTargets.All, damage);
						pv.RPC ("TakeContinousDamage", PhotonTargets.All, continousDamage);
						owner.GetComponent <PhotonView> ().RPC ("AddScore", PhotonTargets.All, damage);
					}
				}
			}

			if (obj.CompareTag ("Obstacle")) {
				HideSelf ();
				shouldBeDestroied = true;
				audioSource.PlayOneShot (hitAudio);
			}
		}

		void HideSelf ()
		{
			GetComponent <Renderer> ().enabled = false;
			GetComponent <Collider2D> ().enabled = false;
		}

		void Update ()
		{
			if (photonView.isMine == false && PhotonNetwork.connected == true) {
				return;
			}
			if ((!audioSource.isPlaying && shouldBeDestroied) || (initiateTime + Constant.LiveTime <= Time.time)) {
				PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
				Destroy (gameObject);
			}
		}

		// Getters and Setters

		public PlayerInfo getOwner ()
		{
			return owner;
		}

		public void setOwner (PlayerInfo owner)
		{
			this.owner = owner;
		}

		#region IPunObservable implementation

		void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting) {
				// We own this player: send the others our data
				stream.SendNext (damage);
				stream.SendNext (continousDamage);
				stream.SendNext (initiateTime);
				stream.SendNext (shouldBeDestroied);
			} else {
				// Network player, receive data
				this.damage = (float)stream.ReceiveNext ();
				this.continousDamage = (float)stream.ReceiveNext ();
				this.initiateTime = (float)stream.ReceiveNext ();
				this.shouldBeDestroied = (bool)stream.ReceiveNext ();
			}
		}

		#endregion
	}

}
