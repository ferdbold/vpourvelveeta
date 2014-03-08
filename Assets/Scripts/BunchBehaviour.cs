/// <summary>
/// BunchBehaviour.cs
/// Gère le déplacement du joueur ainsi que le mouvement de son bunch.
/// </summary>

using UnityEngine;
using System.Collections;

public class BunchBehaviour : MonoBehaviour {

	// Attributs publics
	public GameObject p_yunitto;
	public float moveSpeed = 5;
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

			/*GameObject yuni = (GameObject)Instantiate(p_yunitto, offset, transform.rotation);
			yuni.transform.parent = transform;
			yunitto = (YunittoWiggle)yuni.GetComponent<YunittoWiggle>();
			if(n==1)yunitto.SetStats (0.2f, 0.5f, 0.3f);
			if(n==2)yunitto.SetStats (0.5f, 0.5f, 0.0f);*/
		}
	}

	public void Move() {
		float moveAxis = Input.GetAxis(_player.moveInput);

		transform.Translate(moveAxis * Time.deltaTime * moveSpeed, 0F, 0F);
	}
}
