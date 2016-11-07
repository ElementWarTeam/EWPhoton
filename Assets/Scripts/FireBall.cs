using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class FireBall : Photon.PunBehaviour, IPunObservable
	{

		public AudioClip shootAudio;
		public AudioClip hitAudio;

		public float damage;
		public float continousFireBallHealthDamage;

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
			PhotonView pv = obj.transform.GetComponent<PhotonView> ();
			Debug.Log ("FireBall: " + photonView.owner.name + "'s fireball hits " + pv.name);

			if (obj.CompareTag ("Element")) {
				if (pv.owner.name.Equals (photonView.owner.name)) {
					return;
				} 
				HideSelf ();
				if (photonView.isMine == true && PhotonNetwork.connected == true) {
					pv.RPC ("TakeDamage", PhotonTargets.All, damage);
					pv.RPC ("TakeContinousDamage", PhotonTargets.All, continousFireBallHealthDamage);
					owner.GetComponent <PhotonView> ().RPC ("AddScore", PhotonTargets.All, damage);
				}
				audioSource.PlayOneShot (hitAudio);
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

			if ((!audioSource.isPlaying && shouldBeDestroied) || (initiateTime + Constant.LiveTime <= Time.time)) {
				if (photonView.isMine == false && PhotonNetwork.connected == true) {
					HideSelf ();
				} else {
					PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
					Destroy (gameObject);
				}

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
				stream.SendNext (continousFireBallHealthDamage);
				stream.SendNext (initiateTime);
				stream.SendNext (shouldBeDestroied);
			} else {
				// Network player, receive data
				this.damage = (float)stream.ReceiveNext ();
				this.continousFireBallHealthDamage = (float)stream.ReceiveNext ();
				this.initiateTime = (float)stream.ReceiveNext ();
				this.shouldBeDestroied = (bool)stream.ReceiveNext ();
			}
		}

		#endregion
	}

}
