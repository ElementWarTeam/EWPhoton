using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Com.EW.MyGame
{
	public class GameStatsController : Photon.PunBehaviour
	{

		// presentation on board
		public Text PlayerName;
		public Text SurvivalTime;
		public Text Eliminations;
		public Text Score;
		public Text DamageTaken;
		public Text HealingDone;

		// record everything before target is destoried
		public static string recordPlayerName;
		public static string recordSurvivalTime;
		public static string recordEliminations;
		public static string recordScore;
		public static string recordDamageTaken;
		public static string recordHealingDone;

		void Awake ()
		{
		}

		// Use this for initialization
		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{
			PlayerName.text = recordPlayerName;
			SurvivalTime.text = recordSurvivalTime;
			Eliminations.text = recordEliminations;
			Score.text = recordScore;
			DamageTaken.text = recordDamageTaken;
			HealingDone.text = recordHealingDone;
		}


		public void BackToMenu ()
		{
			PhotonNetwork.LeaveRoom ();
//			SceneManager.LoadScene ("Launcher");
		}

		public void ExitGame ()
		{
//			Application.Quit ();
			PhotonNetwork.LeaveRoom ();
		}

		public override void OnLeftRoom ()
		{
			// return to loggin page
			SceneManager.LoadScene (0);
		}
	}
}
