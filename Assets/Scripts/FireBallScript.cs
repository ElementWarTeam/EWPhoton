using UnityEngine;
using System.Collections;

public class FireBallScript : Photon.PunBehaviour
{

	public AudioClip shootAudio;
	public AudioClip hitAudio;

	private Rigidbody2D rb2d;
	private Collider2D cl2d;
	private bool shouldBeDestroied = false;
	private AudioSource audioSource;

	private float initiateTime = 0f;

	// Use this for initialization
	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.velocity = new Vector2 (0, 0);
		cl2d = GetComponent<Collider2D> ();
		audioSource = GetComponent<AudioSource> ();
		audioSource.PlayOneShot (shootAudio);
		initiateTime = Time.time;
	}

	// Update is called once per frame
	void Update ()
	{
		if ((!audioSource.isPlaying && shouldBeDestroied) || (initiateTime + 5f <= Time.time)) {
			PhotonNetwork.Destroy (gameObject.GetComponent <PhotonView> ());
			Destroy (gameObject);
		}
	}
		
	// Function called when the enemy collides with another object
	void OnTriggerEnter2D (Collider2D obj)
	{
		Debug.Log ("OnTriggerEnter2D");
		Debug.Log ("shouldBeDestroied = " + shouldBeDestroied);
		if (shouldBeDestroied)
			return;
		if (obj.CompareTag ("Bullet") && !obj.name.Equals (cl2d.name)) {
			Debug.Log ("Bullet hit Bullet: shouldBeDestroied");
			audioSource.PlayOneShot (hitAudio);
			GetComponent <Renderer> ().enabled = false;
			shouldBeDestroied = true;
		}

		if (obj.CompareTag ("Obstacle")) {
			Debug.Log ("Bullet hit Obstacle: shouldBeDestroied");
			audioSource.PlayOneShot (hitAudio);
			GetComponent <Renderer> ().enabled = false;
			shouldBeDestroied = true;
		}
	}
}
