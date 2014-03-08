using UnityEngine;
using System.Collections;


public class Shoot_Projectile : MonoBehaviour {
	
	public int direction=1; 					//Direction du tir, elle ÉGALE A 1 POUR DES TESTS SEULEMENT
	public float _distance; 					//Distance entre deux unités
	public float _angleprojectile; 				//Angle de la de la vitesse du projectile
	public float _angletir=45.0f;						//Angle du  tir du projectile
	public float _xspeed; 						//Variable vitesse verticale du projectile
	public float _yspeed; 						//Variable vitesse horizontale du projectile
	public float _speed; 						//Variable vitesse du projectile
	public const float _speedreduction = 5.0f;  //Constante de reduction de vitesse

	//Added by Alex
	private float dmg;
	public float Dmg
	{
		get {return dmg;}
		set {dmg = value;}
	}
	private bool isP1;
	public bool IsP1
	{
		get {return isP1;}
		set {isP1 = value;}
	}
	public GameObject model;


	
	
	// Use this for initialization
	void Start () {
		_speed = 2.0f + Random.Range (0, 3); 
		_yspeed = Mathf.Sin (_angletir) * _speed;
		_xspeed = Mathf.Cos (_angletir) * _speed * direction;
		/*_xspeed  = 5.0f*direction; 				//Envoie le projectile vers la direction droite (positive) ou gauche (négative)
		_yspeed  = 5.0f; 						//Vitesse initiale de tir en hauteur*/

	}
	
	// Update is called once per frame
	void Update () {


		transform.Translate(new Vector3(_xspeed,_yspeed,0.0f)*Time.deltaTime); // Mouvement de l'objet selon un vecteur [x,y]
		/*_angleprojectile = (Mathf.Atan(_yspeed / _xspeed))*(360.0f/(2.0f*Mathf.PI)) ; //Calcul de l'angle en radian et conversion en degré
		_angleprojectile -= 100*Time.deltaTime;
		if (_angleprojectile < 0) 													//Section mise en commentaire car elle cause des problemes au niveau de l'objet
			{
				_angleprojectile+=360; 
			}*/
		//transform.eulerAngles = new Vector3(0.0f, 0.0f, _angleprojectile);
		_yspeed  -=  _speedreduction*Time.deltaTime;
		//_speed = Mathf.Pow((_xspeed * _xspeed + _yspeed * _yspeed), 0.5f); //USELESS

	}


}