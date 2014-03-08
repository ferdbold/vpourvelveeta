using UnityEngine;
using System.Collections;

public class Base : MonoBehaviour {
	private float hp;
	public float Hp
	{
		get {return hp;}
		set { hp = value;}
	}

	// Use this for initialization
	void Start () {
		hp = 100;
		if(transform.parent.name == "P1") gameObject.layer = 8;
		else gameObject.layer = 9;
	}
	
	// Update is called once per frame
	void Update () {
		if (hp <= 0) {
			Debug.Log(transform.parent.name + " HAS LOST!");
		}
	}
}
