using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;



namespace Com.EW.MyGame {


	public class AssignPlayerType : MonoBehaviour {

		public Button btn;
		public bool isSelectedNow;
		private string[] types = {"Fire", "Ice", "Electric", "Rancher"};



		// Use this for initialization
		void Start () {
			btn = this.GetComponent<Button> ();
		}
		
	
		public void setLocalPlayerType () {

			// current player type
			string btnText = btn.GetComponentInChildren<Text>().text;

			isSelectedNow = true;

			Debug.Log ("text in button:" + btnText);

			// assign player type
			string playerType = btnText + "Element";
			PlayerManager.LocalPlayerType = playerType;

			Debug.Log ("Player Type now: " + playerType);

			// set color
			setColor(btnText);

			// press sound
			AudioSource pressBtnSound = btn.GetComponent<AudioSource>();
			pressBtnSound.Play ();
		}


		public void setColor(string myType) {
			int idx = Array.IndexOf (types, myType);
			if (idx == -1)
				return;
			
			// set current white
			btn.GetComponentInChildren<Text> ().color = Color.white;

			// set others black
			for (int i = 0; i < types.Length; i++) {
				if (i != idx) {
					GameObject obj = GameObject.Find ("Select " + types [i]);
					obj.GetComponent<AssignPlayerType> ().isSelectedNow = false;
					obj.GetComponentInChildren<Text> ().color = Color.black;
				}
			}
		}



	}


}// end of namespace
