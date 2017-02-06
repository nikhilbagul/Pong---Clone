using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	// Update is called once per frame, roughly 60 times per second. (System dependent value)
	void Update ()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene("GameScreen");               //load GameScreen scene from main menu on user selection
        }

        if (Input.GetKey(KeyCode.Space))                        //quit the game from main menu
        {
            Application.Quit();
        }
	}
}
