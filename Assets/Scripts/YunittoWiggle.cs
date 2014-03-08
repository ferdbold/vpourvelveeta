/// <summary>
/// YunittoWiggle.cs
/// Gère le mouvement funky des yunittos
/// </summary>

using UnityEngine;
using System.Collections;

public class YunittoWiggle : MonoBehaviour {

	// Constantes
	private int DEATH_ANIM_LENGTH = 15;

	// Attributs
	private float unitRange;
	private Yunitto yunitto;
	private YunittoEnemy yunittoEnemy;
	public float _yVelocity;

	private YunittoState _state;

	// Attributs publics
	public float wiggleSpeed = 1F;
	public float leashLength = 10F;
	public float jumpChance = 0.005F;
	public float jumpHeight = 3F;
	public float jumpVar = 1.5F;
	public float gravity = 0.3F;

	void Start () {
		_yVelocity = 0F;

		// S'il s'agit d'un allié
		if(gameObject.GetComponent<Yunitto>() != null) {
			_state = new AllyMarchState(this);

			yunitto = (Yunitto)gameObject.GetComponent<Yunitto>();
			unitRange = yunitto.Range;
		}

		// S'il s'agit d'un ennemi
		else {
			_state = new EnemyMarchState(this);

			yunittoEnemy = (YunittoEnemy)gameObject.GetComponent<YunittoEnemy>();
			unitRange = yunittoEnemy.Range;
		}
	}

	void Update () {
		_state.Update();
	}

	private void Move() {
		// Biaiser les limites de mouvement pour que le yunitto ne s'éloigne pas trop du bunc
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
		}
	}

	public class AllyAttackState : YunittoState {

		// Constructeurs
		public AllyAttackState(YunittoWiggle yunitto) : base(yunitto) {}
		
		// Méthodes
		public override void Update() {}
	}

	public class EnemyMarchState : YunittoState {

		// Constructeurs
		public EnemyMarchState(YunittoWiggle yunitto) : base(yunitto) {}

		// Méthodes
		public override void Update() {
			_yunitto.Move();
			_yunitto.Hop();
		}
	}

	public class EnemyAttackState : YunittoState {
		
		// Constructeurs
		public EnemyAttackState(YunittoWiggle yunitto) : base(yunitto) {}
		
		// Méthodes
		public override void Update() {}
	}

	public class DeathState : YunittoState {

		// Attributs
		private int _framesElapsed = 0;

		// Constructeurs
		public DeathState(YunittoWiggle yunitto) : base(yunitto) {}
		
		// Méthodes
		public override void Update() {
			_framesElapsed++;

			if (_framesElapsed == _yunitto.DEATH_ANIM_LENGTH) {
				GameObject.Destroy(_yunitto.gameObject);
			}
		}
	}
}
