using UnityEngine;
using System.Collections;

public class PlayerVictory : MonoBehaviour {

	public Texture backgroundTexture;
	private string Winner;
	public GUISkin skin;

	void OnGUI() {

		GUI.skin = skin;

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);
			if(Base.P1won){
			Winner="Player one wins!"; //Il manque une condition de victoire
		}
		else{
			Winner="Player two wins!";
		}
		GUI.Label (new Rect (0 ,0.2f*Screen.height, Screen.width, 200), Winner);
		
		
		//Display our Buttons with gui outlines
		if (GUI.Button (new Rect (Screen.width * 0.15f, Screen.height * 0.75f, 230*Screen.width/1280, 100*Screen.height/720), "New Game"))
		{
			Application.LoadLevel (2);
			print ("clicked");
		}
		if (GUI.Button (new Rect (Screen.width * 0.65f, Screen.height * 0.75f, 230*Screen.width/1280, 100*Screen.height/720), "Exit Game"))
		{
			Application.Quit();
			print ("clicked");
		}
	}

}
