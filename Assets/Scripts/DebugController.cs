using UnityEngine;
using System.Collections;

public class DebugController : MonoBehaviour
{

	public GameObject obstaclePrefabs;
	public GameObject player;
	public Camera myCamera;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		#if UNITY_IOS
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
			Vector2 position = Input.GetTouch (0).position;
			Vector2 point = myCamera.ScreenToWorldPoint (position);
			Instantiate (obstaclePrefabs, point, Quaternion.identity);
		}
		#endif

		#if UNITY_EDITOR
		if (Input.GetMouseButtonUp (0)) {
			Vector2 point = myCamera.ScreenToWorldPoint (Input.mousePosition);
			Instantiate (obstaclePrefabs, point, Quaternion.identity);
		}
		#endif


	}
}
