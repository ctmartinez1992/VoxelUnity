using UnityEngine;
using System.Collections;
 
[AddComponentMenu("Util/MyFPS")]
public class MyFPS : MonoBehaviour {
 
	public Rect position = new Rect(0, 0, 30, 20); 			//The position of the text
	
	public float frequency = 0.5F; 							//The update frequency of the fps
	public int nbDecimal = 1; 								//How many decimal do you want to display
	
	public bool updateColor = true; 						//Do you want the color to change if the FPS gets low
	
	private GUIStyle style; 								//The style the text will be displayed at, based en defaultSkin.label
	private Color color = Color.white; 						//The color of the GUI, depending of the FPS (R < 10, Y < 30, G >= 30)
 
	private float accum = 0f; 								//FPS accumulated over the interval
	private int frames = 0;									//Frames drawn over the interval
	
	private string sFPS = "0";								//The fps formatted into a string.
 
	void Start() {
	    StartCoroutine(FPS());
	}
 
	IEnumerator FPS() {
		while(true) {
		    float fps = accum / frames;
		    sFPS = Mathf.Clamp(fps, 1, 60).ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));
 
			color = (fps >= 30) ? Color.green : ((fps > 10) ? Color.red : Color.yellow);
 
	        accum = 0.0F;
	        frames = 0;
 
			yield return new WaitForSeconds(frequency);
		}
	}
 
	void OnGUI() {
		if(style == null) {
			style = new GUIStyle(GUI.skin.label);
			style.normal.textColor = Color.white;
			style.alignment = TextAnchor.MiddleCenter;
		}
 
		GUI.color = updateColor ? color : Color.white;
		GUI.Label(new Rect(position.x, position.y, position.width, position.height), sFPS, style);
	}
 
	void Update() {
	    accum += Time.timeScale / Time.deltaTime;
	    ++frames;
	}
}