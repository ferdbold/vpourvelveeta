/// <summary>
/// Main menu.
/// Attached to Main Camera
/// </summary>
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture backgroundTexture;

	public GUIStyle Button1;
	public GUIStyle Button2;

	void OnGUI(){
// Display background texture
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);

// Display our Buttons with gui outlines
		if (GUI.Button (new Rect (Screen.width * 0.25f, Screen.height * 0.50f, Screen.width * 0.5f, Screen.height * 0.2f), "How To Play"))
		{
			Application.LoadLevel (1);
			print ("clicked");
		}
		if (GUI.Button (new Rect (Screen.width * 0.25f, Screen.height * 0.75f, Screen.width * 0.5f, Screen.height * 0.2f), "Play Game"))
		{
			Application.LoadLevel (2);
			print ("clicked");
		}

// Display our Buttons without gui outlines
/*		if (GUI.Button (new Rect (Screen.width * 0.25f, Screen.height * 0.50f, Screen.width * 0.5f, Screen.height * 0.2f),"How to play",Button1))
		{
			print ("clicked");
		}
		if (GUI.Button (new Rect (Screen.width * 0.25f, Screen.height * 0.75f, Screen.width * 0.5f, Screen.height * 0.2f), "Play Game",Button2))
		{
			print ("clicked");
		}
*/
	}
}
