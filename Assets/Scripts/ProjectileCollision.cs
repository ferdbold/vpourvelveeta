using UnityEngine;
using System.Collections;

public class ProjectileCollision : MonoBehaviour {
	private float atk;
	private string unitName;
	private float range;
	private RaycastHit hit;

	private float angle;
	private float xSpeed;
	private float ySpeed;
	private int direction;

	private GameObject target;

	Shoot_Projectile shoot_Projectile;
	LayerMask layerMask;
	// Use this for initialization
	void Start () {
		range = 0.5f;

		shoot_Projectile = (Shoot_Projectile)transform.parent.GetComponent<Shoot_Projectile> ();
		direction = shoot_Projectile.direction;
		atk = shoot_Projectile.Dmg;
		unitName = shoot_Projectile.UnitName;
		xSpeed = shoot_Projectile._xspeed;
		switch(unitName) {
		case "P1Army(Clone)": 
			layerMask = ~( (1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9) | (1 << 10)); //Si c'est le J1 , On remarque les collisions avec le j2
			break;
		case "P2Army(Clone)":
			layerMask = ~( (1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9) | (1 << 11));
			break;
		case "P1Minions(Clone)":
			layerMask = ~( (1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 10) | (1 << 11)); // <-- LAYERS a IGNORER !
			break;
		case "P2Minions(Clone)":
			layerMask = ~( (1 << 0) |(1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 9) | (1 << 10) | (1 << 11));
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		ySpeed = shoot_Projectile._yspeed;
		angle = (Mathf.Atan(ySpeed / xSpeed))*(360.0f/(2.0f*Mathf.PI)) ; //Calcul de l'angle en radian et conversion en degré
		if (angle < 0) 	 angle+=360;
		transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);


		Debug.DrawRay (transform.position,transform.right*direction*range,Color.cyan);
		if (Physics.Raycast (new Ray (transform.position, transform.right*direction), out hit, range, layerMask)) {
			target = hit.collider.gameObject;

			if(target.name == "Base"){
				Base baseScript =  (Base)target.GetComponent<Base>();
				baseScript.Hp -= atk;
				Debug.Log (baseScript.Hp); 
			}
			else {
				YunittoWiggle yuni = (YunittoWiggle)target.transform.parent.gameObject.GetComponent<YunittoWiggle>();
				Debug.Log(target);
				if (yuni != null) yuni.Hp -= atk;
			}

			Destroy (gameObject.transform.parent.gameObject);
		}

		if(unitName == "P1Army(Clone)" || unitName == "P2Minions(Clone)" )
		{
			if(transform.parent.position.y<=0f) //La constante est sujet aux changements!
				Destroy (gameObject.transform.parent.gameObject);
		}
		else
		{
			if(transform.parent.position.y<=-4.8f)//La constante est sujet aux changements!
				Destroy (gameObject.transform.parent.gameObject);
		}
	}
}
