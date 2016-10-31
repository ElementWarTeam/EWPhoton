using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

namespace Com.EW.MyGame
{
	public class DebugController : MonoBehaviour
	{
		private Text text;
		private static DebugController instance;

		// Use this for initialization
		void Start ()
		{
			instance = this;
			text = GetComponent <Text> ();
			text.text = "Debug Information\n";
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}

		public static void displayPlayerInfo (PlayerInfo playerInfo)
		{
			StringBuilder builder = new StringBuilder ();
			builder.AppendLine ("Debug Information");
			builder.AppendLine ("Position: " + playerInfo.transform.position);
			builder.AppendLine ("Score: " + playerInfo.score);
			builder.AppendLine ("Health: " + playerInfo.health + "/" + playerInfo.initialHealth);
			builder.AppendLine ("Energy: " + playerInfo.energy + "/" + playerInfo.initialEnergy);
			builder.AppendLine ("Speed: " + playerInfo.speed);
			builder.AppendLine ("Defense: " + playerInfo.defense);
			builder.AppendLine ("Fire Rate: " + playerInfo.fireRate);
			instance.text.text = builder.ToString ();
		}
	}
}