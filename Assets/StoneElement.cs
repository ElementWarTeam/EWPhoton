using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class StoneElement :  Photon.PunBehaviour, IPunObservable
	{

		private PlayerInfo playerInfo;

		private static string chargePrefabName = Constant.StoneChargePrefabName;

		private float nextDecreaseVelocityTime = 0.0f;
		private float stopTime = 0.0f;

		void Start ()
		{
			playerInfo = this.GetComponent<PlayerInfo> ();
			playerInfo.setup (
				Constant.DarkElementInitialNeedleDamage,
				Constant.DarkElementInitialSpeed, 
				Constant.DarkElementInitialHealth, 
				Constant.DarkElementInitialDefensePercentage, 
				Constant.DarkElementInitialFireRate, 
				Constant.DarkElementInitialEnergy, 
				Constant.DarkElementInitialEnergyRecoverRatePercentage);
		}

		void Update ()
		{
			Rigidbody2D rb2d = GetComponent<Rigidbody2D> ();
			if (stopTime < Time.time) {
				rb2d.velocity = Vector2.zero;
			}
		}

		private GameObject generateBullet (Vector2 position)
		{
			GameObject bulletObj = PhotonNetwork.Instantiate (chargePrefabName, position, Quaternion.identity, 0);
			return bulletObj;
		}

		public void showShadow (Vector2 position, float angle, Vector2 direction)
		{
//			GameObject chargeShadow = generateBullet (position);
//			chargeShadow.GetComponent<Rigidbody2D> ().rotation = angle;
		}

		public void charge (Vector2 position, Vector2 direction)
		{
			Rigidbody2D rb2d = GetComponent<Rigidbody2D> ();
			Debug.Log ("Charge called: " + direction);
			Vector2 moveToPos = rb2d.position + direction.normalized * 3f;
//			rb2d.AddForce (direction * 3);
			rb2d.velocity = direction * 10;
			stopTime = Time.time + 0.5f;
		}

		public void useUltra (Vector2 position)
		{
//			for (float angle = 0; angle < 360; angle += 30) {
//				float radians = angle * Mathf.Deg2Rad;
//				Vector2 direction = new Vector2 (Mathf.Sin (radians), Mathf.Cos (radians));
//				fire (position, angle, direction);
//			}
		}

		#region IPunObservable implementation

		void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting) {
				// Nothing to write
			} else {
				// Nothing to receive
			}
		}

		#endregion
	}
}