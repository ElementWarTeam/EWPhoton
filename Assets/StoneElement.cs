using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class StoneElement :  Photon.PunBehaviour, IPunObservable
	{

		private PlayerInfo playerInfo;

		private static string chargePrefabName = Constant.StoneChargePrefabName;

		private bool isAccumulating = false;
		private bool isCharging = false;
		private float chargeInitiateTime = 0f;
		private float chargingTime = 0f;
		private float accumulatingInitiateTime = 0f;
		private Vector2 releasePressDirection = new Vector2 (0f, 0f);
		private const float ACCUMULATING_TIME_MAX = 3f;
		private const float ACCUMULATING_TIME_MIN = 1f;

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
			if (isCharging) {
				playerInfo.defense *= 3; // TODO
				if (chargeInitiateTime + chargingTime < Time.time) {
					GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
					isCharging = false;
				}
				playerInfo.defense /= 3; // TODO
			}
		}

		private GameObject generateBullet (Vector2 position)
		{
			GameObject bulletObj = PhotonNetwork.Instantiate (chargePrefabName, position, Quaternion.identity, 0);
			return bulletObj;
		}

		public void startCharge (Vector2 position, float angle, Vector2 direction)
		{
			if (isCharging)
				return;
			if (isAccumulating) {
				if (direction.magnitude > 0) {
					releasePressDirection [0] = direction [0];
					releasePressDirection [1] = direction [1];
				}

			} else {
				accumulatingInitiateTime = Time.time;
				isAccumulating = true;
			}
		}

		public void charge (Vector2 position)
		{
			if (isAccumulating) {
				Rigidbody2D rb2d = GetComponent<Rigidbody2D> ();
				Debug.Log ("Charge called: " + releasePressDirection);

				float accumulatedTime = Time.time - accumulatingInitiateTime;
				if (accumulatedTime > ACCUMULATING_TIME_MAX)
					accumulatedTime = ACCUMULATING_TIME_MAX;
				chargingTime = 0f;
				if (accumulatedTime >= ACCUMULATING_TIME_MIN) {
					// ACCUMULATING_TIME_MIN -> 0.2
					// ACCUMULATING_TIME_MAX -> 0.8
					// TODO: @Cairu: coordinate these data with Constant.cs
					chargingTime = 0.2f + 0.6f * (accumulatedTime - ACCUMULATING_TIME_MIN) / (ACCUMULATING_TIME_MAX - ACCUMULATING_TIME_MIN);
					rb2d.velocity = releasePressDirection * 15;
					chargeInitiateTime = Time.time;
					isCharging = true;
				}
				isAccumulating = false;
			}
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