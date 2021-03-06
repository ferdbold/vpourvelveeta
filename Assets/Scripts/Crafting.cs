﻿using UnityEngine;
using System.Collections;

public class Crafting : MonoBehaviour {

	// Attributes
	private Player _player;
	private GameManager _gameManager;
	private bool _isCrafting;
	public float CurrentTime;
	private int NodeNumber;
	private int CurrentNode;
	public float Tempo;
	private bool KeyTry;
	private int KeyPressed;
	private float HpSuccess;
	private float AtkSuccess;
	private float RangeSuccess;
	private int Failure;
	private GameObject[] Spheres;
	private GameObject[] Anneaux;
	private float animationDistanceUI = 20f;

	// Public attributes
	public GameObject p_sphere;
	public GameObject p_anneau;
	public GameObject Message1;
	public GameObject Message2;
	public GameObject CraftingUI;
	private GameObject CraftUI;

	public int NodeNumberMax = 3;
	public int NodeNumberMin = 8;
	public float Latency = 0.05F;
	public float TotalTime = 1.5F;

	void Start () {
		_player = (Player)transform.parent.GetComponent<Player>();
		_gameManager = (GameManager)transform.parent.parent.GetComponent<GameManager>();
		_isCrafting = false;


		Spheres = new GameObject[0];
		Anneaux = new GameObject[0];
	}

	void Update () {
		if (_isCrafting) {
			if (CurrentTime >= 0.25f){
				Anneaux[CurrentNode-1].transform.localScale -= new Vector3((ShrinkSpeed()*Time.deltaTime),(ShrinkSpeed()*Time.deltaTime), 0);
			} 
			if (CheckKeystroke () && IsInTheZone () && !KeyTry) {

				Spheres[CurrentNode-1].renderer.material.SetColor ("_Color", ColorSet());
				if (KeyPressed == 1)
					HpSuccess++;
				else if (KeyPressed == 2)
					AtkSuccess++;
				else if (KeyPressed == 3)
					RangeSuccess++;
				KeyTry = true;
			} else if ((CheckKeystroke () && !IsInTheZone ()&& !KeyTry)) {
				Spheres[CurrentNode-1].renderer.material.SetColor ("_Color", Color.black);
				Failure++;
				KeyTry = true;
			}

				TimeoutCurrentNode ();
				CheckCurrentNode ();
				CurrentTime += Time.deltaTime;
		}
	}
	
	/// <summary>
	/// Démarre une session de crafting.
	/// </summary>
	public void BeginCraftingSession (){
		//Create UI, start it's entering animation. Crafting itself not yet started.
		CraftUI = (GameObject)Instantiate(CraftingUI,transform.position,transform.rotation);
		CraftUI.transform.Translate( new Vector3(-animationDistanceUI,0,0.5f));
		CraftUIAnim craftUIAnim = (CraftUIAnim)CraftUI.GetComponent<CraftUIAnim>();
		craftUIAnim.opening = true;
		craftUIAnim.AnimationDistanceUI = animationDistanceUI;
		craftUIAnim.AnimationTime = _player.CraftingTransitionTime;

	}
	public void EndCraftingSession (){
		//Remove UI
		GameObject.Destroy (CraftUI);
		
	}
	public void EndCraftingAnimationStart(){
		//Starts the removal of crafting UI, crafting over, but not yet in Battle.
		CraftUIAnim craftUIAnim = (CraftUIAnim)CraftUI.GetComponent<CraftUIAnim>();
		craftUIAnim.closing = true;
	}

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
	public void QuitCrafting() {
		// Détruire les sphères
		foreach(GameObject sphere in Spheres) {
			GameObject.Destroy(sphere);
		}

		_isCrafting = false;
	}

	/// <summary>
	/// Crée les Yunitto résultants d'une session de crafting.
	/// </summary>
	private void SpawnCraftingResults() {
		float hp = (HpSuccess / NodeNumber);
		float atk = (AtkSuccess / NodeNumber);
		float range = (RangeSuccess / NodeNumber);
			_player.CreateYunitto (hp, atk, range);
		for (int i=0; i<(int)(_gameManager.EnnemyAmountMultiplier); i++) {
			_player.OtherPlayer.CreateEnemyYunitto(hp,atk,range);
		}
		audio.Play ();
		if(((HpSuccess+AtkSuccess+RangeSuccess)/NodeNumber) > 0.6f) {
			Instantiate (Message1, transform.position,transform.rotation);
		}
		else Instantiate (Message2, transform.position,transform.rotation);
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
			return Color.yellow;
		else if (KeyPressed == 2)
			return Color.red;
		else if (KeyPressed == 3)
			return Color.blue;
		else 
			return Color.black;
	}

	void TimeoutCurrentNode(){
		if ((CurrentTime >= CurrentNode * Tempo + Latency) && !KeyTry) {
			Spheres [CurrentNode - 1].renderer.material.SetColor ("_Color", Color.black);
			Failure++;
		}
	}
	float ShrinkSpeed(){
		return 0.58f * NodeNumber;
	}

}
