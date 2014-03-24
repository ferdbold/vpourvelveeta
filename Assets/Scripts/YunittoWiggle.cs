/// <summary>
/// YunittoWiggle.cs
/// Gère le mouvement funky des yunittos
/// </summary>

using UnityEngine;
using System.Collections;

public class YunittoWiggle : MonoBehaviour {
	
	// Constantes
	private int DEATH_ANIM_LENGTH = 20;
	const int MAX_RANGE = 3;
	private float MIN_RANGE = 0.5f;
	const int BASE_ATK = 1;
	const int BASE_HP = 10;
	public float BASE_SPEED = 1.5f;

	// Attributs
	private float unitRange;
	private YunittoWiggle yunitto;
	private YunittoWiggle yunittoEnemy;
	public float _yVelocity;
	private Player _player;
	private Transform _cursor;
	private float _personalSpeed;
	private Transform _body;

	private float hp; // vie de l'unité
	private float atk; // attaque de l'unité
	private float range; //Portée de tir de l'unité 
	private float cooldown; //Peut selement attaquer si cette valeur est zero et moins.
	public int unitType; //Type d'unité (1:HP, 2:ATK, 3:Range, 4:Balanced, 5:Weak)
	private bool isGood; // Est-ce que l'unité appartient a la faction du haut(good) ou du bas(bad)
	
	private float weakThreshold; //Si les stats combinées d'un unité est inférieur au threshold, elle est faible.
	
	public LayerMask layerMask;
	private RaycastHit hit;
	private GameObject ManagerObject;
	private GameManager manager; //Game Manager
	private ProjectileManager projectileManager; //Script ProjectileManager dans ManagerObject

	private YunittoState _state;
	public YunittoState State
	{
		get{return _state;}
	}
	public bool isInAllyAttackState 
	{
		get{ return(State is AllyAttackState);}
	}

	// Attributs publics
	public float wiggleSpeed = 1F;
	public float leashLength = 10F;
	public float jumpChance = 0.005F;
	public float jumpHeight = 20F;
	public float jumpVar = 1.5F;
	public float gravity = 0.001F;

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
		_cursor = transform.parent.parent.FindChild("Cursor");
		_personalSpeed = Random.Range(1F, 3F);

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

		_body = transform.FindChild("Sphere001");
		if (_body == null) _body = transform.FindChild("Bor");

		switch(name) { // Set de LayerMask
		case "P1Army(Clone)": 
			layerMask = ~( (1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9) | (1 << 10) | (1 << 12) | (1 << 13) | (1 << 14)); //Si c'est le J1 , On remarque les collisions avec le j2
			break;
		case "P2Army(Clone)":
			layerMask = ~( (1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9) | (1 << 11) | (1 << 12) | (1 << 13) | (1 << 14));
			break;
		case "P1Minions(Clone)":
			layerMask = ~( (1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 10) | (1 << 11) | (1 << 12) | (1 << 13) | (1 << 14)); // <-- LAYERS a IGNORER !
			break;
		case "P2Minions(Clone)":
			layerMask = ~( (1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 9) | (1 << 10) | (1 << 11) | (1 << 12) | (1 << 13) | (1 << 14));
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
		// Les minions visent la position du bunch ennemi (le château) et l'armée vise le curseur
		switch(gameObject.tag) {
		case "Minion":
			float interest = ((-transform.parent.position.x - (0.4f*unitRange))) * wiggleSpeed; //ajouté le unit range pour que les ranges soient plus en arriere.
			float movement = Random.Range(interest-wiggleSpeed, interest+wiggleSpeed) * Time.deltaTime;
			transform.position = new Vector3(transform.position.x + movement, 
			                                 transform.position.y, 
			                                 transform.position.z);
			break;
		
		case "Army":
			float mov = ((_cursor.transform.position.x - transform.position.x) - (0.6f * unitRange) *_personalSpeed) * Time.deltaTime;
			mov = Random.Range(mov - wiggleSpeed, mov + wiggleSpeed);
			transform.Translate(0, 0, mov);
			break;
		}
	}

