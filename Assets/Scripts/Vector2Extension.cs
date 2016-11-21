using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public static class Vector2Extension
	{

		public static Vector2 Rotate (this Vector2 v, float degrees)
		{
			float radians = degrees * Mathf.Deg2Rad;
			float sin = Mathf.Sin (radians);
			float cos = Mathf.Cos (radians);

			float tx = v.x;
			float ty = v.y;

			return new Vector2 (cos * tx - sin * ty, sin * tx + cos * ty);
		}

		public static Vector2 RandomPosition ()
		{
			Vector2 position = new Vector2 (0.0f, 0.0f);
			position [0] = UnityEngine.Random.Range (-Constant.RANGE_X, Constant.RANGE_X);
			position [1] = UnityEngine.Random.Range (-Constant.RANGE_Y, Constant.RANGE_Y);
			return position;
		}
	}
}