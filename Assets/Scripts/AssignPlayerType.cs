using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;



namespace Com.EW.MyGame
{


	public class AssignPlayerType : MonoBehaviour
	{

		public Button btn;
		private string[] types = { "Fire", "Ice", "Electric", "Rancher" };


		// Use this for initialization
		void Start ()
		{
			btn = this.GetComponent<Button> ();
		}

	
		public void setLocalPlayerType ()
		{

//			GameObject go = GameObject.Find ("Select Fire");
//			string str = go.GetComponentInChildren<Text> ().text;
//			Debug.Log ("here it is!:::" + str);

			// current player type
			string btnText = btn.GetComponentInChildren<Text> ().text;

			Debug.Log ("text in button:" + btnText);

			// assign player type
			string playerType = btnText + "Element"; // TODO change this to Constant
			PlayerManager.LocalPlayerType = playerType;

			Debug.Log ("Player Type now: " + playerType);

			// set color
			setColor (btnText);

			// button press sound
			AudioSource btnPressSound = btn.GetComponent<AudioSource>();
			btnPressSound.Play ();
		}


		public void setColor (string myType)
		{
			int idx = Array.IndexOf (types, myType);
			if (idx == -1)
				return;
			// set current white
			btn.GetComponentInChildren<Text> ().color = Color.white;
			// set others black
			for (int i = 0; i < types.Length; i++) {
				if (i != idx) {
					GameObject obj = GameObject.Find ("Select " + types [i]);
					obj.GetComponentInChildren<Text> ().color = Color.black;
				}
			}
		}



	}


}
// end of namespace
