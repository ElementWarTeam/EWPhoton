using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class StoneCharge : Photon.PunBehaviour, IPunObservable
	{

		public AudioClip shootAudio;
		public AudioClip hitAudio;

		public float damage;
		public float continousDamage;

		private PlayerInfo owner;
		private bool shouldBeDestroied = false;
		private AudioSource audioSource;
		private float initiateTime = 0f;

		// Special effect of fireball
		private PlayerInfo playerBeHitted;

		void Start ()
		{
			audioSource = GetComponent<AudioSource> ();
			audioSource.PlayOneShot (shootAudio);
			initiateTime = Time.time;
		}

		void Update ()
		{

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