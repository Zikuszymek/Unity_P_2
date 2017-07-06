using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public void loadLevel(string levelName){
		Application.LoadLevel (levelName);
	}

	public void quitGame(){
		Application.Quit ();
	}

	public void LoadNextLevel(){
		Application.LoadLevel (Application.loadedLevel + 1);
	}

}
