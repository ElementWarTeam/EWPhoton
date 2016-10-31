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

		//the time hit other element
		private float hitTime = 0f;

		// Special effect of fireball
		private PlayerInfo playerBeHitted;

		void Start ()
		{
			audioSource = GetComponent<AudioSource> ();
			audioSource.PlayOneShot (shootAudio);
			initiateTime = Time.time;
		}

		void OnTriggerEnter2D (Collider2D obj)
		{
			if (photonView.isMine == false && PhotonNetwork.connected == true) {
				return;
			}

			// Fireball hit an element, which is not the owner of the fireball
			if (obj.CompareTag ("Element") && !obj.GetComponent<PlayerInfo> ().Equals (owner)) {
				Debug.Log ("FireBall: " + owner.name + "'s fireball hits " + obj.name);
				hitTime = Time.time;
				playerBeHitted = obj.GetComponent<PlayerInfo> ();
<<<<<<< HEAD
				// damage formula
				playerBeHitted.health -= (1 - playerBeHitted.defense) * damage;
				owner.GetComponent <PlayerInfo> ().score += 10;
=======
//				playerBeHitted.health -= damage;
//				owner.GetComponent <PlayerInfo> ().score += 10;

				obj.transform.GetComponent<PhotonView> ().RPC ("TakeDamage", PhotonTargets.All, damage);

>>>>>>> 9de5bd5bcbdcffd974c3e400576f4a01e8d0af6c
				shouldBeDestroied = true;
				audioSource.PlayOneShot (hitAudio);
				GetComponent <Renderer> ().enabled = false;
			}

			if (obj.CompareTag ("Obstacle")) {
				shouldBeDestroied = true;
				audioSource.PlayOneShot (hitAudio);
				GetComponent <Renderer> ().enabled = false;
				GetComponent <Collider2D> ().enabled = false;
			}

		}

		void Update ()
		{
			if (photonView.isMine == false && PhotonNetwork.connected == true) {
				return;
			}

			if (playerBeHitted != null && (hitTime + Constant.BasicEffectTime <= Time.time)) {
				playerBeHitted.health -= (1 - playerBeHitted.defense) * continousFireBallHealthDamage;
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