	public void Jump() {
		//Applique un saut si on est pas déja en saut
		float yunittoY = transform.localPosition.y;
		if (yunittoY == 0) 
			{
			_yVelocity = jumpHeight + Random.Range(-jumpVar, jumpVar);
			}
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
	public void checkHit() {

		int coefficient = 1;
		if (gameObject.tag == "Minion") coefficient = -1;
		Debug.DrawRay (transform.position, new Vector3 (coefficient * range, 0, 0),Color.red);
		if(Physics.Raycast (new Ray(transform.position, new Vector3 (coefficient, 0, 0)),out hit,(1.1f*range),layerMask)){
			GameObject target = hit.collider.gameObject;
			if (Mathf.Abs(target.transform.position.x - transform.position.x) >= 4*MIN_RANGE) {
				Shoot();
				cooldown = 0;
			}
			else {
				if(target.name == "Base"){
					Base baseScript =  (Base)target.GetComponent<Base>();
					baseScript.Hp -= atk;
				} else {
				YunittoWiggle yuni = (YunittoWiggle)target.transform.parent.gameObject.GetComponent<YunittoWiggle>();
				if (yuni != null) yuni.Hp -= atk;
				cooldown = 0;
				}
			}
			cooldown = 0;
		}
	}
	
	public void SetStats(float health, float attack, float atk_range){ //Vie,Attaque,Portée, Joueur(P1 ou P2)
		unitType = setUnitType (health, attack, atk_range); //On choisit le type du joueur selon les stats
		setUnitColor (unitType);

		// Stats des alliés
		if (gameObject.tag == "Army") {
			hp = BASE_HP +(health * 100 * manager.FriendlyStatMultiplier);
			atk = BASE_ATK +(attack * 50 * manager.FriendlyStatMultiplier);
			range = (atk_range * MAX_RANGE);
			if(range < MIN_RANGE) range = MIN_RANGE;
		}

		// Stats des ennemis
		else if (gameObject.tag == "Minion") {
			hp = (BASE_HP+(health * 100 * manager.StatMultiplier))*0.75f;
			atk = BASE_ATK+(attack * 50 * manager.StatMultiplier);
			range = ((atk_range) * MAX_RANGE);
			if(range < MIN_RANGE) range = MIN_RANGE;
			if(range > MAX_RANGE) range = MAX_RANGE;
		}
	}

	int setUnitType(float H,float A,float R){ //Vérifie quel valeur est la plus grande parmis hp, Atk et range et renvoit la bon type
		//Type d'unité (1:HP, 2:ATK, 3:Range, 4:Balanced, 5:Weak)
		if (H + A + R < weakThreshold) return 5; //Si les stats de l'unité sont trop faible (le joueur a manqué le CRAFT)
		if (H > A) {
			if (H > R) return 1;
			else if (H < R) return 3;
		}
		else if (A > H) {
			if (A > R) return 2;
			else if (A < R) return 3;
		}
		else if (A > R) {
			if (A > H) return 2;
			else if (A < H) return 1;
		}
		else if (R > A) {
			if (R > H) return 3;
			else if (R < H) return 1;
		}
		return 4;
	}

	void setUnitColor (int uType) {
		if (gameObject.tag == "Army") {
			BunchBehaviour parentBunch = (BunchBehaviour)transform.parent.gameObject.GetComponent<BunchBehaviour>();
			switch (uType) {
			case 1:
				_body.renderer.material = (Material)parentBunch.green;
				break;
			case 2:
				_body.renderer.material = (Material)parentBunch.red;
				break;
			case 3:
				_body.renderer.material = (Material)parentBunch.blue;
				break;
			case 4:
				_body.renderer.material = (Material)parentBunch.white;
				break;
			case 5:
				_body.renderer.material = (Material)parentBunch.brown;
				break;
			}
		} else if (gameObject.tag == "Minion") {
			EnemyBunch parentBunch = (EnemyBunch)transform.parent.GetComponent<EnemyBunch>();
			switch (uType) {
			case 1:
				_body.renderer.material = (Material)parentBunch.green;
				break;
			case 2:
				_body.renderer.material = (Material)parentBunch.red;
				break;
			case 3:
				_body.renderer.material = (Material)parentBunch.blue;
				break;
			case 4:
				_body.renderer.material = (Material)parentBunch.white;
				break;
			case 5:
				_body.renderer.material = (Material)parentBunch.brown;
				break;
			}
		}
	}
	
	void Shoot() {
		int direction = 1;
		if (gameObject.tag == "Minion") direction = -1;
		projectileManager.CreateProjectile(direction,atk,transform.position, gameObject.name,range); //On indique au projetileManager de créer un projectile (Direction,attaque du projectile,position de la création, a qui appartient le projectile)
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
			
			if(Mathf.Abs(hit.collider.transform.position.x - transform.position.x) < 4*MIN_RANGE)
			{
				YunittoWiggle yuni = (YunittoWiggle)hit.collider.gameObject.transform.parent.gameObject.GetComponent<YunittoWiggle> ();
				if(yuni != null) yuni.Hp -= atk;
				//projectileManager.CreateProjectile(1,atk,transform.position,_player.name=="P1");
			}
		}
		else 
		{
			projectileManager.CreateProjectile(1,atk,transform.position, gameObject.name,range);
			/*YunittoEnemy yuni = (YunittoEnemy)hit.collider.gameObject.GetComponent<YunittoEnemy> ();
			if(yuni != null) yuni.Hp -= atk;*/
		}
		
		cooldown = 0;
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
		public AllyMarchState(YunittoWiggle yunitto) : base(yunitto) {

			// Changer l'animation
			_yunitto.animation["Sk_Yunito_Rob_Walk"].time = Random.Range(0.0F, _yunitto.animation["Sk_Yunito_Rob_Walk"].length);
			_yunitto.animation.Play ("Sk_Yunito_Rob_Walk");
		}

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
		public AllyAttackState(YunittoWiggle yunitto) : base(yunitto) {
			// Les yunittos collent au sol lorsqu'ils attaquent
			_yunitto.transform.localPosition = new Vector3(_yunitto.transform.localPosition.x,
			                                               0, 
			                                               _yunitto.transform.localPosition.z);
		}
		
		// Méthodes
		public override void Update() {
			_yunitto.Move();

			if (!_yunitto.IsInCooldown) {
				
				// S'il n'y a plus de menace, on revient en mode march
				if (!_yunitto.CheckThreat()) {
					_yunitto._state = new AllyMarchState(_yunitto);
				}
				
				else {
					if(Mathf.Abs(_yunitto.hit.collider.transform.position.x - _yunitto.transform.position.x) > 4*_yunitto.MIN_RANGE) {
						// Changer l'animation
						if (_yunitto.animation.clip.name != "Sk_Yunito_Rob_attR") {
							_yunitto.animation.Play ("Sk_Yunito_Rob_attR");
						}

						// FIRE IN THE HOLE
						_yunitto.Shoot();
					}
					else {
						// Changer l'animation
						if (_yunitto.animation.clip.name != "Sk_Yunito_Rob_AttM") {
							_yunitto.animation.Play ("Sk_Yunito_Rob_AttM");
						}

						// HIT IT
							_yunitto.checkHit();
					}
				}
			}
		}
	}

