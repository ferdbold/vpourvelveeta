﻿/// <summary>
/// YunittoWiggle.cs
/// Gère le mouvement funky des yunittos
/// </summary>

using UnityEngine;
using System.Collections;

public class YunittoWiggle : MonoBehaviour {
	
	// Constantes
	private int DEATH_ANIM_LENGTH = 15;
	const int MAX_RANGE = 5;
	private float MIN_RANGE = 0.5f;
	const int BASE_ATK = 1;
	const int BASE_HP = 5;
	const float BASE_SPEED = 1f;

	// Attributs
	private float unitRange;
	private YunittoWiggle yunitto;
	private YunittoWiggle yunittoEnemy;
	public float _yVelocity;
	private Player _player;

	private float hp; // vie de l'unité
	private float atk; // attaque de l'unité
	private float range; //Portée de tir de l'unité 
	private float cooldown; //Peut selement attaquer si cette valeur est zero et moins.
	private int unitType; //Type d'unité (1:HP, 2:ATK, 3:Range, 4:Balanced, 5:Weak)
	private bool isGood; // Est-ce que l'unité appartient a la faction du haut(good) ou du bas(bad)
	
	private float weakThreshold; //Si les stats combinées d'un unité est inférieur au threshold, elle est faible.
	
	public LayerMask layerMask;
	private RaycastHit hit;
	private GameObject ManagerObject;
	private GameManager manager; //Game Manager
	private ProjectileManager projectileManager; //Script ProjectileManager dans ManagerObject

	private YunittoState _state;

	// Attributs publics
	public float wiggleSpeed = 1F;
	public float leashLength = 10F;
	public float jumpChance = 0.005F;
	public float jumpHeight = 3F;
	public float jumpVar = 1.5F;
	public float gravity = 0.3F;

	/**
	 * MÉTHODES
	 **/
	void Awake () {
		weakThreshold = 0.5f; //si le total est en dessous du threshold. l'unité est faible.
		//Get Objects
		ManagerObject = GameObject.Find("Game");
		manager = (GameManager)ManagerObject.GetComponent<GameManager>();
		projectileManager = (ProjectileManager)ManagerObject.GetComponent<ProjectileManager>();
	}

	public void Start () {
		_yVelocity = 0F;

		_player = (Player)transform.parent.parent.GetComponent<Player>();

		switch(gameObject.tag) {
			// S'il s'agit d'un allié
			case "Army":
				_state = new AllyMarchState(this);

				yunitto = (YunittoWiggle)gameObject.GetComponent<YunittoWiggle>();
				unitRange = yunitto.Range;
				break;

			// S'il s'agit d'un ennemi
			case "Minion":
				_state = new EnemyMarchState(this);

				yunittoEnemy = (YunittoWiggle)gameObject.GetComponent<YunittoWiggle>();
				unitRange = yunittoEnemy.Range;
				break;
		}
	}

	void Update () {
		_state.Update();

		cooldown += Time.deltaTime;

		if (hp < 0 && !(_state is DeathState)) {
			_state = new DeathState(this);
		}
	}

	private void Move() {
		// Biaiser les limites de mouvement pour que le yunitto ne s'éloigne pas trop du bunch
		float interest = ((-transform.localPosition.x - (0.6f*unitRange))/leashLength) * wiggleSpeed; //ajouté le unit range pour que les ranges soient plus en arriere.
		float movement = Random.Range(interest-wiggleSpeed, interest+wiggleSpeed) * Time.deltaTime;
		transform.localPosition = new Vector3(transform.localPosition.x + movement, 
		                                      transform.localPosition.y, 
		                                      transform.localPosition.z);
	}
	
	private void Hop() {
		// Appliquer un saut si ça proc et qu'on est pas déjà dans un saut
		float yunittoY = transform.localPosition.y;

		if (yunittoY == 0 && Random.Range(0F, 1F) < jumpChance) {
			_yVelocity = jumpHeight + Random.Range(-jumpVar, jumpVar);
		}

		// Appliquer la gravité
		_yVelocity -= gravity;

		// Calculer le nouveau Y
		yunittoY = transform.localPosition.y + _yVelocity * Time.deltaTime;
		if (yunittoY < 0) {
			yunittoY = 0;
			_yVelocity = 0;
		}

		// Appliquer le mouvement du saut
		transform.localPosition = new Vector3(transform.localPosition.x, 
		                                      yunittoY, 
		                                      transform.localPosition.z);
	}

