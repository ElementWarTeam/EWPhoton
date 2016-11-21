using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class ElectricElement : Photon.PunBehaviour, IPunObservable
	{

		private PlayerInfo playerInfo;

		private static string bulletPrefabName = Constant.ElectricArcPrefabName;

		void Start ()
		{
			playerInfo = this.GetComponent<PlayerInfo> ();
			playerInfo.setup (
				Constant.ElectricElementInitialElectricArcDamage,
				Constant.ElectricElementInitialSpeed, 
				Constant.ElectricElementInitialHealth, 
				Constant.ElectricElementInitialDefensePercentage, 
				Constant.ElectricElementInitialFireRate, 
				Constant.ElectricElementInitialEnergy, 
				Constant.ElectricElementInitialEnergyRecoverRatePercentage);
		}

		private GameObject generateBullet (Vector2 position)
		{
			GameObject bulletObj = PhotonNetwork.Instantiate (bulletPrefabName, position, Quaternion.identity, 0);
			// Setup ice crystal damange/owner
			ElectricArc arc = bulletObj.GetComponent <ElectricArc> ();
			arc.damage = playerInfo.bulletDamage;
			arc.setOwner (this.playerInfo);
			return bulletObj;
		}

		private GameObject generateElectricField (Vector2 position)
		{
			GameObject fieldObj = PhotonNetwork.Instantiate (Constant.ElectricFieldPrefabName, position, Quaternion.identity, 0);
			ElectricField field = fieldObj.GetComponent <ElectricField> ();
			field.continousDamage = Constant.ElectricFieldContinousDamage;
			field.setOwner (this.playerInfo);
			field.setParent (this.gameObject);
			return fieldObj;
		}

		public void fire (Vector2 position, float angle, Vector2 direction)
		{
			if (playerInfo.isUsingPowerUp) {
				fireOneBullet (position, angle, direction);
				fireOneBullet (position, angle + 90f, Vector2Extension.Rotate (direction, 90));
				fireOneBullet (position, angle - 90f, Vector2Extension.Rotate (direction, -90));
				fireOneBullet (position, angle + 180f, Vector2Extension.Rotate (direction, 180));
			} else {
				fireOneBullet (position, angle, direction);
			}
		}

		private void fireOneBullet (Vector2 position, float angle, Vector2 direction)
		{
			GameObject bulletObj = generateBullet (position);

			// Setup physic body
			Rigidbody2D body = bulletObj.GetComponent <Rigidbody2D> (); // physical body
			body.rotation = angle;
			body.AddForce (direction * Constant.IceCystalSpeed);
		}

		public void useUltra (Vector2 position)
		{
			generateElectricField (position);
			playerInfo.energy = 0f;
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