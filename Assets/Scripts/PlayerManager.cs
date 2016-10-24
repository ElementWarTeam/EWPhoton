#if UNITY_5 && (!UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2 && ! UNITY_5_3) || UNITY_6
#define UNITY_MIN_5_4
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// left joysticker
using UnityStandardAssets.CrossPlatformInput;

// right joysticker
using CnControls;



using System.Collections;

namespace Com.EW.MyGame
{
	/// <summary>
	/// Player manager. 
	/// Handles fire Input and Beams.
	/// </summary>
	public class PlayerManager : Photon.PunBehaviour, IPunObservable
	{
		
		#region Public Variables

		public static GameObject LocalPlayerInstance;
		public static string LocalPlayerType;

		public GameObject PlayerUiPrefab;
		public GameObject PlayerScorePrefab;

		// OOP
		public PlayerInfo playerInfo;
		public bool IsFiring;
		public bool UsingUltra;

		#endregion

		#region Private Variables

		public float timeBetweenShots = 1f;
		private float nextShootTime = 0.0f;

		#endregion

		#region MonoBehaviour CallBacks

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
		/// </summary>
		void Awake ()
		{
			// #Important
			// used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
			if (photonView.isMine) {
				// SETUP PLAYER INFO
				playerInfo = this.GetComponent <PlayerInfo> ();
				playerInfo.playerId = "player name";
				playerInfo.type = PlayerManager.LocalPlayerType;
			}
			// #Critical
			// we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
			DontDestroyOnLoad (this.gameObject);

		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during initialization phase.
		/// </summary>
		void Start ()
		{	// camera work
			CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork> ();

			if (_cameraWork != null) {
				if (photonView.isMine) {
					_cameraWork.OnStartFollowing ();
				}
			} else {
				Debug.LogError ("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
			}


			// player UI
			if (PlayerUiPrefab != null) {
				GameObject _uiGo = Instantiate (PlayerUiPrefab) as GameObject;
				_uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
			} else {
				Debug.LogWarning ("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
			}


			// player Score
			if (PlayerScorePrefab != null) {
				GameObject _uiGo = Instantiate (PlayerScorePrefab) as GameObject;
				_uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
			} else {
				Debug.LogWarning ("<Color=Red><a>Missing</a></Color> PlayerScorePrefab reference on player Prefab.", this);
			}



			// 
			#if UNITY_MIN_5_4
			// Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) => {
				this.CalledOnLevelWasLoaded (scene.buildIndex);
			};
			#endif
		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity on every frame.
		/// </summary>
		void Update ()
		{	
			if (photonView.isMine == false && PhotonNetwork.connected == true) {
				return;
			}

			if (playerInfo.health <= 0f) {
				GameManager.Instance.LeaveRoom ();
			}

			ProcessInputs ();
			if (IsFiring) {
				if (nextShootTime <= Time.time) {
					CmdShoot ();
					nextShootTime = Time.time + playerInfo.fireRate;
				}
			}
			if (UsingUltra) {
				UseUltra ();
			}
			return;

		}


		#if !UNITY_MIN_5_4
		/// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
		void OnLevelWasLoaded(int level)
		{
		this.CalledOnLevelWasLoaded(level);
		}
		#endif


		void CalledOnLevelWasLoaded (int level)
		{
			GameObject _uiGo = Instantiate (this.PlayerUiPrefab) as GameObject;
			_uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);

			// player Score
			GameObject _uiGo1 = Instantiate (this.PlayerScorePrefab) as GameObject;
			_uiGo1.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
		}

		// The player take damage here
		void OnTriggerEnter2D (Collider2D obj)
		{
			Debug.Log ("PlayerManager: OnTriggerEnter2D");
			if (!photonView.isMine) {
				return;
			}

			Debug.Log (obj);
		}

		public float getHealthPercentage ()
		{
			return playerInfo.health / playerInfo.initialHealth;
		}



		#endregion

		#region Custom

		/// <summary>
		/// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
		/// </summary>
		void ProcessInputs ()
		{
			Vector2 shootVec = new Vector2 (CnInputManager.GetAxis ("Horizontal"), CnInputManager.GetAxis ("Vertical"));
			if (shootVec.magnitude > 0) {
				IsFiring = true;
			} else {
				IsFiring = false;
			}

			if (CrossPlatformInputManager.GetButtonUp ("Ultra")) {
				UsingUltra = true;
			} else {
				UsingUltra = false;
			}

		}

		void CmdShoot ()
		{
			Debug.Log ("CmdShoot is called");
			Vector2 shootVec = new Vector2 (CnInputManager.GetAxis ("Horizontal"), CnInputManager.GetAxis ("Vertical"));
			float angle = Mathf.Atan2 (shootVec.y, shootVec.x) * Mathf.Rad2Deg - 90f;

			switch (LocalPlayerType) {
			case Constant.FireElementType:
				this.GetComponent <FireElement> ().fire (this.transform.position, angle, shootVec.normalized);
				break;
			case Constant.ElectricElementType:
				this.GetComponent <ElectricElement> ().fire (this.transform.position, angle, shootVec.normalized);
				break;
			case Constant.IceElementType:
				this.GetComponent <IceElement> ().fire (this.transform.position, angle, shootVec.normalized);
				break;
			}
		}

		void UseUltra ()
		{
			Debug.Log ("UsingUltra is called");
			switch (PlayerManager.LocalPlayerType) { // set at GameManager.cs: Start()
			case Constant.FireElementType:
				this.GetComponent <FireElement> ().useUltra (this.transform.position);
				break;
			case Constant.ElectricElementType:
				this.GetComponent <ElectricElement> ().useUltra (this.transform.position);
				break;
			case Constant.IceElementType:
				this.GetComponent <IceElement> ().useUltra (this.transform.position);
				break;
			case "RancherElement":
				// TODO
				break;
			case "StoneElement":
				// TODO
				break;
			default:
				// TODO
				break;
			}
		}

		#endregion

		#region IPunObservable implementation

		void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting) {
				// We own this player: send the others our data
				stream.SendNext (IsFiring);
				stream.SendNext (playerInfo);
				stream.SendNext (UsingUltra);
			} else {
				// Network player, receive data
				this.IsFiring = (bool)stream.ReceiveNext ();
				this.playerInfo = (PlayerInfo)stream.ReceiveNext ();
				this.UsingUltra = (bool)stream.ReceiveNext ();
			}
		}

		#endregion
	}
}
