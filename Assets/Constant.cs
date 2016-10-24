using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public class Constant : MonoBehaviour
	{
		// Types
		public const string FireElementType = "FireElement";
		public const string IceElementType = "IceElement";
		public const string ElectricElementType = "ElectricElement";
		public const string RancherElementType = "RancherElement";

		// Prefab names
		public static string FireBallPrefabName = "FireBall";
		public static string ElectricArcPrefabName = "ElectricArc";
		public static string IceCrystalPrefabName = "IceCrystal";
		public static string StoneChargePrefabName = "StoneCharge";
		public static string RancherSwordPrefabName = "RancherSword";
		public static string ElectricFieldPrefabName = "ElectricField";

		// Fire element player information
		public static float FireElememtInitialHealth = 100f;
		public static float FireElememtInitialFireballDamage = 5f;
		public static float FireElementInitialSpeed = 100f;
		public static float FireElementInitialDefensePercentage = 0.1f;
		public static float FireElementInitialFireRate = 1f;
		public static float FireElementInitialEnergy = 100f;
		public static float FireElementInitialEnergyRecoverRatePercentage = 0.05f;

		public static float FireBallSpeed = 150f;

		// Obstacle information
		public static float ObstacleCollisionDamage = 10f;
	}
}