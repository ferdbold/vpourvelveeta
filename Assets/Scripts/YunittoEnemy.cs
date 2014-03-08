using UnityEngine;
using System.Collections;

public class YunittoEnemy : MonoBehaviour {
	const int MAX_RANGE = 5;
	const float MIN_RANGE = 0.5f;
	const int BASE_ATK = 1;
	const int BASE_HP = 5;
	const float BASE_SPEED = 1f;

	private float hp; // vie de l'unité
	private float atk; // attaque de l'unité
	private float range; //Portée de tir de l'unité 
	private float cooldown; //Peut selement attaquer si cette valeur est zero et moins.
	private int unitType; //Type d'unité (1:HP, 2:ATK, 3:Range, 4:Balanced, 5:Weak)
	private bool isGood; // Est-ce que l'unité appartient a la faction du haut(good) ou du bas(bad)
	private bool onCooldown;

	
	private float weakThreshold; //Si les stats combinées d'un unité est inférieur au threshold, elle est faible.

	private LayerMask layerMask;
	private RaycastHit hit;
	private GameObject ManagerObject; //Objet GameManager
	private GameManager manager; //Game Manager scripts
	private ProjectileManager projectileManager;

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
	
	public void SetStats(float health, float attack, float atk_range){ //Vie,Attaque,Portée, Joueur(P1 ou P2)
		unitType = setUnitType (health, attack, atk_range); //On choisit le type du joueur selon les stats (TODO : Changé la couleur du modele en fonction de l'unitType)
		isGood = !(transform.parent.parent.gameObject.name == "P1"); //on vérifie si le parent est P1 ou P2
		//Stats des ennemis
		hp = BASE_HP+(health * 100 * manager.StatMultiplier);
		atk = BASE_ATK+(attack * 100 * manager.StatMultiplier);
		range = ((atk_range * manager.StatMultiplier) * MAX_RANGE);
		if(range < MIN_RANGE) range = MIN_RANGE;
		if(range > MAX_RANGE) range = MAX_RANGE;
		//On fait les layerMask ici puisque c'est le moment ou on object le joueur a qui appartient l'unité
		if(isGood) {
			gameObject.layer = 10; //On le met a la layer P1
			layerMask = ~( (1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 10)| (1 << 11)); //Si c'est le J1 , On remarque les collisions avec le j2
		}
		else {
			gameObject.layer = 11; //On le met a la layer P1
			layerMask = ~( (1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 9) | (1 << 11)); //Si c'est le J2 , On remarque les collisions avec le j1
		}
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
		projectileManager.CreateProjectile(-1,atk,transform.position,isGood); //On indique au projetileManager de créer un projectile (Direction,attaque du projectile,position de la création, a qui appartient le projectile)
		onCooldown = true;
		cooldown = BASE_SPEED;
		
	}
	void HitMelee(GameObject target) {
		Yunitto yuni = target.GetComponent<Yunitto> ();  //On fait atk dégats a l'ennemi touché
		yuni.Hp -= atk;
		onCooldown = true;
		cooldown = BASE_SPEED;
		Debug.Log ("enemy melee");
	}
	
	void Awake () {
		weakThreshold = 0.5f; //si le total est en dessous du threshold. l'unité est faible.
		onCooldown = false;
		//Get Objects
		ManagerObject = GameObject.Find("Game");
		manager = ManagerObject.GetComponent<GameManager>();
		projectileManager = ManagerObject.GetComponent<ProjectileManager>();
	}
	
	void Update () {
		if(!onCooldown) {
			Debug.DrawRay (transform.position, new Vector3 (-range, 0, 0),Color.green);
			if (Physics.Raycast (new Ray(transform.position, new Vector3 (-1, 0, 0)),out hit,range,layerMask)) {
				if(Mathf.Abs(hit.collider.transform.position.x - transform.position.x) > 2*MIN_RANGE) Shoot();
				else HitMelee(hit.collider.gameObject);
			}
		} else if(cooldown > 0) {
			cooldown -= (1*Time.deltaTime);
		} else onCooldown = false;
		if (hp <= 0) {
			Destroy (gameObject);
			if(isGood) Debug.Log ("Player2's army Brutally Murdered Player1's minions");
			else Debug.Log ("Player1's army Brutally Murdered Player2's minions");
		}
	}
}
