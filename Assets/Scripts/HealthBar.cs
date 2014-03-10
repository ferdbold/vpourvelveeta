using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public GameObject BaseObject;
	public float barDisplay; //current progress
	private Vector2 pos2 = new Vector2(0.05f*Screen.width,0.60f*Screen.height);
	private Vector2 pos1 = new Vector2(0.05f*Screen.width,0.08f*Screen.height);
	private Vector2 size = new Vector2(240,10);
	public Texture2D emptyTex;
	public Texture2D fullTex;
	public Base healthbase;
	public GUIStyle progress_empty;
	public GUIStyle progress_full;

	//SPAM FEEDBACK BAR
	public float spamBarDisplay;
	private Vector2 spamBarPos2 = new Vector2(0.05f*Screen.width,0.56f*Screen.height);
	private Vector2 spamBarPos1 = new Vector2(0.05f*Screen.width,0.04f*Screen.height);
	private Vector2 spamBarSize = new Vector2(240,10);
	public Texture2D spamEmptyTex;
	public Texture2D spamFullTex;
	private GameObject Player;
	private Player player;


	void Start() {
		BaseObject = gameObject;
		healthbase =(Base)BaseObject.GetComponent<Base>();
		Player = transform.parent.gameObject;
		player = (Player)Player.GetComponent<Player>();


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

			//Spam FeedBACK
			GUI.BeginGroup(new Rect(spamBarPos1.x, spamBarPos1.y, spamBarSize.x, spamBarSize.y));
			GUI.Box(new Rect(0,0, spamBarSize.x, spamBarSize.y), spamEmptyTex, progress_empty);
			//draw the filled-in part:
			GUI.BeginGroup(new Rect(0,0, spamBarSize.x * spamBarDisplay, spamBarSize.y));
			GUI.Box(new Rect(0,0, spamBarSize.x, spamBarSize.y), spamFullTex, progress_full);
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

			//Spam FeedBACK
			GUI.BeginGroup(new Rect(spamBarPos2.x, spamBarPos2.y, spamBarSize.x, spamBarSize.y));
			GUI.Box(new Rect(0,0, spamBarSize.x, spamBarSize.y), spamEmptyTex, progress_empty);
			//draw the filled-in part:
			GUI.BeginGroup(new Rect(0,0, spamBarSize.x * spamBarDisplay, spamBarSize.y));
			GUI.Box(new Rect(0,0, spamBarSize.x, spamBarSize.y), spamFullTex, progress_full);
			GUI.EndGroup();
			GUI.EndGroup();
		}
	}

	void Update() {
		barDisplay = healthbase.Hp/100.0f; // 100.0f correspond a la vie de la base
		spamBarDisplay = player.AttackMeter/player.MAX_attackMeter;
	}

}