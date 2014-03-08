using UnityEngine;
using System.Collections;

public class HowToPlay : MonoBehaviour {

	public Texture backgroundTexture;
	
	void OnGUI() {
		// Display background texture
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);
		
		// Display our Buttons with gui outlines
		if (GUI.Button (new Rect (Screen.width * 0.25f, Screen.height * 0.75f, Screen.width * 0.5f, Screen.height * 0.2f), "Play"))
		{
			Application.LoadLevel ("Level1");
			print ("clicked");
		}
	}
}