	/// <summary>
	/// Retourne si oui ou non il y a une menace pour le Yunnito, et stocke le hit s'il y en a une.
	/// </summary>
	/// <returns><c>true</c>, if threat was gotten, <c>false</c> otherwise.</returns>
	public bool CheckThreat() {
		int coefficient = 1;
		if (gameObject.tag == "Minion") coefficient = -1;

		Debug.DrawRay (transform.position, new Vector3 (coefficient * range, 0, 0),Color.green);
		return Physics.Raycast (new Ray(transform.position, new Vector3 (coefficient, 0, 0)), out hit, range, layerMask);
	}
	
	public void SetStats(float health, float attack, float atk_range){ //Vie,Attaque,Portée, Joueur(P1 ou P2)
		unitType = setUnitType (health, attack, atk_range); //On choisit le type du joueur selon les stats (TODO : Changé la couleur du modele en fonction de l'unitType)
		setUnitColor (unitType);

		// Stats des alliés
		if (gameObject.tag == "Army") {
			hp = BASE_HP +(health * manager.FriendlyStatMultiplier);
			atk = BASE_ATK +(attack * manager.FriendlyStatMultiplier);
			range = (atk_range * MAX_RANGE);
			if(range < MIN_RANGE) range = MIN_RANGE;
		}

		// Stats des ennemis
		else if (gameObject.tag == "Minion") {
			hp = BASE_HP+(health * 100 * manager.StatMultiplier);
			atk = BASE_ATK+(attack * 100 * manager.StatMultiplier);
			range = ((atk_range * manager.StatMultiplier) * MAX_RANGE);
			if(range < MIN_RANGE) range = MIN_RANGE;
			if(range > MAX_RANGE) range = MAX_RANGE;
		}

		//On fait les layerMask ici puisque c'est le moment ou on object le joueur a qui appartient l'unité
		if(_player.name == "P1") { //On le met a la layer P1
			if(gameObject.tag == "Army") {
				gameObject.layer = 8;
				layerMask = ~((1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9) | (1 << 10));
			} else if (gameObject.tag == "Minion") {
				gameObject.layer = 11;
				layerMask = ~((1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 9) | (1 << 10)| (1 << 11));
			}

		}
		else { //On le met a la layer P2
			if (gameObject.tag == "Army") {
				gameObject.layer = 9;
				layerMask = ~((1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9) | (1 << 11));
			} else if (gameObject.tag == "Minion") {
				gameObject.layer = 10;
				layerMask = ~((1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 10) | (1 << 11));
			}
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

	void setUnitColor (int uType) {
		if (gameObject.tag == "Army") {
			BunchBehaviour bunchBehaviour = (BunchBehaviour)transform.parent.gameObject.GetComponent<BunchBehaviour>();
			switch (uType) {
			case 1:
				gameObject.renderer.material = (Material)bunchBehaviour.green;
				break;
			case 2:
				gameObject.renderer.material= (Material)bunchBehaviour.red;
				break;
			case 3:
				gameObject.renderer.material = (Material)bunchBehaviour.blue;
				break;
			case 4:
				gameObject.renderer.material= (Material)bunchBehaviour.white;
				break;
			case 5:
				gameObject.renderer.material = (Material)bunchBehaviour.brown;
				break;
			}
		}

		else if (gameObject.tag == "Minion") {
			switch (uType) {
			case 1:
				gameObject.renderer.material.color = Color.green;
				break;
			case 2:
				gameObject.renderer.material.color = Color.red;
				break;
			case 3:
				gameObject.renderer.material.color = Color.blue;
				break;
			case 4:
				gameObject.renderer.material.color = Color.white;
				break;
			case 5:
				gameObject.renderer.material.color = Color.black;
				break;
			}
		}
	}
	
	void Shoot() {
		int direction = 1;
		if (gameObject.tag == "Minion") direction = -1;

		projectileManager.CreateProjectile(direction,atk,transform.position, (_player.gameObject.name == "P1")); //On indique au projetileManager de créer un projectile (Direction,attaque du projectile,position de la création, a qui appartient le projectile)
		cooldown = 0;
	}

	void HitMelee(GameObject target) {
		if (target.name == "Base") {
			Base baseScript =  (Base)target.GetComponent<Base>();
			baseScript.Hp -= atk;
		} else {
			YunittoWiggle yuni = (YunittoWiggle)target.GetComponent<YunittoWiggle>();  //On fait atk dégats a l'ennemi touché
			if (yuni != null) {
				yuni.Hp -= atk;
				cooldown = 0;
				Debug.Log ("pif paf pif");
			}
		}
	}

	/**
	 * ACCESSEURS
	 **/
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
	public bool IsInCooldown {
		get { return (cooldown < BASE_SPEED); }
	}

	public void AttackPlayer () {
		if (Physics.Raycast (new Ray(transform.position, new Vector3 (1, 0, 0)),out hit,range,layerMask)) 
		{
			
			if(Mathf.Abs(hit.collider.transform.position.x - transform.position.x) < 2*MIN_RANGE)
			{
				YunittoEnemy yuni = (YunittoEnemy)hit.collider.gameObject.GetComponent<YunittoEnemy> ();
				if(yuni != null) yuni.Hp -= atk;
				//projectileManager.CreateProjectile(1,atk,transform.position,isGood);
			}
		}
		else 
		{
			projectileManager.CreateProjectile(1,atk,transform.position, (_player.gameObject.name == "P1"));
			/*YunittoEnemy yuni = (YunittoEnemy)hit.collider.gameObject.GetComponent<YunittoEnemy> ();
			if(yuni != null) yuni.Hp -= atk;*/
		}
		
		cooldown -= 0.05f;
	}

	/**
	 * STATE MACHINE
	 **/
	public abstract class YunittoState {

		// Attributs
		protected YunittoWiggle _yunitto;

		// Constructeurs
		private YunittoState() {}
		public YunittoState(YunittoWiggle yunitto) {
			_yunitto = yunitto;
		}

		// Méthodes
		public abstract void Update();
	}

	public class AllyMarchState : YunittoState {

		// Constructeurs
		public AllyMarchState(YunittoWiggle yunitto) : base(yunitto) {}

		// Méthodes
		public override void Update() {
			_yunitto.Move();
			_yunitto.Hop();

			if (!_yunitto.IsInCooldown) {
				// S'il y a une menace, on passe en mode attaque
				if (_yunitto.CheckThreat()) {
					_yunitto._state = new AllyAttackState(_yunitto);
				}
			}
		}
	}

	public class AllyAttackState : YunittoState {

		// Constructeurs
		public AllyAttackState(YunittoWiggle yunitto) : base(yunitto) {}
		
		// Méthodes
		public override void Update() {
			if (!_yunitto.IsInCooldown) {
				
				// S'il n'y a plus de menace, on revient en mode march
				if (!_yunitto.CheckThreat()) {
					_yunitto._state = new AllyMarchState(_yunitto);
				}
				
				else {
					if(Mathf.Abs(_yunitto.hit.collider.transform.position.x - _yunitto.transform.position.x) > 2*_yunitto.MIN_RANGE)
						_yunitto.Shoot();
					else
						_yunitto.HitMelee(_yunitto.hit.collider.gameObject);
				}
			}
		}
	}

	public class EnemyMarchState : YunittoState {

		// Constructeurs
		public EnemyMarchState(YunittoWiggle yunitto) : base(yunitto) {}

		// Méthodes
		public override void Update() {
			_yunitto.Move();
			_yunitto.Hop();


			if (!_yunitto.IsInCooldown) {
				// S'il y a une menace, on passe en mode attaque
				//Debug.Log(_yunitto.CheckThreat());
				if (_yunitto.CheckThreat()) {
					_yunitto._state = new EnemyAttackState(_yunitto);
				}
			}
		}
	}

	public class EnemyAttackState : YunittoState {
		
		// Constructeurs
		public EnemyAttackState(YunittoWiggle yunitto) : base(yunitto) {
			Debug.Log ("Enter attack state");
		}
		
		// Méthodes
		public override void Update() {
			if (!_yunitto.IsInCooldown) {

				// S'il n'y a plus de menace, on revient en mode march
				if (!_yunitto.CheckThreat()) {
					_yunitto._state = new EnemyMarchState(_yunitto);
				}

				else {
					if(Mathf.Abs(_yunitto.hit.collider.transform.position.x - _yunitto.transform.position.x) > 2*_yunitto.MIN_RANGE)
						_yunitto.Shoot();
					else
						_yunitto.HitMelee(_yunitto.hit.collider.gameObject);
				}
			}
		}
	}

	public class DeathState : YunittoState {

		// Attributs
		private int _framesElapsed = 0;

		// Constructeurs
		public DeathState(YunittoWiggle yunitto) : base(yunitto) {

		}
		
		// Méthodes
		public override void Update() {
			_framesElapsed++;

			if (_framesElapsed >= _yunitto.DEATH_ANIM_LENGTH) {
				Die();
			}
		}

		private void Die() {
			Debug.Log ("That die tho");
			GameObject.Destroy(_yunitto.gameObject);
			if(_yunitto.isGood) Debug.Log ("Player2's army Brutally Murdered Player1's minions");
			else 				Debug.Log ("Player1's army Brutally Murdered Player2's minions");
		}
	}
}
