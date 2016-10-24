using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class PlayerInfo : MonoBehaviour
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

		public void setup (float bulletDamage, float speed, float initialHealth, float defense, float fireRate, float initialEnergy, float energyRecoverRate)
		{
			this.bulletDamage = bulletDamage;
			this.speed = speed;
			this.health = initialHealth;
			this.initialHealth = initialHealth;
			this.defense = defense;
			this.fireRate = fireRate;
			this.energy = initialEnergy;
			this.energyRecoverRate = energyRecoverRate;
		}
	}
}