﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Com.EW.MyGame
{
	public class MyScoreUI : MonoBehaviour
	{

		#region Public Properties

		[Tooltip ("Ult button to show percentage of ult energy")]
		public Button UltButton;

		public Image innerImage;

		#endregion


		#region Private Properties

		PlayerManager _target;
		PlayerInfo playerInfo;


		#endregion


		#region MonoBehaviour Messages

		void Update ()
		{
			if (playerInfo == null)
				return;

			// Update Ult Energy
			if (playerInfo.energy >= 100f) {
				playerInfo.energy = 100f; // TODO: @Cairu add to constant
				UltButton.GetComponentInChildren<Text> ().text = "Ult";
				playerInfo.isUltReady = true;
			}

			if (playerInfo.energy != 100f) {
				playerInfo.isUltReady = false;
				playerInfo.energy += 0.18f; // TODO: @Cairu add to constant
				float energy = playerInfo.energy;
				UltButton.GetComponentInChildren<Text> ().text = energy.ToString ("0") + "%";
				if (energy < 50) {
					UltButton.GetComponentInChildren<Text> ().color = Color.black;
				} else {
					UltButton.GetComponentInChildren<Text> ().color = Color.white;
				}
			}
				
			// Update Ult Percentage
			innerImage.fillAmount = playerInfo.energy / 100f;

		
			// Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
			if (_target == null) {
				Destroy (this.gameObject);
				return;
			}
		}

		#endregion


		// first called

		#region Public Methods

		public void SetTarget (PlayerManager target)
		{
//			Debug.LogWarning("<Color=Red><a>Testing</a></Color>SetTarget Method is called!!!!!!!");

			if (target == null) {
				Debug.LogError ("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget. At this time, target is null!!", this);
				return;
			}
			// Cache references for efficiency
			_target = target;
			playerInfo = _target.GetComponent<PlayerInfo> ();
			Debug.Log ("init energy: " + playerInfo.energy);
			Debug.Log ("init health: " + playerInfo.health);

//			Debug.Log ("At this time, _target has been set");
		}

		void LateUpdate ()
		{

			// #Critical
			// Follow the Target GameObject on screen.
			// The energy bar is still, so we do not need to add code here
		}

		#endregion


	}
}
