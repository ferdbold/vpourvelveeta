using UnityEngine;
using System.Collections;

public class HowToPlay : MonoBehaviour {

	public Texture backgroundTexture;
	public GUISkin skin;
	
	void OnGUI() {
		GUI.skin = skin;

		// Display background texture
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);
		
		// Display our Buttons with gui outlines
		if (GUI.Button (new Rect (Screen.width * 0.5f - (230*Screen.width/1280/2), Screen.height * 0.75f, 230*Screen.width/1280, 100*Screen.height/720), "GO!"))
		{
			Application.LoadLevel (2);
			print ("clicked");
		}
	}
}
