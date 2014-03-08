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
	private Transform _crafting;

	public GameObject unitsGood;
	public GameObject unitsBad;
	private Yunitto yunitto;
	private YunittoEnemy yunittoEnemy;

	void Start() {
		_bunch = transform.Find("Bunch");
		_enemyBunch = transform.Find("Enemies");
		_crafting = transform.Find("Crafting");

		yunitto = unitsGood.GetComponent<Yunitto>();
		yunittoEnemy = unitsBad.GetComponent<YunittoEnemy>();
	}
	
	void Update () {
		/*if (Input.GetAxis(craftInput)) {
			Crafting crafting = (Crafting)_crafting.GetComponent<Crafting>();
			crafting.BeginCrafting();
		}*/
	}

	// Accesseurs
	public Transform Bunch {
		get { return _bunch; }
	}

	public Transform EnemyBunch {
		get { return _enemyBunch; }
	}
}
