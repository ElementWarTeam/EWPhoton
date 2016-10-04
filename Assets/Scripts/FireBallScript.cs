using UnityEngine;
using System.Collections;

public class FireBallScript : MonoBehaviour
{

	public float speed = 6;
	public AudioClip shootAudio;
	public AudioClip hitAudio;

	private Rigidbody2D rb2d;
	private bool shouldBeDestroied = false;
	private AudioSource audioSource;

	public float initiateTime = 0f;

	// Use this for initialization
	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.velocity = new Vector2 (0, 0);
		audioSource = GetComponent<AudioSource> ();
		audioSource.PlayOneShot (shootAudio);
		initiateTime = Time.time;
	}

	// Update is called once per frame
	void Update ()
	{
		if ((!audioSource.isPlaying && shouldBeDestroied) || (initiateTime + 5f <= Time.time)) {
			Destroy (gameObject);
		}
	}
		
	// Function called when the enemy collides with another object
	void OnTriggerEnter2D (Collider2D obj)
	{
		if (shouldBeDestroied)
			return;
		if (obj.CompareTag ("Element") && !obj.name.Equals ("FireElement")) {
			audioSource.PlayOneShot (hitAudio);
			GetComponent <Renderer> ().enabled = false;
			shouldBeDestroied = true;
		}
	}
}
