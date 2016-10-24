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



	#endregion
}
