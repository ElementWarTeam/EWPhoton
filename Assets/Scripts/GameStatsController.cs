using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Com.EW.MyGame {
	public class GameStatsController : MonoBehaviour {

		public Text PlayerName;
		public Text SurvivalTime;
		public Text Eleminations;
		public Text Score;
		public Text DamageDone;
		public Text HealingDone;


		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			PlayerName = PlayerManager.
		}
			

		public void BackToMenu() {
			SceneManager.LoadScene ("Launcher");
		}

		public void ExitGame() {
			Application.Quit();
		}
	}
}
