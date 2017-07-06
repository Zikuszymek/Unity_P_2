using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreGet : MonoBehaviour {

	void Start () {
		Text myText = GetComponent<Text> ();
		myText.text = ScroreKeeper.score.ToString ();
		ScroreKeeper.Reset ();
	}
	
}
