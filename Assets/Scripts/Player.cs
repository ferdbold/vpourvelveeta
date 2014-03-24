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
	public string JumpInput;
	public float MAX_attackMeter = 25f;
	public int numberOfAllies = 0;
	public int numberOfEnemies = 0;
	
	// Attributs
	private float ScreenFlashCountdown = 0f;
	private float ScreenFlashCooldown = 4f;
	private float attackMeter;
	private float craftingTransitionTime = 0.6f;
	private Transform _bunch;
	private Transform _enemyBunch;
	private Transform _crafting;
	private BunchBehaviour _bunchBehaviour;
	private Crafting _craftingBehaviour;

	private PlayerState _state;

	public GameObject unitsGood;
	public GameObject unitsBad;
	public GameObject unitEmphasisYellow;
	public GameObject unitEmphasisRed;
	public GameObject unitEmphasisBlue;
	public GameObject unitEmphasisWhite;
	public GameObject unitEmphasisBlack;
	public GameObject ScreenFlashTexture;

	private GameObject otherPlayerObj;
	private Player otherPlayer;
	// GETS/SETS
	public Player OtherPlayer {
		get{ return otherPlayer;}
	}
	public float AttackMeter {
		get{ return attackMeter;}
		set{ attackMeter = value;}
	}
	public float CraftingTransitionTime {
		get{ return craftingTransitionTime;}
		set{ craftingTransitionTime = value;}
	}

	// Méthodes
	void Awake() {
		_bunch = transform.Find("Bunch");
		_enemyBunch = transform.Find("Enemies");
		ScreenFlashTexture = (GameObject.Find("Flashtext"));

		attackMeter = 0;

	}

	void Start() {
		_crafting = transform.Find("Crafting");
		if (name == "P1") {
			Transform otherPlayerT = transform.parent.transform.Find ("P2");
			otherPlayerObj = otherPlayerT.gameObject;
		}
		else {
			Transform otherPlayerT = transform.parent.transform.Find ("P1");
			otherPlayerObj = otherPlayerT.gameObject;
		}
		otherPlayer = (Player)otherPlayerObj.GetComponent<Player> ();

		_bunchBehaviour = (BunchBehaviour)_bunch.GetComponent<BunchBehaviour>();
		_craftingBehaviour = (Crafting)_crafting.GetComponent<Crafting>();


		_state = new BattleState(this);
	}

	void Update() {
		_state.Update();
		//Make screen flash red if player is about to die
		ScreenFlashCountdown += Time.deltaTime;
		if(ScreenFlashCountdown > ScreenFlashCooldown) {
			if(2*numberOfAllies < numberOfEnemies){
				StartCoroutine(FlashScreen(ScreenFlashTexture));
				ScreenFlashCountdown = 0;
			}
		}
	}

	//Fade and FlashScreen functions
	private IEnumerator Fade (float start, float end, float length, GameObject currentObject) { // Start Transparency, End Transparency, Lenght of Fading, Object to Fade
		//if (currentObject.guiTexture.color.a == start){
			Color col = currentObject.guiTexture.color;
			for (float i = 0.0f; i < 1.0f; i += Time.deltaTime*(1f/length)) { //for length of fade
				col.a = (float)Mathf.Lerp(start, end, i); //lerp transparency
				currentObject.guiTexture.color = col;
				yield return null;
				col.a = 0;
				currentObject.guiTexture.color = col; // end lerp
			} 	
		//}	
	}
	public IEnumerator FlashScreen(GameObject ScreenFlashTexture){
		StartCoroutine(Fade (0f, 0.5f, 2.0f, ScreenFlashTexture)); //Make flash appear
		yield return new WaitForSeconds(2f); // Wait x sec
		StartCoroutine(Fade (0.5f, 0f, 2.0f, ScreenFlashTexture)); //Make flash dissapear
	}

	public void CreateEnemyYunitto(float hp = 0.2f, float atk = 0.4f, float range = 0.4f) {
		GameObject yuni = (GameObject)Instantiate(unitsBad, new Vector3(10, transform.position.y, transform.position.z), Quaternion.Euler (0, -90, 0));
		yuni.transform.parent = _enemyBunch;

		YunittoWiggle yunitto = (YunittoWiggle)yuni.GetComponent<YunittoWiggle>();
		yunitto.Start();
		yunitto.SetStats(hp, atk, range);

		numberOfEnemies++;
	}

	public void CreateYunitto(float hp = 0.2f, float atk = 0.4f, float range = 0.4f) {
		GameObject yuni = (GameObject)Instantiate(unitsGood, new Vector3(-10, transform.position.y, transform.position.z), Quaternion.Euler (0, 90, 0));
		GameObject unitEmphasisInstance;
		yuni.transform.parent = _bunch;
		YunittoWiggle yunitto = (YunittoWiggle)yuni.GetComponent<YunittoWiggle>();
		yunitto.Start();
		yunitto.SetStats(hp, atk, range);
		switch(yunitto.unitType) {
		case 1 : 
			unitEmphasisInstance = (GameObject)Instantiate(unitEmphasisYellow, yuni.transform.position, yuni.transform.rotation);
			break;
		case 2 : 			
			unitEmphasisInstance = (GameObject)Instantiate(unitEmphasisRed, yuni.transform.position, yuni.transform.rotation);
			break;
		case 3 : 			
			unitEmphasisInstance = (GameObject)Instantiate(unitEmphasisBlue, yuni.transform.position, yuni.transform.rotation);
			break;
		case 4 : 			
			unitEmphasisInstance = (GameObject)Instantiate(unitEmphasisWhite, yuni.transform.position, yuni.transform.rotation);
			break;
		default : 			
			unitEmphasisInstance = (GameObject)Instantiate(unitEmphasisBlack, yuni.transform.position, yuni.transform.rotation);
			break;
		}
		unitEmphasisInstance.transform.Rotate (new Vector3 (0, 90, 0)); //Rotate le sprite de 90 degree sinon il est invisible vu de la camera
		unitEmphasisInstance.transform.Translate (new Vector3 (0f, 0.50f, -0.5f)); //centre l'anneau sur le model (d'ou le 0.50f)
		unitEmphasisInstance.transform.parent = yuni.transform;

		numberOfAllies++;
	}
	// Accesseurs
	public Transform Bunch {
		get { return _bunch; }
	}

	public Transform EnemyBunch {
		get { return _enemyBunch; }
	}


	/**
	 * STATE MACHINE
	 **/
	public abstract class PlayerState {
		// Attributs
		protected Player _player;

		// Constructeurs
		private PlayerState() {}
		public PlayerState(Player player) {
			_player = player;
		}

		// Méthodes
		public abstract void Update();
	}
	
	public class BattleState : PlayerState {
		// Constructeurs
		public BattleState(Player player) : base(player) {}

		// Méthodes
		public override void Update() {
			_player._bunchBehaviour.Move();
			_player.AttackMeter -= 30f * Time.deltaTime;
			if(_player.AttackMeter <= 0f) _player.AttackMeter = 0f;


			if(Input.GetButtonDown (_player.attackInput)){
				_player.AttackMeter += 10f;
				if(_player.AttackMeter >= _player.MAX_attackMeter) _player.AttackMeter = _player.MAX_attackMeter;
				YunittoWiggle[] units = (YunittoWiggle[])_player._bunch.GetComponentsInChildren<YunittoWiggle>();
				foreach (YunittoWiggle unit in units) {
					unit.AttackPlayer();
				}
			
			}
			if(Input.GetButtonDown (_player.craftInput)) {
				_player._state = new CraftingStateBegin(_player);
			}
			if(Input.GetButtonDown (_player.JumpInput)) {
				Debug.Log("Jumped");
				YunittoWiggle[] units = (YunittoWiggle[])_player.Bunch.GetComponentsInChildren<YunittoWiggle>();
				foreach (YunittoWiggle unit in units){
					unit.Jump();
				}
			}
		}
	}

	public class CraftingStateBegin : PlayerState {
		float timer;
		//Constructeurs
		public CraftingStateBegin(Player player) : base(player) {
			_player._craftingBehaviour.BeginCraftingSession();
			timer = 0f;
			_player.AttackMeter = 0;
		}
		// Méthodes
		public override void Update() {
			timer += (1 * Time.deltaTime);
			if(timer >= _player.CraftingTransitionTime) {
				_player._state = new CraftingState(_player);
			}
		}
	}
	public class CraftingStateStop : PlayerState {
		float timer;
		//Constructeurs
		public CraftingStateStop(Player player) : base(player) {
			_player._craftingBehaviour.EndCraftingAnimationStart();
			timer = 0f;
		}
		// Méthodes
		public override void Update() {
			timer += 1 * Time.deltaTime;
			if(timer >= _player.CraftingTransitionTime) {
				_player._craftingBehaviour.EndCraftingSession();
				_player._state = new BattleState(_player);
			}
		}
	}




	public class CraftingState : PlayerState {
		// Constructeurs
		public CraftingState(Player player) : base(player) {
			_player._craftingBehaviour.BeginCrafting();
		}

		// Méthodes
		public override void Update() {
			if(Input.GetButtonDown(_player.craftInput)) {
				_player._craftingBehaviour.QuitCrafting();
				_player._state = new CraftingStateStop(_player);
			}
		}
	}
}
