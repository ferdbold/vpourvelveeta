/// <summary>
/// BunchBehaviour.cs
/// Gère le déplacement du joueur ainsi que le mouvement de son bunch.
/// </summary>

using UnityEngine;
using System.Collections;

public class BunchBehaviour : MonoBehaviour {

	// Attributs publics
	public float moveSpeed = 5;
	public Transform _cursor;

	// materiel_couleur
	public Material white;
	public Material brown;
	public Material red;
	public Material blue;
	public Material green;

	// Attributs
	private Player _player;
	private YunittoWiggle yunitto;

	void Start () {
		_player = transform.parent.gameObject.GetComponent<Player>();
		_cursor = transform.parent.FindChild("Cursor");

		// Pour tests seulement
		this.GenerateTestBunch(1);
		this.GenerateTestBunch(2);
	}

	private void GenerateTestBunch(int n) {
		for(int i=0; i<10; i++) {
			// Random offset
			Vector3 offset = transform.position;
			offset.x += Random.Range(0.0F, 1.0F);

			_player.CreateYunitto(0.2f, 0.5f, 0.3f);
		}
	}

	public void Move() {
		float moveAxis = Input.GetAxis(_player.moveInput);

		_cursor.transform.Translate(moveAxis * Time.deltaTime * moveSpeed, 0F, 0F);
	}
}
