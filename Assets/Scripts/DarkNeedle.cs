using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class DarkNeedle : Photon.PunBehaviour, IPunObservable
	{

		public AudioClip shootAudio;
		public AudioClip hitAudio;

		public float damage;
		public float continousDarkNeedleExtractDamage;

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

//			this.GetComponent<Transform> ().localScale *= DarkElement.amplify;
		}

		void OnTriggerEnter2D (Collider2D obj)
		{
			// Hit obj 
			PhotonView pv = obj.transform.GetComponent<PhotonView> ();
			Debug.Log ("Bullet: " + photonView.owner.name + "'s bullet hits " + pv.name);

			if (obj.CompareTag ("Element")) {
				if (pv.owner.name.Equals (photonView.owner.name)) {
					return;
				} 
				HideSelf ();
				if (photonView.isMine == true && PhotonNetwork.connected == true) {
					Debug.Log ("DarkNeedle: " + owner.name + "'s DarkNeedle hits " + obj.name);
					pv.RPC ("TakeDamage", PhotonTargets.All, damage);
					owner.GetComponent <PhotonView> ().RPC ("AddHealth", PhotonTargets.All, continousDarkNeedleExtractDamage); // TODO: @Cairu: should be partial of the damage and calcualetd in DarkElement, done
					owner.GetComponent <PhotonView> ().RPC ("AddScore", PhotonTargets.All, damage);
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

			if (playerBeHitted != null && (hitTime + Constant.BasicEffectTime <= Time.time)) {
				playerBeHitted.health -= continousDarkNeedleExtractDamage;
				owner.health += continousDarkNeedleExtractDamage;
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
				stream.SendNext (continousDarkNeedleExtractDamage);
				stream.SendNext (initiateTime);
				stream.SendNext (shouldBeDestroied);
			} else {
				// Network player, receive data
				this.damage = (float)stream.ReceiveNext ();
				this.continousDarkNeedleExtractDamage = (float)stream.ReceiveNext ();
				this.initiateTime = (float)stream.ReceiveNext ();
				this.shouldBeDestroied = (bool)stream.ReceiveNext ();
			}
		}

		#endregion
	}
}