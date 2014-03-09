using UnityEngine;
using System.Collections;

public class ProjectileManager : MonoBehaviour {

	public GameObject projectile;
	private Shoot_Projectile shoot_Projectile;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateProjectile(int direction,float damage,Vector3 position,string unitName,float range){
		GameObject copy = (GameObject)Instantiate (projectile, position, transform.rotation);
		shoot_Projectile = (Shoot_Projectile)copy.GetComponent<Shoot_Projectile> ();
		shoot_Projectile.direction = direction;
		shoot_Projectile.Dmg = damage;
		shoot_Projectile.UnitName = unitName;
		shoot_Projectile.unitRange = range;
	}
}
