using UnityEngine;
using System.Collections;


namespace Com.EW.MyGame
{
	public class FireElement : MonoBehaviour
	{

		private PlayerInfo playerInfo;

		private static string fireBallPrefabName = "FireBall";

		void Start ()
		{
			playerInfo = this.GetComponent<PlayerInfo> ();
		}

		private GameObject generateFireBall (Vector2 position)
		{
//			Debug.Log (playerInfo.playerId + " generate a fireball");
			GameObject fireballObj = PhotonNetwork.Instantiate (fireBallPrefabName, position, Quaternion.identity, 0);
			return fireballObj;
		}

		public void fire (Vector2 position, float angle, Vector2 direction)
		{
			GameObject fireBallObj = generateFireBall (position);

			// Setup fire ball damange/owner
			FireBall fireball = fireBallObj.GetComponent <FireBall> ();
			fireball.damage = playerInfo.bulletDamage;
			fireball.owner = this.gameObject;

			// Setup physic body
			Rigidbody2D body = fireBallObj.GetComponent <Rigidbody2D> (); // physical body
			body.rotation = angle;
			Debug.Log ("Add force direction: " + direction);
			Debug.Log ("Add force speed: " + fireball.getFireBallSpeed ());
			body.AddForce (direction * fireball.getFireBallSpeed ());
		}
	}
}