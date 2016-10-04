using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour
{

	public float speed = 0;

	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.velocity = new Vector2 (0, speed);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	// Function called when the enemy collides with another object
	void OnTriggerEnter2D (Collider2D obj)
	{
		// Name of the object that collided with the enemy
		string name = obj.gameObject.name;

		// If the enemy collided with a bullet
		if (name.Equals ("FireBall(Clone)")) {
			// Destroy itself (the enemy) and the bullet
			Destroy (gameObject);
			Destroy (obj.gameObject);
		}

		// If the enemy collided with the spaceship
		if (name.Equals ("FireElement")) {
			// Destroy itself (the enemy) to keep things simple
			Destroy (gameObject);
		}

		if (name.Equals ("wallpaper")) {
			// Destroy itself (the enemy) to keep things simple
			Destroy (gameObject);
		} 
	}
}
