using UnityEngine;
using System.Collections;


public class Shoot_Projectile : MonoBehaviour {
	
	public int direction; 					//Direction du tir, elle ÉGALE A 1 POUR DES TESTS SEULEMENT
	public float _distance; 					//Distance entre deux unités
	public float _angleprojectile; 				//Angle de la de la vitesse du projectile
	public float _angletir=45.0f;				//Angle du  tir du projectile
	public float _xspeed; 						//Variable vitesse verticale du projectile
	public float _yspeed; 						//Variable vitesse horizontale du projectile
	public float _speed; 						//Variable vitesse du projectile
	public const float _speedreduction = 5.0f;  //Constante de reduction de vitesse
	public float _angle;

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
	private string unitName;
	public string UnitName{
		get { return unitName;}
		set { unitName = value;}
	}
	public GameObject model;
	public float unitRange;


	
	
	// Use this for initialization
	void Start () {
		_speed = (2.0f+(0.5f*unitRange)) + Random.Range (0, 3);	 
		_yspeed = Mathf.Sin (_angletir) * _speed; 
		_xspeed = Mathf.Cos (_angletir) * _speed * direction;
		transform.eulerAngles = new Vector3(0f, 0f, 0f);

	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate(new Vector3(_xspeed,_yspeed,0.0f)*Time.deltaTime); // Mouvement de l'objet selon un vecteur [x,y]
		_yspeed  -=  _speedreduction*Time.deltaTime; 						   

	

	}


}