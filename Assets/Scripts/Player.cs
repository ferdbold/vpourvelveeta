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

	public GameObject unitsGood;
	public GameObject unitsBad;
	private Yunitto yunitto;
	private YunittoEnemy yunittoEnemy;


	void Start () {
		yunitto = unitsGood.GetComponent<Yunitto>();
		yunittoEnemy = unitsBad.GetComponent<YunittoEnemy>();
		GameObject clone;
		clone = (GameObject)Instantiate(unitsBad,transform.position,Quaternion.identity);
		yunittoEnemy = (YunittoEnemy)clone.GetComponent<YunittoEnemy>();
		yunittoEnemy.SetStats (0.5f, 0.5f, 0f, true);

	}
	// Update is called once per frame
	void Update () {
		
	}
}
