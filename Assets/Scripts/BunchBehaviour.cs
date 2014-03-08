/// <summary>
/// BunchBehaviour.cs
/// Gère le déplacement du joueur ainsi que le mouvement de son bunch.
/// </summary>

using UnityEngine;
using System.Collections;

public class BunchBehaviour : MonoBehaviour {

	// Attributs publics
	public GameObject p_yunitto;

	void Start () {
		// Pour tests seulement
		this.GenerateTestBunch();
	}

	void Update () {
		Move();
	}

	void GenerateTestBunch() {
		for(int i=0; i<10; i++) {
			// Random offset
			Vector3 offset = transform.position;
			offset.x += Random.Range(0.0F, 1.0F);

			GameObject yuni = (GameObject)Instantiate(p_yunitto, offset, transform.rotation);
			yuni.transform.parent = transform;
		}
	}

	void Move() {

	}
}