	public class EnemyMarchState : YunittoState {

		// Constructeurs
		public EnemyMarchState(YunittoWiggle yunitto) : base(yunitto) {

			// Changer l'animation
			_yunitto.animation["Sk_Yunito_Rob_Walk"].time = Random.Range(0.0F, _yunitto.animation["Sk_Yunito_Rob_Walk"].length);
			_yunitto.animation.Play ("Sk_Yunito_Rob_Walk");
		}

		// Méthodes
		public override void Update() {
			_yunitto.Move();
			_yunitto.Hop();


			if (!_yunitto.IsInCooldown) {
				// S'il y a une menace, on passe en mode attaque

				if (_yunitto.CheckThreat()) {
					_yunitto._state = new EnemyAttackState(_yunitto);
				}
			}
		}
	}

	public class EnemyAttackState : YunittoState {
		
		// Constructeurs
		public EnemyAttackState(YunittoWiggle yunitto) : base(yunitto) {
			// Les yunittos collent au sol lorsqu'ils attaquent
			_yunitto.transform.localPosition = new Vector3(_yunitto.transform.localPosition.x,
			                                               0, 
			                                               _yunitto.transform.localPosition.z);

			// Changer l'animation
			_yunitto.animation.Play ("Sk_Yunito_Rob_AttM");
		}
		
		// Méthodes
		public override void Update() {
			if (!_yunitto.IsInCooldown) {

				// S'il n'y a plus de menace, on revient en mode march
				if (!_yunitto.CheckThreat()) {
					_yunitto._state = new EnemyMarchState(_yunitto);
				}

				else {
					_yunitto.checkHit();
					/*Debug.Log (hit);
					if (Mathf.Abs(hit.collider.transform.position.x - _yunitto.transform.position.x) >= 4*_yunitto.MIN_RANGE) {
						_yunitto.Shoot();
					}
					else {
						_yunitto.HitMelee(hit.collider.gameObject);
					}*/

				}
			}
		}
	}

	public class DeathState : YunittoState {

		// Attributs
		private int _framesElapsed = 0;

		// Constructeurs
		public DeathState(YunittoWiggle yunitto) : base(yunitto) {

			// Changer l'animation
			_yunitto.animation.Play ("Sk_Yunito_Rob_death");
		}
		
		// Méthodes
		public override void Update() {
			_framesElapsed++;

			if (_framesElapsed >= _yunitto.DEATH_ANIM_LENGTH) {
				Die();
			}
		}

		private void Die() {
			//if(_yunitto.gameObject.tag == "minion") {
				//if (_yunitto._player.gameObject.name == "P1") 	_yunitto._player._killCount++;
				//}		

			GameObject.Destroy(_yunitto.gameObject);

		}
	}
}
