using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class StoneElement : IElement, IPunObservable
	{

		private PlayerInfo playerInfo;

		private static string chargePrefabName = Constant.StoneChargePrefabName;

		private bool isAccumulating = false;
		private bool isCharging = false;
		private bool isUsingUltra = false;
		private float ultraEndTime = 0f;
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
			if (photonView.isMine == false && PhotonNetwork.connected == true) {
				return;
			}
			if (isCharging) {
				playerInfo.defense *= 3; // TODO
				if (chargeInitiateTime + chargingTime < Time.time) {
					GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
					isCharging = false;
				}
				playerInfo.defense /= 3; // TODO
			}
			if (isUsingUltra && ultraEndTime < Time.time) {
				isUsingUltra = false;
				playerInfo.immune = false;
				transform.localScale /= 2;
			}
		}

		void OnTriggerEnter2D (Collider2D obj)
		{
			Debug.Log ("PlayerManager: OnTriggerEnter2D");
			if (!photonView.isMine) {
				return;
			}

			PhotonView pv = obj.transform.GetComponent<PhotonView> ();
			Debug.Log ("Stone: " + photonView.owner.name + "'s fireball hits " + pv.name);

			if (obj.CompareTag ("Element")) {
				if (pv.owner.name.Equals (photonView.owner.name)) {
					return;
				} 
				if (photonView.isMine == true && PhotonNetwork.connected == true) {
					pv.RPC ("TakeDamage", PhotonTargets.All, playerInfo.bulletDamage);
					this.photonView.RPC ("AddScore", PhotonTargets.All, playerInfo.bulletDamage);
				}
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

		public override void fire (Vector2 position, float angle, Vector2 direction)
		{
			
		}

		public override void useUltra (Vector2 position)
		{
			isUsingUltra = true;
			ultraEndTime = Time.time + 10f; // TODO
			playerInfo.immune = true;
			transform.localScale *= 2;
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