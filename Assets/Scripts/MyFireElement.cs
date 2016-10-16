using UnityEngine;
using System.Collections;
using CnControls;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class MyFireElement: Photon.MonoBehaviour
{

	public float speed;
	public float scale = 0.01f;

	private Rigidbody2D rb2d;
	private Transform fireBallInstance;

	#region MONOBEHAVIOUR MESSAGES

	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		if (photonView.isMine == false && PhotonNetwork.connected == true) {
			return;
		}
	}

	void FixedUpdate ()
	{
		if (photonView.isMine == false && PhotonNetwork.connected == true) {
			return;
		}

		Vector2 moveVec = new Vector2 (CrossPlatformInputManager.GetAxis ("LeftHorizontal"), CrossPlatformInputManager.GetAxis ("LeftVertical")) * speed; 
		rb2d.position += moveVec.normalized * speed * scale;
		Vector2 shootVec = new Vector2 (CnInputManager.GetAxis ("Horizontal"), CnInputManager.GetAxis ("Vertical"));
		if (shootVec.magnitude > 0) {
			float angle = Mathf.Atan2 (shootVec.y, shootVec.x) * Mathf.Rad2Deg - 90f;
			rb2d.rotation = angle;
		}

	}

	#endregion
}
