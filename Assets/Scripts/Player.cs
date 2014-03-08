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
	private BunchBehaviour _bunchBehaviour;
	private Crafting _craftingBehaviour;

	private PlayerState _state;

	public GameObject unitsGood;
	public GameObject unitsBad;
	private Yunitto yunitto;
	private YunittoEnemy yunittoEnemy;

	// Méthodes
	void Start() {
		_bunch = transform.Find("Bunch");
		_enemyBunch = transform.Find("Enemies");
		_crafting = transform.Find("Crafting");

		_bunchBehaviour = (BunchBehaviour)_bunch.GetComponent<BunchBehaviour>();
		_craftingBehaviour = (Crafting)_crafting.GetComponent<Crafting>();

		yunitto = unitsGood.GetComponent<Yunitto>();
		yunittoEnemy = unitsBad.GetComponent<YunittoEnemy>();

		_state = new BattleState(this);
	}

	void Update() {
		_state.Update();
	}

	public void CreateEnemyYunitto(float hp = 0.2f, float atk = 0.4f, float range = 0.4f) {
		GameObject yuni = (GameObject)Instantiate(unitsBad, new Vector3(10, transform.position.y, transform.position.z), transform.rotation);
		yuni.transform.parent = _enemyBunch;
		
		YunittoEnemy yunitto = (YunittoEnemy)yuni.GetComponent<YunittoEnemy>();
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
			if(Input.GetButton (_player.attackInput)){
				Yunitto[] units = (Yunitto[])_player._bunch.GetComponentsInChildren<Yunitto>();
				foreach (Yunitto unit in units) {
					//yuni.AttackPlayer();
				}
			
			}
			if(Input.GetButtonDown (_player.craftInput)) {
				_player._state = new CraftingState(_player);
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
