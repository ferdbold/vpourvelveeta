/// <summary>
/// YunittoWiggle.cs
/// Gère le mouvement funky des yunittos
/// </summary>

using UnityEngine;
using System.Collections;

public class YunittoWiggle : MonoBehaviour {

	// Attributs
	public float _yVelocity;

	// Attributs publics
	public float wiggleSpeed ;
	public float leashLength;
	public float jumpChance ;
	public float jumpHeight;
	public float jumpVar;
	public float gravity;

	void Start () {
		_yVelocity = 0F;
		wiggleSpeed = 1;
		leashLength = 10;
		jumpChance = 0.005f;
		jumpHeight = 3;
		jumpVar = 1.5f;
		gravity = 0.3f;
	}

	void Update () {
		Move();
		Jump();
	}

	private void Move() {
		// Biaiser les limites de mouvement pour que le yunitto ne s'éloigne pas trop du bunc
		float interest = (-transform.localPosition.x/leashLength) * wiggleSpeed;
		float movement = Random.Range(interest-wiggleSpeed, interest+wiggleSpeed) * Time.deltaTime;
		transform.localPosition = new Vector3(transform.localPosition.x + movement, 
		                                      transform.localPosition.y, 
		                                      transform.localPosition.z);
	}
	
	private void Jump() {
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
}
