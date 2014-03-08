/// <summary>
/// GameManager.cs
/// Gère l'application.
/// </summary>

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private int statMultiplier;

	public int StatMultiplier
	{
		get { return statMultiplier;}
		set { statMultiplier = value;}
	}
	// Use this for initialization
	void Start () {
		statMultiplier = 10;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
