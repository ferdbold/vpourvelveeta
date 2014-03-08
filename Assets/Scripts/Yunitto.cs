using UnityEngine;
using System.Collections;

public class Yunitto : MonoBehaviour {

	const int MAX_RANGE = 600;

	private int hp; // vie de l'unité
	private int atk; // attaque de l'unité
	private int range; //Portée de tir de l'unité 
	private int unitType; //Type d'unité (1:HP, 2:ATK, 3:Range, 4:Balanced, 5:Weak)
	private bool isGood; // Est-ce que l'unité appartient a la faction du haut(good) ou du bas(bad)

	private float weakThreshold; //Si les stats combinées d'un unité est inférieur au threshold, elle est faible.
	private float friendlyStatMultiplier;
	private RaycastHit hit;

	private GameObject ManagerObject;
	private GameManager manager; //Game Manager


	
	//Getters/Setters
	public int Hp 
	{ 
		get{return hp;}
		set{hp = value;}
	}
	public int Atk
	{
		get{return atk;}
		set{atk = value;}
	}
	public int Range
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

	Yunitto(float health, float attack, float atk_range,bool good){ //Vie,Attaque,Portée, Type d'unité(Good ou Bad), Joueur créant l'unit
		unitType = setUnitType (health, attack, atk_range);
		//Stats de base pour le joueur
			hp = (int)(health * friendlyStatMultiplier);
			atk = (int)(attack * friendlyStatMultiplier);
			range = (int)(atk_range * MAX_RANGE);
			isGood = good;
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

	}

	void Start () {
		weakThreshold = 0.5f;
		friendlyStatMultiplier = 10f;
		ManagerObject = GameObject.Find("GameManager");
		manager = ManagerObject.GetComponent<GameManager>();


	}

	void Update () {
		if (Physics.Raycast (new Ray(transform.position, new Vector3 (1, 0, 0)),out hit,(float)range)) {
		
			Shoot();
		}
	}
}
