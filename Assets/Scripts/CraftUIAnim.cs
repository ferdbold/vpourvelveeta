using UnityEngine;
using System.Collections;

public class CraftUIAnim : MonoBehaviour {

	//Public Attributes
	public bool opening = false;
	public bool closing = false;

	//Attributes
	private float timer = 0;
	private float animationDistanceUI;
	private float animationTime;

	//Getters/setters
	public float AnimationDistanceUI {
		get{ return animationDistanceUI;}
		set{ animationDistanceUI = value;}
	}
	public float AnimationTime {
		get{ return animationTime;}
		set{ animationTime = value;}
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(opening) {
			if(timer <= animationTime) {
				timer += 1*Time.deltaTime;
				transform.Translate( new Vector3((animationDistanceUI/animationTime*Time.deltaTime),0,0));
			} else {
				timer = 0;
				opening = false;
			}
		}
		if(closing){
			if(timer <= animationTime) {
				timer += 1*Time.deltaTime;
				transform.Translate( new Vector3(-animationDistanceUI/animationTime*Time.deltaTime,0,0));
			} else {
				timer = 0;
				closing = false;
			}
		}
	}
}
