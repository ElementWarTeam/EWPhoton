using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class PlayerInfo : Photon.PunBehaviour, IPunObservable
	{
		public string playerId;
		public string type;

		public float score;

		public float bulletDamage;
		public float speed;
		public float health;
		public float defense;
		public float fireRate;
		public float energy;
		public float energyRecoverRate;
		public float initialHealth;
		public float initialEnergy;
		public bool isUltReady;

		private float continousNextDamageTime;
		private float continousDamageEndTime;
		private float continousNextSpeedDamageEndTime;
		private float continousSpeedDamageTime;

		// about player stats
		public float SpawnTime = 0f;
		public float DeadTime = 0f;
		public float survivalTime = 0f;
		public float eliminations = 0f;
		public float damageTaken = 0f;
		public float healingDone =0f;

		public void setup (float bulletDamage, float speed, float initialHealth, float defense, float fireRate, float initialEnergy, float energyRecoverRate)
		{
			this.bulletDamage = bulletDamage;
			this.speed = speed;
			this.health = initialHealth;
			this.initialHealth = initialHealth;
			this.defense = defense;
			this.fireRate = fireRate;
			this.energy = initialEnergy;
			this.initialEnergy = initialEnergy;
			this.energyRecoverRate = energyRecoverRate;
		}

		void Update ()
		{
			// TODO: @Cairu
//			if (playerBeHitted != null && (hitTime + Constant.BasicEffectTime <= Time.time)) {
//				playerBeHitted.speed -= continousIceCrystalSpeedDamage;
//			}
		}

		public void takeDamage (float damage)
		{
			this.damageTaken += damage;
			this.health -= damage * (1f - defense);
		}

		public void takeContinousDamage (float continousDamage)
		{
			// TODO
		}
			

		public void addHealth (float health)
		{
			this.healingDone += health;

			this.health += health;
			if (this.health > initialHealth) {
				this.health = initialHealth;
			}
		}

		public void addScore (float score)
		{
			this.score += score;
		}

		public void takeContiousSpeedDamage (float speedDelta, float time)
		{
			this.speed += speedDelta;
			continousNextSpeedDamageEndTime = Time.time + 1f; // TODO: @Cairu: every 1 second
			continousNextSpeedDamageEndTime = Time.time + time; // TODO
		}

		public void changeDenfense (float defenseDelta)
		{
			this.defense += defenseDelta;
		}

		#region IPunObservable implementation

		void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting) {
				// We own this player: send the others our data
				stream.SendNext (score);
				stream.SendNext (bulletDamage);
				stream.SendNext (speed);
				stream.SendNext (initialHealth);
				stream.SendNext (initialHealth);
				stream.SendNext (defense);
				stream.SendNext (fireRate);
				stream.SendNext (initialEnergy);
				stream.SendNext (energyRecoverRate);
				stream.SendNext (isUltReady);
			} else {
				// Network player, receive data
				this.score = (float)stream.ReceiveNext ();
				this.bulletDamage = (float)stream.ReceiveNext ();
				this.speed = (float)stream.ReceiveNext ();
				this.initialHealth = (float)stream.ReceiveNext ();
				this.initialHealth = (float)stream.ReceiveNext ();
				this.defense = (float)stream.ReceiveNext ();
				this.fireRate = (float)stream.ReceiveNext ();
				this.initialEnergy = (float)stream.ReceiveNext ();
				this.energyRecoverRate = (float)stream.ReceiveNext ();
				this.isUltReady = (bool)stream.ReceiveNext ();
			}
		}

		#endregion
	}
}