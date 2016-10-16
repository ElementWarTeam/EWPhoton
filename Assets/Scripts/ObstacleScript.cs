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
}
