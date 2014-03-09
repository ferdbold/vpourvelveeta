using UnityEngine;
using System.Collections;

public class PlayerVictory : MonoBehaviour {

	public Texture backgroundTexture;
	private string Winner;
	public GUIStyle myGUIstyle;


		


	void OnGUI() {

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);
			if(Base.P1won){
			Winner="Player1 wins!"; //Il manque une condition de victoire
		}
		else{
			Winner="Player2 wins!";
		}
		GUI.Label (new Rect (0.42f*Screen.width,0.35f*Screen.height, Screen.width, Screen.height), Winner, myGUIstyle);
		
		
		//Display our Buttons with gui outlines
		if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.75f, Screen.width * 0.4f, Screen.height * 0.2f), "New Game"))
		{
			Application.LoadLevel (2);
			print ("clicked");
		}
		if (GUI.Button (new Rect (Screen.width * 0.55f, Screen.height * 0.75f, Screen.width * 0.4f, Screen.height * 0.2f), "Exit Game"))
		{
			Application.Quit();
			print ("clicked");
		}
	}

}
