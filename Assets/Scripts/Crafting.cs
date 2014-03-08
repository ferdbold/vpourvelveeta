using UnityEngine;
using System.Collections;

public class Crafting : MonoBehaviour {

	// Attributes
	private Player _player;
	private bool _isCrafting;
	private float CurrentTime;
	private int NodeNumber;
	private int CurrentNode;
	private float Tempo;
	private bool KeyTry;
	private int KeyPressed;
	private float HpSuccess;
	private float AtkSuccess;
	private float RangeSuccess;
	private int Failure;
	private GameObject[] Spheres;
	private GameObject[] Anneaux;

	// Public attributes
	public GameObject p_sphere;
	public GameObject p_anneau;

	public int NodeNumberMax = 3;
	public int NodeNumberMin = 8;
	public float Latency = 0.25F;
	public float TotalTime = 5.0F;

	void Start () {
		_player = (Player)transform.parent.GetComponent<Player>();
		_isCrafting = false;

		Spheres = new GameObject[0];
		Anneaux = new GameObject[0];
	}

	void Update () {
		if (_isCrafting) {
			if (Input.GetButtonDown(_player.craftInput)) {
				QuitCrafting();
			} else {
				Anneaux[CurrentNode-1].transform.localScale -= new Vector3(0.015F/Tempo, 0.015F/Tempo, 0);
				if (CheckKeystroke () && IsInTheZone () && !KeyTry) {
					Spheres[CurrentNode-1].renderer.material.SetColor ("_Color", ColorSet());
					if (KeyPressed == 1)
						HpSuccess++;
					else if (KeyPressed == 2)
						AtkSuccess++;
					else if (KeyPressed == 3)
						RangeSuccess++;
					KeyTry = true;
				} else if (CheckKeystroke () && !IsInTheZone ()&& !KeyTry) {
					Spheres[CurrentNode-1].renderer.material.SetColor ("_Color", Color.black);
					Failure++;
					KeyTry = true;
				}
				TimeoutCurrentNode ();
				CheckCurrentNode ();
				CurrentTime += Time.deltaTime;
			}
		} else if (Input.GetButtonDown(_player.craftInput)) {
			QuitCrafting();
			BeginCrafting();
		}
	}
	
	/// <summary>
	/// Démarre une session de crafting.
	/// </summary>
	public void BeginCrafting() {
		// Déterminer le nombre de ticks et le tempo
		SetNodeNumber();
		SetTempo();

		// Instancier les sphères et les anneaux
		float sphereMediane = (float)(NodeNumber+1) / 2F - 1;
		for(int i=0; i<NodeNumber; i++) {
			float xSphere = transform.position.x + (i - sphereMediane) * 2;

			Spheres[i] = (GameObject)Instantiate(p_sphere, new Vector3(xSphere, transform.position.y, transform.position.z), transform.rotation);
			Spheres[i].transform.parent = transform;

			Anneaux[i] = (GameObject)Instantiate(p_anneau, new Vector3(xSphere, transform.position.y, transform.position.z), transform.rotation);
			Anneaux[i].transform.parent = Spheres[i].transform;
		}

		// Initialiser les variables
		CurrentTime = 0.0f;
		CurrentNode = 1;
		HpSuccess = 0.0f;
		AtkSuccess = 0.0f;
		RangeSuccess = 0.0f;
		Failure = 0;
		KeyTry = false;

		this._isCrafting = true;
	}

	/// <summary>
	/// Détruire les éléments inutiles à la fin d'une séance de crafting.
	/// </summary>
	private void QuitCrafting() {
		// Détruire les sphères
		foreach(GameObject sphere in Spheres) {
			GameObject.Destroy(sphere);
		}

		_isCrafting = false;
	}

	/// <summary>
	/// Termine la session en cours de crafting et crée les Yunitto résultants.
	/// </summary>
	private void SpawnCraftingResults() {
		// @TODO
	}

	void SetNodeNumber () {
		NodeNumber = Random.Range (NodeNumberMin, NodeNumberMax);
		Spheres = new GameObject[NodeNumber];
		Anneaux = new GameObject[NodeNumber];
	}

	void SetTempo () {
		Tempo = TotalTime / (float)NodeNumber;
	}

	bool IsInTheZone (){
		if ((CurrentTime >= CurrentNode * Tempo - Latency) && (CurrentTime <= CurrentNode * Tempo + Latency) && CurrentTime <= TotalTime+Latency)
			return true;
		else
			return false;
	}

	bool CheckKeystroke () {
		if (Input.GetButtonDown(_player.craftHpInput)) {
			KeyPressed = 1;
			return true;
		} else if (Input.GetButtonDown(_player.craftAtkInput)) {
			KeyPressed = 2;
            return true;
		} else if (Input.GetButtonDown(_player.craftRangeInput)) {
			KeyPressed = 3;
			return true;
		} else
			return false;
	}

	void CheckCurrentNode (){
		if (CurrentTime >= CurrentNode * Tempo + Latency) {
			if (CurrentNode < NodeNumber) {
				CurrentNode++;
				KeyTry = false;
			} else {
				QuitCrafting();
				SpawnCraftingResults();
				BeginCrafting();
			}
		}
	}

	Color ColorSet(){
		if (KeyPressed == 1)
			return Color.red;
		else if (KeyPressed == 2)
			return Color.blue;
		else if (KeyPressed == 3)
			return Color.green;
		else 
			return Color.black;
	}

	void TimeoutCurrentNode(){
		if ((CurrentTime >= CurrentNode * Tempo + Latency) && !KeyTry)
						Spheres [CurrentNode - 1].renderer.material.SetColor ("_Color", Color.black);
	}

}
