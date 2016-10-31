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

		public void takeDamage (float damage)
		{
			this.health -= damage * (1f - defense);
		}

		public void takeContinousDamage (float continousDamage)
		{
			// TODO
		}

		public void addHealth (float health)
		{
			this.health += health;
			if (this.health > initialHealth) {
				this.health = initialHealth;
			}
		}

		public void addScore (float score)
		{
			this.score += score;
		}

		public void changeSpeed (float speedDelta)
		{
			this.speed += speedDelta;
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