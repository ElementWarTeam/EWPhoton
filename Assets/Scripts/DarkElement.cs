using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class DarkElement : IElement, IPunObservable
	{

		private PlayerInfo playerInfo;
		public static float amplify = 1f;

		private float timeRemaining = 0f;

		private static string bulletPrefabName = Constant.DarkNeedlePrefabName;

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

		public void Update ()
		{
			timeRemaining -= Time.deltaTime;
			if (timeRemaining < 0) {
				this.GetComponent<SpriteRenderer> ().color = new Color32 (152, 66, 217, 255);
			}
		}

		private GameObject generateBullet (Vector2 position)
		{
			GameObject bulletObj = PhotonNetwork.Instantiate (bulletPrefabName, position, Quaternion.identity, 0);
			return bulletObj;
		}

		public override void fire (Vector2 position, float angle, Vector2 direction)
		{
			GameObject bulletObj = generateBullet (position);

			// Setup fire ball damange/owner
			DarkNeedle needle = bulletObj.GetComponent <DarkNeedle> ();
			needle.damage = playerInfo.bulletDamage;
			needle.setOwner (this.playerInfo);

			// Setup physic body
			Rigidbody2D body = bulletObj.GetComponent <Rigidbody2D> (); // physical body
			body.rotation = angle;
			body.AddForce (direction * Constant.FireBallSpeed);
		}

		public override void useUltra (Vector2 position)
		{
			for (float angle = 0; angle < 360; angle += 30) {
				float radians = angle * Mathf.Deg2Rad;
				Vector2 direction = new Vector2 (Mathf.Sin (radians), Mathf.Cos (radians));
				fire (position, angle, direction);
			}
				
			// each evolution, the evil vampire's size grows by 25%
			this.GetComponent<Transform> ().localScale *= 1.35f;
			amplify *= 1.35f;

			// right after evolution, color change to blood red and the damage he dealt to others can heal himself
			this.GetComponent<SpriteRenderer> ().color = new Color32 (255, 0, 0, 240);

			// set vampire time is 4s
			timeRemaining = 4.0f;

			// reset energy to zero
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