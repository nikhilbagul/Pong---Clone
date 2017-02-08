using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public GameObject divider, mainMenuCanvas, player1ModeHelpCanvas, player2ModeHelpCanvas;

    private bool player1Mode, player2Mode, isMainMenu;

    void Start()
    {
        player1Mode = false;
        player2Mode = false;
        isMainMenu = true;
    }

	// Update is called once per frame, roughly 60 times per second. (System dependent value)
	void Update ()
    {
        if (isMainMenu)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                on1PlayerClick();                                       //load Help Screen for 1 player scene from main menu on user selection
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                on2PlayerClick();                                       //load Help Screen scene for 2 players from main menu on user selection
            }

            if (Input.GetKey(KeyCode.Space))                            //quit the game from main menu
            {
                onQuitClick();
            }
        }
        

        if (player1Mode)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                SceneManager.LoadScene("GameScreen-1player");               //load GameScreen scene for 1 player mode
            }
        }

        if (player2Mode)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                SceneManager.LoadScene("GameScreen-2players");               //load GameScreen scene for 2 player mode
            }
        }
    }

    public void on1PlayerClick()
    {
        divider.SetActive(true);
        player1ModeHelpCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        isMainMenu = false;
        player1Mode = true;                
    }

    public void on2PlayerClick()
    {
        divider.SetActive(true);
        player2ModeHelpCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        isMainMenu = false;
        player2Mode = true;       
    }

    public void onQuitClick()
    {
        Application.Quit();                                         //quit the game from main menu
    }
}
