using UnityEngine;
using System.Collections;

public class ElectricFieldScript : Photon.PunBehaviour
{

	public float existTime = 10f;

	private float initiateTime = 0f;

	// Use this for initialization
	void Start ()
	{
		initiateTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (initiateTime + existTime <= Time.time) {
			PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D obj)
	{
		Debug.Log ("ElectricField OnTriggerEnter2D");
	}
}
