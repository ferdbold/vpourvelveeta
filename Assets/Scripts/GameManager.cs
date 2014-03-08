/// <summary>
/// GameManager.cs
/// Gère l'application.
/// </summary>

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Attributes
	private float statMultiplier; //Multiplé aux stats des armées ennemis
	private float friendlyStatMultiplier; //Multiplé aux stats des armées des joueurs
	private float ennemyAmountMultiplier; //Nombre d'ennemis qui apparait a chaque craft
	private float _timeElapsed;
	private float _spawnTimeElapsed;



	// Public attributes
	public Player[] players;
	public float spawnInterval = 2F;
	public GameObject p_yunittoEnemy;

	public float StatMultiplier
	{
		get { return statMultiplier;}
		set { statMultiplier = value;}
	}
	public float FriendlyStatMultiplier
	{
		get { return friendlyStatMultiplier;}
		set { friendlyStatMultiplier = value;}
	}
	public float EnnemyAmountMultiplier
	{
		get { return ennemyAmountMultiplier;}
		set { ennemyAmountMultiplier = value;}
	}

	void Start () {
		_timeElapsed = 0;
		_spawnTimeElapsed = 0;
		statMultiplier = 0.6f;
		friendlyStatMultiplier = 1f;

	}
	
	void Update () {
		_timeElapsed += Time.deltaTime;
		_spawnTimeElapsed += Time.deltaTime;
		ennemyAmountMultiplier = (0.1f * _timeElapsed);

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
			player.CreateEnemyYunitto();
		}
	}


}
