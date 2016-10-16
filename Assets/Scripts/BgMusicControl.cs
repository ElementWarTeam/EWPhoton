using UnityEngine;
using System.Collections;

public class BgMusicControl : MonoBehaviour
{

	private AudioSource audioSource;

	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
		audioSource.Play ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void switchMusic ()
	{
		if (audioSource.isPlaying) {
			audioSource.Stop ();
		} else {
			audioSource.Play ();
		}
	}
}
