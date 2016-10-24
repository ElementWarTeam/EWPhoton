﻿using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class StoneElement :  Photon.PunBehaviour, IPunObservable
	{

		private PlayerInfo playerInfo;

		private static string chargePrefabName = Constant.StoneChargePrefabName;

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

		private GameObject generateBullet (Vector2 position)
		{
			GameObject bulletObj = PhotonNetwork.Instantiate (chargePrefabName, position, Quaternion.identity, 0);
			return bulletObj;
		}

		public void charge (Vector2 position, float angle, Vector2 direction)
		{
			GameObject bulletObj = generateBullet (position);

			// Setup fire ball damange/owner
			StoneCharge charge = bulletObj.GetComponent <StoneCharge> ();
			charge.damage = playerInfo.bulletDamage;
			charge.setOwner (this.playerInfo);

			// Setup physic body
			Rigidbody2D body = bulletObj.GetComponent <Rigidbody2D> (); // physical body
			body.rotation = angle;
			body.AddForce (direction * Constant.FireBallSpeed);
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