using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class ElectricField : Photon.PunBehaviour, IPunObservable
	{

		public AudioClip electricFieldOnAudio;
		public AudioClip electricFieldHittingAudio;

		public float continousDamage;

		private PlayerInfo owner;
		private AudioSource audioSource;
		private float initiateTime = 0f;
		private float nextContinousDamageTime = 0f;

		public float existTime = Constant.ElectricFieldLiveTime;

		private GameObject parent;

		// Use this for initialization
		void Start ()
		{
			audioSource = GetComponent<AudioSource> ();
			audioSource.PlayOneShot (electricFieldOnAudio);
			initiateTime = Time.time;
			nextContinousDamageTime = initiateTime;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (photonView.isMine == false && PhotonNetwork.connected == true) {
				return;
			}

			if (initiateTime + existTime <= Time.time) {
				PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
				Destroy (gameObject);
			}
			if (photonView.isMine) {
				this.transform.position = parent.transform.position;
			}
		}

		void OnTriggerEnter2D (Collider2D obj)
		{
			if (obj.CompareTag ("Obstacle")) {
				PhotonNetwork.Destroy (obj.GetComponent <PhotonView> ());
				Destroy (obj);
			}
		}

		void OnTriggerStay2D (Collider2D obj)
		{
			// Hit obj 
			PhotonView pv = obj.transform.GetComponent<PhotonView> ();
			Debug.Log ("OnTriggerStay2D: " + owner.name + "'s electric field hits " + obj.name);

			if (obj.CompareTag ("Element")) {
				if (pv.owner.name.Equals (photonView.owner.name)) {
					return;
				} 
				if (photonView.isMine == true && PhotonNetwork.connected == true) {
					if (nextContinousDamageTime < Time.time) {
						nextContinousDamageTime = Time.time + 0.1f; // TODO: @Cairu
						pv.RPC ("TakeDamage", PhotonTargets.All, continousDamage); // TODO: @Cairu
						owner.GetComponent <PhotonView> ().RPC ("AddScore", PhotonTargets.All, 1f); // TODO: @Cairu
					}

				}
//				audioSource.PlayOneShot (hitAudio); // TODO: @cairu: electric sound
			}

			if (obj.CompareTag ("Obstacle")) {
				pv.RPC ("TakeHit", PhotonTargets.All);
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

		public void setParent (GameObject parent)
		{
			this.parent = parent; 
		}

		#region IPunObservable implementation

		void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting) {
				// We own this player: send the others our data
				stream.SendNext (continousDamage);
				stream.SendNext (initiateTime);
//				stream.SendNext (parent.transform.position);
			} else {
				// Network player, receive data
				this.continousDamage = (float)stream.ReceiveNext ();
				this.initiateTime = (float)stream.ReceiveNext ();
//				this.parent.transform.position = (Vector3)stream.ReceiveNext ();
			}
		}

		#endregion
	}
}