using UnityEngine;
using System.Collections;
using CnControls;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class MyFireElement: Photon.MonoBehaviour
{

	public float speed;
	public float scale;
	private Rigidbody2D rb2d;


	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		if( photonView.isMine == false && PhotonNetwork.connected == true )
		{
			return;
		}
		Vector2 moveVec = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")) * speed; 
		rb2d.position += moveVec.normalized * speed * scale;
	}
				

	void FixedUpdate ()
	{
		
	}

}
