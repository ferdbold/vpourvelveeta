using UnityEngine;
using System.Collections;


public class Shoot_projectile1 : MonoBehaviour {
	
	private int direction=1;
	public float _angle; 						//Angle de la vitesse
	public float _xspeed; 						//Variable vitesse verticale du projectile
	public float _yspeed; 						// Variable vitesse horizontale du projectile
	public float _speed; 						// Variable vitesse du projectile
	public const float _speedreduction = 1.0f;  // Constante de reduction de vitesse
	
	
	
	// Use this for initialization
	void Start () {
		
		_xspeed  = 5.0f*direction;
		_yspeed  = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Translate(new Vector3(_xspeed,_yspeed,0.0f)*Time.deltaTime); // Mouvement de l'objet selon un vecteur [x,y]
		_angle = (Mathf.Atan(_yspeed / _xspeed))*(360.0f/(2.0f*Mathf.PI)) ; //Calcul de l'angle en radian et conversion en degré
		/*if (_angle < 0) 
			{
				_angle+=360; 
			}
		transform.eulerAngles = new Vector3(0.0f, 0.0f, _angle);*/
		_yspeed  -=  _speedreduction*Time.deltaTime;
		_speed = Mathf.Pow((_xspeed * _xspeed + _yspeed * _yspeed), 0.5f);
		
		
		
		
		
		
	}
}