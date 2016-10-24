using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class IceElement : Photon.PunBehaviour, IPunObservable
	{

		private PlayerInfo playerInfo;

		private static string bulletPrefabName = Constant.IceCrystalPrefabName;

		void Start ()
		{
			playerInfo = this.GetComponent<PlayerInfo> ();
			playerInfo.setup (
				Constant.IceElementInitialIceCystalDamage,
				Constant.IceElementInitialSpeed, 
				Constant.IceElementInitialHealth, 
				Constant.IceElementInitialDefensePercentage, 
				Constant.IceElementInitialFireRate, 
				Constant.IceElementInitialEnergy, 
				Constant.IceElementInitialEnergyRecoverRatePercentage);
		}

		private GameObject generateBullet (Vector2 position)
		{
			GameObject bulletObj = PhotonNetwork.Instantiate (bulletPrefabName, position, Quaternion.identity, 0);
			return bulletObj;
		}

		public void fire (Vector2 position, float angle, Vector2 direction)
		{
			GameObject bulletObj = generateBullet (position);

			// Setup ice crystal damange/owner
			IceCrystal cystal = bulletObj.GetComponent <IceCrystal> ();
			cystal.damage = playerInfo.bulletDamage;
			cystal.setOwner (this.playerInfo);

			// Setup physic body
			Rigidbody2D body = bulletObj.GetComponent <Rigidbody2D> (); // physical body
			body.rotation = angle;
			body.AddForce (direction * Constant.IceCystalSpeed);
		}

		public void useUltra (Vector2 position)
		{
			for (float angle = 0; angle < 360; angle += 30) {
				float radians = angle * Mathf.Deg2Rad;
				Vector2 direction = new Vector2 (Mathf.Sin (radians), Mathf.Cos (radians));
				fire (position, angle, direction);
			}
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