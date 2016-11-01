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
		public GameObject UltraUI;

		// OOP
		PlayerInfo playerInfo;
		public bool IsFiring;
		public bool UsingUltra;

		#endregion

		#region Private Variables

		public float timeBetweenShots = 1f;
		private float nextShootTime = 0.0f;
		private Vector2 releasePressDirection = new Vector2 (0f, 0f);

		#endregion

		#region MonoBehaviour CallBacks

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
		/// </summary>
		void Awake ()
		{
			// #Important
			// used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
//			if (photonView.isMine) {
			// SETUP PLAYER INFO
			playerInfo = this.GetComponent <PlayerInfo> ();
			playerInfo.playerId = "player name";
			playerInfo.type = PlayerManager.LocalPlayerType;
			if (photonView.isMine) {
				PlayerManager.LocalPlayerInstance = this.gameObject;
			}
//			}
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


			// player ultra button
			UltraUI = GameObject.Find ("UltUI");
			if (UltraUI != null) {
				UltraUI.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
			} else {
				Debug.LogWarning ("<Color=Red><a>Missing</a></Color> UltraUI reference on player Prefab.", this);
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
			} else { 
				CmdStopFire ();
			}
			if (UsingUltra) {
				UseUltra ();
			}
			// Debug
			DebugController.displayPlayerInfo (playerInfo);
			return;

		}

		void FixedUpdate ()
		{
			if (photonView.isMine == false && PhotonNetwork.connected == true) {
				return;
			}
			Rigidbody2D rb2d = GetComponent<Rigidbody2D> ();
			Vector2 shootVec = new Vector2 (CnInputManager.GetAxis ("Horizontal"), CnInputManager.GetAxis ("Vertical"));
			// Rotate
			if (shootVec.magnitude > 0) {
				float angle = Mathf.Atan2 (shootVec.y, shootVec.x) * Mathf.Rad2Deg - 90f;
				rb2d.rotation = angle;
			}
			if (IsFiring && LocalPlayerInstance.Equals (Constant.StoneElementType)) {
				return; // Stone in charging state will not move
			}
			// Moving
			Vector2 moveVec = new Vector2 (CrossPlatformInputManager.GetAxis ("LeftHorizontal"), CrossPlatformInputManager.GetAxis ("LeftVertical")); 
			rb2d.position += moveVec.normalized * 0.05f;

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

			// player ultra button
			UltraUI = GameObject.Find ("UltUI");
			if (UltraUI != null) {
				UltraUI.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
			} else {
				Debug.LogWarning ("<Color=Red><a>Missing</a></Color> UltraUI reference on player Prefab.", this);
			}
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
			if (playerInfo == null)
				return 1;
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
				releasePressDirection [0] = shootVec [0];
				releasePressDirection [1] = shootVec [1];
				IsFiring = true;
			} else {
				IsFiring = false;
			}

			if (CrossPlatformInputManager.GetButtonUp ("Ultra") && playerInfo.isUltReady) {
				UsingUltra = true;
			} else {
				UsingUltra = false;
			}

		}

		void CmdShoot ()
		{
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
			case Constant.DarkElementType:
				this.GetComponent <DarkElement> ().fire (this.transform.position, angle, shootVec.normalized);
				break;
			case Constant.StoneElementType:
				this.GetComponent <StoneElement> ().startCharge (this.transform.position, angle, shootVec.normalized);
				break;
			}
		}

		void CmdStopFire ()
		{
			switch (LocalPlayerType) {
			case Constant.StoneElementType:
				this.GetComponent <StoneElement> ().charge (this.transform.position);
				break;
			}
		}

		void UseUltra ()
		{
			Debug.Log ("UsingUltra is called");
			switch (PlayerManager.LocalPlayerType) { // set at GameManager.cs: Start()
			case Constant.FireElementType:
				this.GetComponent <FireElement> ().useUltra (this.transform.position);
				playerInfo.isUltReady = false;
				playerInfo.energy = 0f;
				break;
			case Constant.ElectricElementType:
				this.GetComponent <ElectricElement> ().useUltra (this.transform.position);
				playerInfo.isUltReady = false;
				playerInfo.energy = 0f;
				break;
			case Constant.IceElementType:
				this.GetComponent <IceElement> ().useUltra (this.transform.position);
				playerInfo.isUltReady = false;
				playerInfo.energy = 0f;
				break;
			case Constant.DarkElementType:
				this.GetComponent <DarkElement> ().useUltra (this.transform.position);
				playerInfo.isUltReady = false;
				playerInfo.energy = 0f;
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
				stream.SendNext (UsingUltra);
			} else {
				// Network player, receive data
				this.IsFiring = (bool)stream.ReceiveNext ();
				this.UsingUltra = (bool)stream.ReceiveNext ();
			}
		}

		#endregion

		[PunRPC]
		public void TakeDamage (float damage)
		{
			playerInfo.takeDamage (damage);
		}

		[PunRPC]
		public void TakeContinousDamage (float damage)
		{
			playerInfo.takeContinousDamage (damage);
		}

		[PunRPC]
		public void AddHealth (float health)
		{
			playerInfo.addHealth (health);
		}

		[PunRPC]
		public void AddScore (float score)
		{
			playerInfo.addScore (score);
		}

		[PunRPC]
		public void TakeContiousSpeedDamage (float speedDelta, float time)
		{
			playerInfo.takeContiousSpeedDamage (speedDelta, time);
		}

		[PunRPC]
		public void ChangeDenfense (float defenseDeltaDelta)
		{
			playerInfo.changeDenfense (defenseDeltaDelta);
		}
	}
}