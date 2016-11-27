using UnityEngine;
using System.Collections;

namespace Com.EW.MyGame
{
	public abstract class IElement: Photon.PunBehaviour
	{
		public abstract void fire (Vector2 position, float angle, Vector2 direction);

		public abstract void useUltra (Vector2 position);
	}

}