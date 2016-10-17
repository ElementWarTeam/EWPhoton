using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class ObstacleScript : MonoBehaviour
	{


		public float ObstacleCollisionDamage = 0.1f;

		private Rigidbody2D rb2d;

		// Use this for initialization
		void Start ()
		{
			rb2d = GetComponent<Rigidbody2D> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}

		void OnTriggerEnter2D (Collider2D obj)
		{
			// if player collide with obstacle
			if (obj.CompareTag ("Element")) {
				Debug.Log ("Obstacle: an element hits me");
				obj.GetComponent<Health> ().healthPoint -= ObstacleCollisionDamage;
			}
		}
	}

}