using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Com.EW.MyGame
{
	public class IndicatorPanel : MonoBehaviour
	{

		public ElementIndicator indicatorPrefab;

		List<ElementIndicator> indicatorPool = new List<ElementIndicator> ();
		int indicatorCursor = 0;
		float nextCleanTime = 0f;

		void LateUpdate ()
		{
			paint ();
		}

		void paint ()
		{
			resetPool ();
			setIndicatorForElements ();
			switch (PlayerManager.LocalPlayerType) { // set at GameManager.cs: Start()
			case Constant.FireElementType:
				setIndicatorForPowerUp ();
				break;
			case Constant.ElectricElementType:
				setIndicatorForSpeedUp ();
				break;
			case Constant.IceElementType:
				setIndicatorForHealthPack ();
				break;
			case Constant.DarkElementType:
				setIndicatorForBlackHole ();
				break;
			case Constant.StoneElementType:
				setIndicatorForObstacle ();
				break;
			default:
				break;
			}
			if (nextCleanTime < Time.time) {
				nextCleanTime = Time.time + 1f;
				cleanPool ();
			}
		}

		private void setIndicatorForElements ()
		{
			IElement[] objects = GameObject.FindObjectsOfType (typeof(IElement)) as IElement[];
			foreach (IElement obj in objects) {
				if (obj != PlayerManager.LocalPlayerInstance) {
					Vector3 screenPos = Camera.main.WorldToScreenPoint (obj.transform.position);
					setIndicatorForObj (screenPos, Color.magenta);
				}
			}
		}

		private void setIndicatorForObstacle ()
		{
			ObstacleScript[] objects = GameObject.FindObjectsOfType (typeof(ObstacleScript)) as ObstacleScript[];
			foreach (ObstacleScript obj in objects) {
				if (obj != PlayerManager.LocalPlayerInstance) {
					Vector3 screenPos = Camera.main.WorldToScreenPoint (obj.transform.position);
					setIndicatorForObj (screenPos, Color.gray);
				}
			}
		}

		private void setIndicatorForHealthPack ()
		{
			HealthPack[] objects = GameObject.FindObjectsOfType (typeof(HealthPack)) as HealthPack[];
			foreach (HealthPack obj in objects) {
				if (obj != PlayerManager.LocalPlayerInstance) {
					Vector3 screenPos = Camera.main.WorldToScreenPoint (obj.transform.position);
					setIndicatorForObj (screenPos, Color.green);
				}
			}
		}

		private void setIndicatorForSpeedUp ()
		{
			SpeedUpPickUp[] objects = GameObject.FindObjectsOfType (typeof(SpeedUpPickUp)) as SpeedUpPickUp[];
			foreach (SpeedUpPickUp obj in objects) {
				if (obj != PlayerManager.LocalPlayerInstance) {
					Vector3 screenPos = Camera.main.WorldToScreenPoint (obj.transform.position);
					setIndicatorForObj (screenPos, new Color (1F, 0.5F, 0.2F));
				}
			}
		}

		private void setIndicatorForPowerUp ()
		{
			PowerUpPickUp[] objects = GameObject.FindObjectsOfType (typeof(PowerUpPickUp)) as PowerUpPickUp[];
			foreach (PowerUpPickUp obj in objects) {
				if (obj != PlayerManager.LocalPlayerInstance) {
					Vector3 screenPos = Camera.main.WorldToScreenPoint (obj.transform.position);
					setIndicatorForObj (screenPos, Color.red);
				}
			}
		}

		private void setIndicatorForBlackHole ()
		{
			BlackHole[] objects = GameObject.FindObjectsOfType (typeof(BlackHole)) as BlackHole[];
			foreach (BlackHole obj in objects) {
				if (obj != PlayerManager.LocalPlayerInstance) {
					Vector3 screenPos = Camera.main.WorldToScreenPoint (obj.transform.position);
					setIndicatorForObj (screenPos, Color.black);
				}
			}
		}

		private void setIndicatorForObj (Vector3 screenPos, Color color)
		{
			if (screenPos.x > 0 && screenPos.x < Screen.width
			    && screenPos.y > 0 && screenPos.y < Screen.height) {
				// NO indicator
			} else { // OFFSCREEN
				Vector3 screenCenter = new Vector3 (Screen.width, Screen.height, 0) / 2;
				// Coordinate translated to screen center
				screenPos -= screenCenter;
				float angle = Mathf.Atan2 (screenPos.y, screenPos.x);
				angle -= 90 * Mathf.Deg2Rad;
				Vector3 screenBound = screenCenter * 0.9f;
				if (screenPos.x == 0) {
					if (screenPos.y > 0) {
						screenPos = new Vector3 (0, screenBound.y, 0);
					} else {
						screenPos = new Vector3 (0, -screenBound.y, 0);
					}
				} else {
					float k = screenPos.y / screenPos.x;
					float screenK = (float)Screen.height / Screen.width;
					if (k > 0) {
						if (screenPos.x > 0) {
							if (k > screenK) { // TOP: right half
								screenPos = new Vector3 (screenBound.y / k, screenBound.y, 0);
							} else { // RIGHT: top half
								screenPos = new Vector3 (screenBound.x, screenBound.x * k, 0);
							}
						} else {
							if (k > screenK) { // BOTTOM: left half
								screenPos = new Vector3 (-screenBound.y / k, -screenBound.y, 0);
							} else { // LEFT: bottom half
								screenPos = new Vector3 (-screenBound.x, -screenBound.x * k, 0);
							}
						}
					} else {
						if (screenPos.x > 0) {
							if (k > -screenK) { // RIGHT: bottom half
								screenPos = new Vector3 (screenBound.x, screenBound.x * k, 0);
							} else { // BOTTOM: right half
								screenPos = new Vector3 (-screenBound.y / k, -screenBound.y, 0);
							}
						} else {
							if (k > -screenK) { // LEFT: top half
								screenPos = new Vector3 (-screenBound.x, -screenBound.x * k, 0);
							} else { // TOP: left half
								screenPos = new Vector3 (screenBound.y / k, screenBound.y, 0);
							}
						}
					}
				}
				// Coordinate translated to bottom left
				screenPos += screenCenter;
				ElementIndicator indicator = GetIndicator ();
				indicator.transform.position = screenPos;
				indicator.transform.localRotation = Quaternion.Euler (0, 0, angle * Mathf.Rad2Deg);
				indicator.GetComponent <Image> ().color = color;
			}
		}

		ElementIndicator GetIndicator ()
		{
			ElementIndicator output;
			if (indicatorCursor < indicatorPool.Count) {
				output = indicatorPool [indicatorCursor];
			} else {
				output = Instantiate (indicatorPrefab) as ElementIndicator;
				output.transform.SetParent (transform);
				indicatorPool.Add (output);
			}
			indicatorCursor++;
			return output;
		}

		void resetPool ()
		{
			indicatorCursor = 0;
		}

		void cleanPool ()
		{
			while (indicatorPool.Count > indicatorCursor) {
				ElementIndicator obj = indicatorPool [indicatorPool.Count - 1];
				indicatorPool.RemoveAt (indicatorPool.Count - 1);
				Destroy (obj.gameObject);
			}
		}
	}

}