using UnityEngine;
using System.Collections;

public class Crafting : MonoBehaviour {

	public int NodeNumberMax;
	public int NodeNumberMin;
	public float Latency;
	public float TotalTime;
	public int NodeNumber;
	public int CurrentNode;
	public float Tempo;
	public float CurrentTime;
	public bool KeyTry;
	public int KeyPressed;
	public float HpSuccess;
	public float AtkSuccess;
	public float RangeSuccess;
	public int Failure;
	public GameObject[] Cube;
	public GameObject[] Anneau;
	
	// Use this for initialization
	void Start () {
		NodeNumberMin = 3;
		NodeNumberMax = 5;
		Latency = 0.25f;
		CurrentTime = 0.0f;
		TotalTime = 5.0f;
		CurrentNode = 1;
		HpSuccess = 0.0f;
		AtkSuccess = 0.0f;
		RangeSuccess = 0.0f;
		Failure = 0;
		KeyTry = false;
		SetNodeNumber ();
		SetTempo ();
	
	}
	
	// Update is called once per frame
	void Update () {
		Anneau[CurrentNode-1].transform.localScale -= new Vector3(0.015F/Tempo, 0.015F/Tempo, 0);
		if (CheckKeystroke () && IsInTheZone () && !KeyTry) {
						Cube[CurrentNode-1].renderer.material.SetColor ("_Color", ColorSet());
						if (KeyPressed == 1)
								HpSuccess ++;
						else if (KeyPressed == 2)
								AtkSuccess ++;
						else if (KeyPressed == 3)
								RangeSuccess ++;
						KeyTry = true;
				} else if (CheckKeystroke () && !IsInTheZone ()&& !KeyTry) {
						Cube[CurrentNode-1].renderer.material.SetColor ("_Color", Color.black);
						Failure ++;
						KeyTry = true;
				}
		TimeoutCurrentNode ();
		CheckCurrentNode ();
		CurrentTime += Time.deltaTime;
	}

	void SetNodeNumber () {
		NodeNumber = Random.Range (NodeNumberMin, NodeNumberMax);
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
	bool CheckKeystroke (){
		if (Input.GetKeyDown(KeyCode.A)) {
						KeyPressed = 1;
						return true;
				} else if (Input.GetKeyDown(KeyCode.S)) {
						KeyPressed = 2;
			            return true;
				} else if (Input.GetKeyDown(KeyCode.D)) {
						KeyPressed = 3;
						return true;
				} else
						return false;
		}
	void CheckCurrentNode (){
		if (CurrentTime >= CurrentNode * Tempo + Latency) {
						CurrentNode++;
						KeyTry = false;
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
						Cube [CurrentNode - 1].renderer.material.SetColor ("_Color", Color.black);
	}

}
