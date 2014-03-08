/// <summary>
/// BunchBehaviour.cs
/// Gère le déplacement du joueur ainsi que le mouvement de son bunch.
/// </summary>

using UnityEngine;
using System.Collections;

public class BunchBehaviour : MonoBehaviour {

	// Attributs publics
	public GameObject p_yunitto;
	public float moveSpeed = 2;

	// Attributs
	private Player _player;

	void Start () {
		_player = transform.parent.gameObject.GetComponent<Player>();

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
		float moveAxis = Input.GetAxis(_player.moveInput);

		transform.Translate(moveAxis * Time.deltaTime * moveSpeed, 0F, 0F);
	}
}
