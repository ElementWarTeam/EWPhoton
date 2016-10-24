using UnityEngine;
using UnityEngine.UI;


using System.Collections;


namespace Com.EW.MyGame
{
	public class MyPlayerUI : MonoBehaviour
	{


		#region Public Properties

		[Tooltip ("UI Text to display Player's Name")]
		public Text PlayerNameText;

		[Tooltip ("UI Slider to display Player's Health")]
		public Slider PlayerHealthSlider;

		[Tooltip ("UI Slider to display Player's Score")]
		public Slider PlayerScoreSlider;

		[Tooltip ("Pixel offset from the player target")]
		public Vector3 ScreenOffset = new Vector3 (0f, 30f, 0f);

		#endregion


		#region Private Properties

		PlayerManager _target;
		Transform _targetTransform;
		Vector3 _targetPosition;

		#endregion


		#region MonoBehaviour Messages

		void Awake ()
		{
			// A player UI must be represented in a Canvas
			this.GetComponent<Transform> ().SetParent (GameObject.Find ("Canvas").GetComponent<Transform> ());

		}

		void Update ()
		{
			// Reflect the Player Health
			if (PlayerHealthSlider != null && _target != null) {
				PlayerHealthSlider.value = _target.getHealthPercentage ();
			}

			// Update Score Earned
			if (PlayerScoreSlider != null) {
				PlayerScoreSlider.value = 50;
			}

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
			Debug.LogWarning ("<Color=Red><a>Testing</a></Color>SetTarget Method is called!!!!!!!");

			if (target == null) {
				Debug.LogError ("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget. At this time, target is null!!", this);
				return;
			}
			// Cache references for efficiency
			_target = target;
			_targetTransform = _target.transform;

			Debug.Log ("At this time, _target has been set");

			if (PlayerNameText != null) {
				PlayerNameText.text = _target.photonView.owner.name;
			}

		}

		void LateUpdate ()
		{

			// #Critical
			// Follow the Target GameObject on screen.
			if (_targetTransform != null) {
				_targetPosition = _targetTransform.position;
				this.transform.position = Camera.main.WorldToScreenPoint (_targetPosition) + ScreenOffset;
			}
		}

		#endregion


	}
}