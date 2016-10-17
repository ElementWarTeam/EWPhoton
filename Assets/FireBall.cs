using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class FireBall : Photon.PunBehaviour
	{
		
		public float damage;
		public GameObject owner;

		private float speed = 150f;

		void OnTriggerEnter2D (Collider2D obj)
		{
			if (!photonView.isMine) {
				return;
			}
			if (obj.CompareTag ("Element") && !obj.name.Contains ("Fire")) {
				Debug.Log ("FireBall: " + owner.name + "'s fireball hits " + obj.name);
				obj.GetComponent<Health> ().healthPoint -= damage;
				owner.GetComponent <PlayerInfo> ().score += 10;
			}
		}

		void Update ()
		{
			// while with in time limit
			// decrease the enemy health
		}

		public float getFireBallSpeed ()
		{
			return speed;
		}

	}

}
