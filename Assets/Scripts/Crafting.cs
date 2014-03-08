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
	public bool NodeFailed;
	public bool KeyTry;
	public int KeyPressed;
	public float HpSuccess;
	public float AtkSuccess;
	public float RangeSuccess;
	public int Failure;
	public GameObject[] Cube;

	// Use this for initialization
	void Start () {
		NodeNumberMin = 5;
		NodeNumberMax = 5;
		Latency = 1.25f;
		CurrentTime = 0.0f;
		TotalTime = 50.0f;
		CurrentNode = 1;
		HpSuccess = 0.0f;
		AtkSuccess = 0.0f;
		RangeSuccess = 0.0f;
		Failure = 0;
		NodeFailed = false;
		KeyTry = false;
		SetNodeNumber ();
		SetTempo ();
	
	}
	
	// Update is called once per frame
	void Update () {
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
		if (Input.GetKey(KeyCode.A)) {
						KeyPressed = 1;
						return true;
				} else if (Input.GetKey(KeyCode.S)) {
						KeyPressed = 2;
			            return true;
				} else if (Input.GetKey(KeyCode.D)) {
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
}
