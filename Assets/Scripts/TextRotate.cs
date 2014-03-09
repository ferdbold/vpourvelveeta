using UnityEngine;
using System.Collections;

public class TextRotate : MonoBehaviour {
	float size;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 1.0f);
		transform.Translate (new Vector3 (0, 0, -5));
		gameObject.transform.localScale = new Vector3(2f,2f,2f);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Rotate (new Vector3(0,0,1));
		transform.localScale += new Vector3(0.5f,0.5f,0.5f)* Time.deltaTime;
	}
}
