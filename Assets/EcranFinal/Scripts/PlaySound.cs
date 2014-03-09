using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {

	public AudioSource p1won;
	public AudioSource p2won;

	// Use this for initialization
	void Start () {
		if(Base.P1won) p1won.Play();
		else
			p2won.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
