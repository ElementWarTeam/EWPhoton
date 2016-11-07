using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Com.EW.MyGame
{
	public class UltraUI : MonoBehaviour
	{

		#region Public Properties

		[Tooltip ("Ult button to show percentage of ult energy")]
		public Button UltButton;

		public Image innerImage;

		#endregion


		#region Private Properties

		PlayerManager _target;
		PlayerInfo playerInfo;
		private static UltraUI Instance;

		#endregion


		#region MonoBehaviour Messages

		// Use this for initialization
		void Start ()
		{
			Instance = this;
		}

		#endregion

		#region Public Methods

		public static void updateEnergyStatus (PlayerInfo playerInfo)
		{
			float energy = playerInfo.energy;

			Instance.UltButton.GetComponentInChildren<Text> ().text = energy.ToString ("0") + "%";
			if (energy < 50) {
				Instance.UltButton.GetComponentInChildren<Text> ().color = Color.black;
			} else {
				Instance.UltButton.GetComponentInChildren<Text> ().color = Color.white;
			}
			Instance.innerImage.fillAmount = energy / 100f;
		}

		#endregion


	}
}
