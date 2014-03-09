using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {

	public AudioClip myClip;
	public AudioClip myClip2;

	// Use this for initialization
	void Start () {
		audio.PlayOneShot (myClip);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
