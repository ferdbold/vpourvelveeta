using UnityEngine;
using System.Collections;

public class UnitEmphasis : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 1.8f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale -= new Vector3(0.45f,0.45f,0.4f)* Time.deltaTime;
	}
}
