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
	
	// Attributs
	private Transform _bunch;
	private Transform _enemyBunch;
	private Transform _crafting;
	private BunchBehaviour _bunchBehaviour;
	private Crafting _craftingBehaviour;

	private PlayerState _state;

	public GameObject unitsGood;
	public GameObject unitsBad;

	private GameObject otherPlayerObj;
	private Player otherPlayer;
	public Player OtherPlayer {
		get{ return otherPlayer;}
	}

	// Méthodes
	void Awake() {
		_bunch = transform.Find("Bunch");
		_enemyBunch = transform.Find("Enemies");

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
	}

	public void CreateEnemyYunitto(float hp = 0.2f, float atk = 0.4f, float range = 1f) {
		GameObject yuni = (GameObject)Instantiate(unitsBad, new Vector3(10, transform.position.y, transform.position.z), Quaternion.Euler (0, -90, 0));
		yuni.transform.parent = _enemyBunch;

		YunittoWiggle yunitto = (YunittoWiggle)yuni.GetComponent<YunittoWiggle>();
		yunitto.Start();
		yunitto.SetStats(hp, atk, range);
	}

	public void CreateYunitto(float hp = 0.2f, float atk = 0.4f, float range = 0.4f) {
		GameObject yuni = (GameObject)Instantiate(unitsGood, new Vector3(-10, transform.position.y, transform.position.z), Quaternion.Euler (0, 90, 0));
		yuni.transform.parent = _bunch;
		
		YunittoWiggle yunitto = (YunittoWiggle)yuni.GetComponent<YunittoWiggle>();
		yunitto.Start();
		yunitto.SetStats(hp, atk, range);
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

			if(Input.GetButtonDown (_player.attackInput)){
				YunittoWiggle[] units = (YunittoWiggle[])_player._bunch.GetComponentsInChildren<YunittoWiggle>();
				foreach (YunittoWiggle unit in units) {
					unit.AttackPlayer();
				}
			
			}
			if(Input.GetButtonDown (_player.craftInput)) {
				_player._state = new CraftingState(_player);
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

	public class CraftingState : PlayerState {
		// Constructeurs
		public CraftingState(Player player) : base(player) {
			_player._craftingBehaviour.BeginCrafting();
		}

		// Méthodes
		public override void Update() {
			if(Input.GetButtonDown(_player.craftInput)) {
				_player._craftingBehaviour.QuitCrafting();
				_player._state = new BattleState(_player);
			}
		}
	}
}
