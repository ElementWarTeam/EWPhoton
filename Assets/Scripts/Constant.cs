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
		public const string DarkElementType = "DarkElement";
		public const string StoneElementType = "StoneElement";

		// Prefab names
		public const string FireBallPrefabName = "FireBall";
		public const string ElectricArcPrefabName = "ElectricArc";
		public const string IceCrystalPrefabName = "IceCrystal";
		public const string StoneChargePrefabName = "StoneCharge";
		public const string DarkNeedlePrefabName = "DarkNeedle";
		public const string ElectricFieldPrefabName = "ElectricField";

		// Fire element player information
		public const float FireElementInitialHealth = 100f;
		public const float FireElementInitialFireballDamage = 8f;
		public const float FireElementInitialSpeed = 100f;
		public const float FireElementInitialDefensePercentage = 0.1f;
		public const float FireElementInitialFireRate = 1f;
		public const float FireElementInitialEnergy = 0f;
		public const float FireElementInitialEnergyRecoverRatePercentage = 0.05f;

		// Electric element player information
		public const float ElectricElementInitialHealth = 80f;
		public const float ElectricElementInitialElectricArcDamage = 5f;
		public const float ElectricElementInitialSpeed = 120f;
		public const float ElectricElementInitialDefensePercentage = 0.1f;
		public const float ElectricElementInitialFireRate = 1f;
		public const float ElectricElementInitialEnergy = 0f;
		public const float ElectricElementInitialEnergyRecoverRatePercentage = 0.05f;
		public const float ElectricFieldContinousDamage = 10f;

		// Ice element player information
		public const float IceElementInitialHealth = 80f;
		public const float IceElementInitialIceCystalDamage = 5f;
		public const float IceElementInitialSpeed = 120f;
		public const float IceElementInitialDefensePercentage = 0.1f;
		public const float IceElementInitialFireRate = 1f;
		public const float IceElementInitialEnergy = 0f;
		public const float IceElementInitialEnergyRecoverRatePercentage = 0.05f;

		// Dark element player information
		public const float DarkElementInitialHealth = 80f;
		public const float DarkElementInitialNeedleDamage = 5f;
		public const float DarkElementInitialSpeed = 120f;
		public const float DarkElementInitialDefensePercentage = 0.1f;
		public const float DarkElementInitialFireRate = 1f;
		public const float DarkElementInitialEnergy = 0f;
		public const float DarkElementInitialEnergyRecoverRatePercentage = 0.05f;

		// Stone element player information
		public const float StoneElementInitialHealth = 80f;
		public const float StoneElementInitialNeedleDamage = 5f;
		public const float StoneElementInitialSpeed = 120f;
		public const float StoneElementInitialDefensePercentage = 0.1f;
		public const float StoneElementInitialFireRate = 0.5f;
		public const float StoneElementInitialEnergy = 0f;
		public const float StoneElementInitialEnergyRecoverRatePercentage = 0.05f;

		// Bullet
		public const float FireBallSpeed = 150f;
		public const float IceCystalSpeed = 150f;
		public const float LiveTime = 5;
		public const float ElectricFieldLiveTime = 10;

		// Obstacle information
		public const float ObstacleCollisionDamage = 10f;
		public const float ObstacleGenerateInterval = 2f;

		// PICKUPS
		public const float HealthPackRecover = 20f;
		public const float HealthPackGenerateInterval = 5f;
		public const float HealthPackBoundary_x = 10.0f;
		public const float HealthPackBoundary_y = 5.0f;
		public const float PickUpInitTime = 1.0f;
	}
}