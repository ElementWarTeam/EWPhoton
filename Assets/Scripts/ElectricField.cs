﻿using UnityEngine;
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

		public float existTime = Constant.ElectricFieldLiveTime;

		private GameObject parent;

		// Use this for initialization
		void Start ()
		{
			audioSource = GetComponent<AudioSource> ();
			audioSource.PlayOneShot (electricFieldOnAudio);
			initiateTime = Time.time;
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
			if (photonView.isMine == false && PhotonNetwork.connected == true) {
				return;
			}

			// ElectricArc hit an element, which is not the owner of the ElectricArc
			if (obj.CompareTag ("Element") && !obj.GetComponent<PlayerInfo> ().Equals (owner)) {
				Debug.Log ("ElectricArc: " + owner.name + "'s ElectricArc hits " + obj.name);
				PlayerInfo playerBeHitted = obj.GetComponent<PlayerInfo> ();
				playerBeHitted.health -= continousDamage;
				owner.GetComponent <PlayerInfo> ().score += 10;
				audioSource.PlayOneShot (electricFieldHittingAudio);
				GetComponent <Renderer> ().enabled = false;
			}

			if (obj.CompareTag ("Obstacle")) {
				audioSource.PlayOneShot (electricFieldHittingAudio);
				PhotonNetwork.Destroy (obj.GetComponent <PhotonView> ());
				Destroy (obj);
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