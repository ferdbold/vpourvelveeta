/// <summary>
/// Player.cs
/// Gère l'objet joueur (encapsule toute la zone de jeu d'un joueur)
/// </summary>

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Attributs publics
	public string moveInput;
	public string attackInput;
	public string craftInput;
	public string craftHpInput;
	public string craftAtkInput;
	public string craftRangeInput;

	// Attributs
	private Transform _bunch;
	private Transform _enemyBunch;

	void Start() {
		_bunch = transform.Find("Bunch");
		_enemyBunch = transform.Find("Enemies");
	}

	void Update () {
		
	}

	// Accesseurs
	public Transform Bunch {
		get { return _bunch; }
	}

	public Transform EnemyBunch {
		get { return _enemyBunch; }
	}
}
