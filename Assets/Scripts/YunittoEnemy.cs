using UnityEngine;
using System.Collections;

public class YunittoEnemy : MonoBehaviour {
	const int MAX_RANGE = 600;
	const int MIN_RANGE = 20;
	
	private float hp; // vie de l'unité
	private float atk; // attaque de l'unité
	private float range; //Portée de tir de l'unité 
	private int unitType; //Type d'unité (1:HP, 2:ATK, 3:Range, 4:Balanced, 5:Weak)
	private bool isGood; // Est-ce que l'unité appartient a la faction du haut(good) ou du bas(bad)
	
	private float weakThreshold; //Si les stats combinées d'un unité est inférieur au threshold, elle est faible.

	private LayerMask layerMask;
	private RaycastHit hit;
	private GameObject ManagerObject; //Objet GameManager
	private GameManager manager; //Game Manager scripts
	private Shoot_Projectile shoot_Projectile; // Script Shootprojectile

	
	
	
	//Getters/Setters
	public float Hp 
	{ 
		get{return hp;}
		set{hp = value;}
	}
	public float Atk
	{
		get{return atk;}
		set{atk = value;}
	}
	public float Range
	{
		get{ return range;}
		set{ Range = value;}
	}
	public int UnitType
	{
		get{ return unitType;}
		set{ if (value>0 && value<6) unitType = value;}
	}
	public bool IsGood
	{
		get { return isGood;}
		set { isGood = value;}
	}
	
	public void SetStats(float health, float attack, float atk_range,bool isP1){ //Vie,Attaque,Portée, Joueur(P1 ou P2)
		unitType = setUnitType (health, attack, atk_range);
		hp = (health * manager.StatMultiplier);
		Debug.Log (ManagerObject);
		atk = (attack * manager.StatMultiplier);
		range = ((atk_range * manager.StatMultiplier) * MAX_RANGE);
		if(range < MIN_RANGE) range = MIN_RANGE;
		isGood = isP1;
	}
	
	int setUnitType(float H,float A,float R){ //Vérifie quel valeur est la plus grande parmis hp, Atk et range et renvoit la bon type
		if (H + A + R < weakThreshold) return 5; //Si les stats de l'unité sont trop faible (le joueur a manqué le CRAFT)
		if (H > A) {
			if (H > R) return 1;
			else if (H < R) return 3;
		}
		else if (A > H) {
			if (A > R) return 2;
			else if (A < R) return 3;
		}
		return 4;
	}
	
	void Shoot() {
		Debug.Log ("SHOTS FIRED ENEMY");
		//shoot_Projectile;
	}
	
	void Awake () {
		weakThreshold = 0.5f;
		ManagerObject = GameObject.Find("Game");
		manager = ManagerObject.GetComponent<GameManager>();
		layerMask = ~( (1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7));
	}
	
	void Update () {
		Debug.Log ("atk:" + atk);
		Debug.Log ("hp:" + hp);
		Debug.Log ("range:" + range);
		Debug.DrawRay (transform.position, new Vector3 (1, 0, 0),Color.green);
		if (Physics.Raycast (new Ray(transform.position, new Vector3 (-1, 0, 0)),out hit,range,layerMask)) {
			Shoot();
		}
	}
}
