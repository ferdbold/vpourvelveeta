/// <summary>
/// GameManager.cs
/// Gère l'application.
/// </summary>

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private const float Min_AmbientSound = 0.2f;
	private const float Max_AmbientSound = 1.0f;
	// Attributes
	private float statMultiplier; //Multiplé aux stats des armées ennemis
	private float friendlyStatMultiplier; //Multiplé aux stats des armées des joueurs
	private float ennemyAmountMultiplier; //Nombre d'ennemis qui apparait a chaque craft
	private float _timeElapsed;
	private float _spawnTimeElapsed;

	private float timerUpdate;
	private GameObject Player1;




	// Public attributes
	public Player[] players;
	public float spawnInterval = 2F;
	public GameObject p_yunittoEnemy;
	public float ambiantSound;
	public AudioSource AmbiantMusic;

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
		ambiantSound = Min_AmbientSound;
		timerUpdate = 0f;
		Player1 = GameObject.Find ("P1");
		AmbiantMusic.Play ();
		AmbiantMusic.audio.loop = true;
	}
	
	void Update () {
				_timeElapsed += Time.deltaTime;
				_spawnTimeElapsed += Time.deltaTime;
				timerUpdate += Time.deltaTime;
				ennemyAmountMultiplier = (0.1f * _timeElapsed) + 1;
				if (statMultiplier <= friendlyStatMultiplier)
						statMultiplier += (_timeElapsed / 100); // unités ennemis deviennent de plus en plus forte jusqu'a etre aussi fort que les unités normale
		// Faire progresser l'intervalle de spawn (plus en plus de spawns)
		//spawnInterval *= 0.99F;

		// 		Spawner des ennemis random
		if (_spawnTimeElapsed >= spawnInterval) {
			SpawnBasicYunittos();
			spawnInterval *= 0.995F;
		}
		AmbiantMusic.volume = ambiantSound;
		if (timerUpdate >= 0.1) {
			ambiantSound -= 0.01f;
			if(ambiantSound < Min_AmbientSound) ambiantSound = Min_AmbientSound;
			timerUpdate = 0;
			YunittoWiggle[] units = (YunittoWiggle[])Player1.GetComponentsInChildren<YunittoWiggle>();
			foreach (YunittoWiggle unit in units) {

				if (unit.isInAllyAttackState)
				{
					if(ambiantSound < Max_AmbientSound)ambiantSound += 0.20f;
				}
			}
		}
	}

	private void SpawnBasicYunittos() {
		_spawnTimeElapsed = 0;

		foreach (Player player in players) {
			player.CreateEnemyYunitto(0.2f,0.2f,0.2f);
		}
	}


}
