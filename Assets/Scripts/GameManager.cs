/// <summary>
/// GameManager.cs
/// Gère l'application.
/// </summary>

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Attributes
	private float statMultiplier;
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

	void Start () {
		_timeElapsed = 0;
		_spawnTimeElapsed = 0;
		statMultiplier = 0.5f;
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
			CreateEnemyYunitto(player);
		}
	}

	public void CreateEnemyYunitto(Player player, float hp = 0.2f, float atk = 0.5f, float range = 0.3f) {
		GameObject yuni = (GameObject)Instantiate(p_yunittoEnemy, new Vector3(10, player.transform.position.y, player.transform.position.z), transform.rotation);
		yuni.transform.parent = player.EnemyBunch;

		YunittoEnemy yunitto = (YunittoEnemy)yuni.GetComponent<YunittoEnemy>();
		yunitto.SetStats(hp, atk, range);
	}
}
