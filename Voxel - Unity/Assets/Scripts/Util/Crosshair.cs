using UnityEngine;
using System.Collections;
 
public class Crosshair : MonoBehaviour {
 
	private Rect position;
	public Texture2D crosshairTexture;
 
	void Update() {
		position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height - crosshairTexture.height) / 2, crosshairTexture.width, crosshairTexture.height);
	}
 
	void OnGUI() {
		GUI.DrawTexture(position, crosshairTexture);
	}
}