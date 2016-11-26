using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class BlackHole : Photon.PunBehaviour, IPunObservable
	{

		public static float LiveTime = 15f;

		private float initiateTime = 0f;

		// Use this for initialization
		void Start ()
		{
			initiateTime = Time.time;
		}

		// Update is called once per frame
		void Update ()
		{
			transform.Rotate (Vector3.forward, 10f * Time.deltaTime);
			if (photonView.isMine == false && PhotonNetwork.connected == true) {
				return;
			}
			if (initiateTime + LiveTime <= Time.time) {
				if (photonView.isMine == true && PhotonNetwork.connected == true) {
					PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
					Destroy (gameObject);
				} else {
					HideSelf ();
				}
			}
		}

		void HideSelf ()
		{
			GetComponent <Renderer> ().enabled = false;
			GetComponent <Collider2D> ().enabled = false;
		}

		void OnTriggerEnter2D (Collider2D obj)
		{
			PhotonView pv = obj.transform.GetComponent<PhotonView> ();
			if (obj.CompareTag ("Element")) {
				Debug.Log ("Blackhole: an element hits me");
				if (photonView.isMine == true && PhotonNetwork.connected == true) {
					pv.RPC ("ChangeLocation", PhotonTargets.All, Vector2Extension.RandomPosition ());
				}
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