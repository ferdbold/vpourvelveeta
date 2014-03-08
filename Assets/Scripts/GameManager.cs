﻿/// <summary>
/// GameManager.cs
/// Gère l'application.
/// </summary>

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Attributes
	private int statMultiplier;
	private float _timeElapsed;
	private float _spawnTimeElapsed;


	// Public attributes
	public Player[] players;
	public float spawnInterval = 2F;
	public GameObject p_yunittoEnemy;

	public int StatMultiplier
	{
		get { return statMultiplier;}
		set { statMultiplier = value;}
	}

	void Start () {
		_timeElapsed = 0;
		_spawnTimeElapsed = 0;
	}
	
	void Update () {
		_timeElapsed += Time.deltaTime;
		_spawnTimeElapsed += Time.deltaTime;

		// Faire progresser l'intervalle de spawn (plus en plus de spawns)
		spawnInterval *= 0.9997F;

		// Spawner des ennemis random
		if (_spawnTimeElapsed >= spawnInterval) {
			SpawnBasicYunittos();
		}
	}

	private void SpawnBasicYunittos() {
		_spawnTimeElapsed = 0;

		Debug.Log (players);
		foreach (Player player in players) {
			GameObject eYuni = (GameObject)Instantiate(p_yunittoEnemy, new Vector3(Screen.width + 50, transform.position.y, transform.position.z), transform.rotation);
			Debug.Log (player);
			eYuni.transform.parent = player.Bunch;
		}
	}
}
