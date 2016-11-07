using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Com.EW.MyGame {
	public class InfoController : MonoBehaviour {

		public Slider attackSlider;
		public Slider defendSlider;
		public Slider flexibilitySlider;
		public Text title;
		public Text shortWord;
		public Text ultDescription;

		private float maxAttack = 15f;
		private float maxDefense = 130f;
		private float maxFlexi = 130f;

		private float[] defenseArray = {
			Constant.FireElementInitialHealth, 
			Constant.IceElementInitialHealth,
			Constant.ElectricElementInitialHealth,
			Constant.DarkElementInitialHealth,
			Constant.StoneElementInitialHealth
		};

		private float[] attackArray = {
			Constant.FireElementInitialFireballDamage,
			Constant.IceElementInitialIceCystalDamage,
			Constant.ElectricElementInitialElectricArcDamage,
			Constant.DarkElementInitialNeedleDamage,
			Constant.StoneElementInitialNeedleDamage
		};

		private float[] flexiArray = {
			Constant.FireElementInitialSpeed,
			Constant.IceElementInitialSpeed,
			Constant.ElectricElementInitialSpeed,
			Constant.DarkElementInitialSpeed,
			Constant.StoneElementInitialSpeed
		};

		private string[] titleArray = {
			"Fire Element",
			"Ice Element",
			"Electric Element",
			"Dark Element",
			"Stone Element"
		};

		private string[] shortWordArray = {
			"Ignites everything, wipes out all darkness",
			"Purify the world",
			"Faster and powerful",
			"Fear the darkness",
			"Unbreakable, true persistence"
		};

		private string[] ultDescriptionArray = {
			"The fire element ejects fireballs to all directions around it",
			"The ice element creates a zero temperature zone, frozen enemies within this area",
			"The electric element creates a electromagnetic field, dealing damage each second",
			"The dark element transform to vampire",
			"The stone element becomes invulnerable and charge damage increased"
		};

		private string[] elementsArray = {"Fire", "Ice", "Electric", "Dark", "Stone"};


		// Colors
		private Color[] colorsArray = {
			new Color(255/255f, 85/255f, 5/255f),
			new Color(0/255f, 187/255f, 255/255f),
			new Color(252/255f, 240/255f, 47/255f),
			new Color(137/255f, 88/255f, 176/255f),
			new Color(136/255f, 216/255f, 82/255f)
		};

		//		public const Color32 ElectricColor = new Color32(rSliderValue, bSliderValue, gSliderValue, 1);
		//		public const Color32 DarkColor = new Color32(rSliderValue, bSliderValue, gSliderValue, 1);
		//		public const Color32 StoneColor = new Color32(rSliderValue, bSliderValue, gSliderValue, 1);
	


		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void BackToLauncherScene() {
			SceneManager.LoadScene ("Launcher");
		}

		public void ElementSelected(Button btn) {
			string btnText = btn.GetComponentInChildren<Text> ().text;
			Debug.Log ("btn text is:" + btnText);

			int idx = 0;
			for (int i = 0; i < elementsArray.Length; i++) {
				if (btnText.Equals (elementsArray [i])) {
					idx = i;
					break;
				}
			}
				
			setAttributes (idx);

			setButtonClickEffect (idx);

		}


		public void setButtonClickEffect(int curIdx) {
			for (int i = 0; i < elementsArray.Length; i++) {
				GameObject obj = GameObject.Find (elementsArray [i] + " Btn");
				if (i == curIdx) {
					obj.GetComponentInChildren<Text> ().color = Color.white;
					title.color = colorsArray [curIdx];
				} else {
					obj.GetComponentInChildren<Text> ().color = Color.black;
				}
			}	
		}




		public void setAttributes(int idx) {
			// set attack
			attackSlider.value = attackArray[idx] / maxAttack;

			// set defend
			defendSlider.value = defenseArray[idx] / maxDefense;

			// set flexibility
			flexibilitySlider.value = flexiArray[idx] / maxFlexi;

			// set title
			title.text = titleArray[idx];

			// set short word
			shortWord.text = shortWordArray[idx];

			// set ult description
			ultDescription.text = ultDescriptionArray[idx];
		}


	}
}
