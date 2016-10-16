using UnityEngine;
using UnityEngine.UI;


using System.Collections;


namespace Com.EW.MyGame
{
	public class MyScoreUI : MonoBehaviour {


		#region Public Properties

		[Tooltip("UI Slider to display Player's Score")]
		public Slider PlayerScoreSlider;

	
		[Tooltip("UI Slider to display Player's Energy")]
		public Slider PlayerEnergySlider;

		#endregion


		#region Private Properties
		PlayerManager _target;


		#endregion


		#region MonoBehaviour Messages
		void Awake(){
			// A player UI must be represented in a Canvas
			this.GetComponent<Transform>().SetParent (GameObject.Find("Canvas").GetComponent<Transform>());

		}

		void Update()
		{
			// 这里，根据——target的情况来update我们bar的值
			// Reflect the Player Health
//			if (PlayerHealthSlider != null) {
//				PlayerHealthSlider.value = _target.Health;
//			}

			// Update Score Earned
			if (PlayerScoreSlider != null) {
				PlayerScoreSlider.value += 0.0001f;
			}

			// update Ult Energy
			if (PlayerEnergySlider != null) {
				PlayerEnergySlider.value += 0.0002f;
			}

			// Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
			if (_target == null) {
				Destroy(this.gameObject);
				return;
			}
		}

		#endregion


		// first called
		#region Public Methods
		public void SetTarget(PlayerManager target){
			Debug.LogWarning("<Color=Red><a>Testing</a></Color>SetTarget Method is called!!!!!!!");

			if (target == null) {
				Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget. At this time, target is null!!",this);
				return;
			}
			// Cache references for efficiency
			_target = target;

			Debug.Log ("At this time, _target has been set");
		}

		void LateUpdate () {

			// #Critical
			// Follow the Target GameObject on screen.
			// The energy bar is still, so we do not need to add code here
		}

		#endregion


	}
}
