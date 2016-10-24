using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Com.EW.MyGame{
	
	public class MyUIReactController : MonoBehaviour {

	//	public Button playButton;
		private Color highlightedColor = Color.green;
		private Color defaultColor = Color.black;
//		private Color highlightedColorText = Color.white;
//		private Color defaultColorText = Color.black;

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
		
		}

	//	public void MouseOnButtonText(Button btn) {
	//		if (btn.GetComponent<AssignPlayerType>().isSelectedNow == false) {
	//			btn.GetComponentInChildren<Text> ().color = highlightedColorText;
	//		}
	//	}
	//
	//	public void MouseLeaveButtonText(Button btn) {
	//		if (btn.GetComponent<AssignPlayerType>().isSelectedNow == false) {
	//			btn.GetComponentInChildren<Text> ().color = defaultColorText;
	//		}
	//	}


		public void MouseOnButton(Button btn) {
			btn.image.color = highlightedColor;
		}

		public void MouseLeaveButton(Button btn) {
			btn.image.color = defaultColor;
		}


//		public void PlayPressButtonSound(Button btn) {
//			Debug.Log ("Boss in play button sound function");
//			AudioSource audioSource = btn.GetComponent<AudioSource> ();
//			audioSource.Play ();
//		}
	}

} // end of namespace
