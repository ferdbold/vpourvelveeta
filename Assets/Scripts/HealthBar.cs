using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public GameObject BaseObject;
	public float barDisplay; //current progress
	private Vector2 pos2 = new Vector2(0.05f*Screen.width,0.57f*Screen.height);
	private Vector2 pos1 = new Vector2(0.05f*Screen.width,0.05f*Screen.height);
	private Vector2 size = new Vector2(240,10);
	public Texture2D emptyTex;
	public Texture2D fullTex;
	public Base healthbase;
	public GUIStyle progress_empty;
	public GUIStyle progress_full;

	
	void Start() {
		BaseObject = gameObject;
		healthbase =(Base)BaseObject.GetComponent<Base>();
	}
	void OnGUI() {
		if(healthbase.transform.parent.name=="P1")
		{
			//draw the background:
			GUI.BeginGroup(new Rect(pos1.x, pos1.y, size.x, size.y));
			GUI.Box(new Rect(0,0, size.x, size.y), emptyTex, progress_empty);

		
			//draw the filled-in part:
			GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
			GUI.Box(new Rect(0,0, size.x, size.y), fullTex, progress_full);
			GUI.EndGroup();
			GUI.EndGroup();
		}
		if(healthbase.transform.parent.name=="P2")
		{
			//draw the background:
			GUI.BeginGroup(new Rect(pos2.x, pos2.y, size.x, size.y));
			GUI.Box(new Rect(0,0, size.x, size.y), emptyTex, progress_empty);

		
			//draw the filled-in part:
			GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
			GUI.Box(new Rect(0,0, size.x, size.y), fullTex, progress_full);
			GUI.EndGroup();
			GUI.EndGroup();
		}
	}

	void Update() {
		barDisplay = healthbase.Hp/100.0f; // 100.0f correspond a la vie de la base
	}

}