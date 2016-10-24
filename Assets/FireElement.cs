using UnityEngine;
using System.Collections;


namespace Com.EW.MyGame
{
	public class FireElement : MonoBehaviour
	{

		private PlayerInfo playerInfo;

		private static string fireBallPrefabName = Constant.FireBallPrefabName;

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
			fireball.setOwner (this.playerInfo);

			// Setup physic body
			Rigidbody2D body = fireBallObj.GetComponent <Rigidbody2D> (); // physical body
			body.rotation = angle;
			body.AddForce (direction * Constant.FireBallSpeed);
		}

		public void useUltra (Vector2 position)
		{
			for (float angle = 0; angle < 360; angle += 30) {
				float radians = angle * Mathf.Deg2Rad;
				Vector2 direction = new Vector2 (Mathf.Sin (radians), Mathf.Cos (radians));
				fire (position, angle, direction);
			}
		}
	}
}