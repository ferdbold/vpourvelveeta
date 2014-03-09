/// <summary>
/// Main menu.
/// Attached to Main Camera
/// </summary>
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture backgroundTexture;

	public GUISkin style;

	void OnGUI(){
// Display background texture
		GUI.skin = style;

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);

// Display our Buttons with gui outlines
		Debug.Log (Screen.width + " " + Screen.height);
		if (GUI.Button (new Rect (720*Screen.width/1280, 520*Screen.height/720, 230*Screen.width/1280, 100*Screen.height/720), "Instructions"))
		{
			Application.LoadLevel ("HowToPlay");
			print ("clicked");
		}
		if (GUI.Button (new Rect (300*Screen.width/1280, 520*Screen.height/720, 230*Screen.width/1280, 100*Screen.height/720), "GO!"))
		{
			Application.LoadLevel ("Game");
			print ("clicked");
		}
	}
}
