//using UnityEngine;
//using System.Collections;
//using UnityEngine.Networking;
//
//public class ElementNetworkManager : NetworkManager
//{
//
//	public GameObject playerPrefab;
//
//	public ElementNetworkManager ()
//	{
//		autoCreatePlayer = true;
//	}
//
//	public override void OnServerConnect (NetworkConnection conn)
//	{
//		Debug.Log ("OnPlayerConnected");
//	}
//
//	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
//	{
////		if (extraMessageReader != null) {
////			var s = extraMessageReader.ReadMessage<StringMessage> ();
////			Debug.Log ("my name is " + s.value);
////		}
////		OnServerAddPlayer (conn, playerControllerId, extraMessageReader);
//	}
//}
