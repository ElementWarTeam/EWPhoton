﻿#if UNITY_5 && (!UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2 && ! UNITY_5_3) || UNITY_6
#define UNITY_MIN_5_4
#endif
using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

namespace Com.EW.MyGame
{
	/// <summary>
	/// Player manager. 
	/// Handles fire Input and Beams.
	/// </summary>
	public class PlayerManager : Photon.PunBehaviour {

		#region Public Variables
		[Tooltip("The current Health of our player")]
		public float Health = 1000f;

		[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
		public static GameObject LocalPlayerInstance;

		[Tooltip("The Player's UI GameObject Prefab")]
		public GameObject PlayerUiPrefab;


		#endregion

		#region Private Variables

		//True, when the user is firing
		bool IsFiring;

		#endregion

		#region MonoBehaviour CallBacks

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
		/// </summary>
		void Awake()
		{
			// #Important
			// used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
			if ( photonView.isMine)
			{
				PlayerManager.LocalPlayerInstance = this.gameObject;
			}
			// #Critical
			// we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
			DontDestroyOnLoad(this.gameObject);

		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during initialization phase.
		/// </summary>
		void Start()
		{	// camera work
			CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();


			if (_cameraWork!=null )
			{
				if ( photonView.isMine)
				{
					_cameraWork.OnStartFollowing();
				}
			}else{
				Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.",this);
			}


			// player UI
			if (PlayerUiPrefab!=null)
			{
				GameObject _uiGo =  Instantiate(PlayerUiPrefab) as GameObject;
				_uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
			} else {
				Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.",this);
			}

			// 
			#if UNITY_MIN_5_4
			// Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
			{
				this.CalledOnLevelWasLoaded(scene.buildIndex);
			};
			#endif
		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity on every frame.
		/// </summary>
		void Update()
		{	
			// if health less than 0, leave game
			if ( Health <= 0f)
			{
				GameManager.Instance.LeaveRoom();
			}
		}


		#if !UNITY_MIN_5_4
		/// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
		void OnLevelWasLoaded(int level)
		{
		this.CalledOnLevelWasLoaded(level);
		}
		#endif


		void CalledOnLevelWasLoaded(int level)
		{
			// check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
			if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
			{
				transform.position = new Vector3(0f, 5f, 0f);
			}

			GameObject _uiGo = Instantiate(this.PlayerUiPrefab) as GameObject;
			_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
		}




		#endregion

		#region Custom

		/// <summary>
		/// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
		/// </summary>
		void ProcessInputs()
		{
			// when we press the "Fire!" key

		}
		#endregion

	}
}