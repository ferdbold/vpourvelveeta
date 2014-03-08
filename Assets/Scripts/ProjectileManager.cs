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

	public void CreateProjectile(int direction,float damage,Vector3 position,bool isProjectileP1){
		GameObject copy = (GameObject)Instantiate (projectile, position, transform.rotation);
		shoot_Projectile = (Shoot_Projectile)copy.GetComponent<Shoot_Projectile> ();
		shoot_Projectile.direction = direction;
		shoot_Projectile.Dmg = damage;
		shoot_Projectile.IsP1 = isProjectileP1;
	}
}
