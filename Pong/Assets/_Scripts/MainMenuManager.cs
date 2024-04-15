using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    public GameObject divider, mainMenuCanvas, player1ModeHelpCanvas, player2ModeHelpCanvas;

    private bool player1Mode, player2Mode, isIntroTextBlinking, isTypingVFXRunning;

    public Text introText;
    public GameObject creditsPanel, mainMenuPanel;
    

    void Start()
    {
        player1Mode = false;
        player2Mode = false;        
        isTypingVFXRunning = false;

        // Intro Text blinking animation
        //introText.DOColor(Color.black, 1.0f,).SetLoops(-1, LoopType.Yoyo);

        isIntroTextBlinking = true;
        StartCoroutine(BlinkIntroText());
    }

    IEnumerator BlinkIntroText()
    {
        while (isIntroTextBlinking)
        {
            if (introText.color.a == 1.0f)
            {
                introText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
            else 
            {
                introText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }

            yield return new WaitForSeconds(0.5f);
        }
                   
    }

	// Update is called once per frame, roughly 60 times per second. (System dependent value)
	void Update ()
    {
        if (mainMenuPanel.activeSelf)
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                on1PlayerClick();                                       //load Help Screen for 1 player scene from main menu on user selection
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                on2PlayerClick();                                       //load Help Screen scene for 2 players from main menu on user selection
            }

            if (Input.GetKeyUp(KeyCode.Space))                            //quit the game from main menu
            {
                onQuitClick();
            }
        }

        if (Input.anyKeyDown && !mainMenuPanel.activeSelf)
        {
            StartCoroutine(OnSplashScreenClicked());
        }


        if (player1Mode)
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                SceneManager.LoadScene("GameScreen-1player");               //load GameScreen scene for 1 player mode
            }
        }

        if (player2Mode)
        {
            if (Input.GetKeyUp(KeyCode.Return))
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
        player1Mode = true;                
    }

    public void on2PlayerClick()
    {
        divider.SetActive(true);
        player2ModeHelpCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);        
        player2Mode = true;       
    }

    public void onQuitClick()
    {
        //Application.Quit();                                         //quit the game from main menu
        SceneManager.LoadScene("SplashScreenAndMainMenu");
    }   

    public IEnumerator OnSplashScreenClicked()
    {
        if (!isTypingVFXRunning)
        {
            isIntroTextBlinking = false;
            introText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            introText.text = "Pong is a 2D sports game that simulates table tennis. The player controls an in-game paddle by moving it vertically across the left or right side of the screen. They can compete against another player controlling a second paddle on the opposing side. Players use the paddles to hit a ball back and forth. The goal is for each player to reach 5 points before the opponent; points are earned when one fails to return the ball to the other.";

            introText.GetComponent<TypingEffect>().StartTypingTextVFX(PostTypingVFX); // Start typing animation
            isTypingVFXRunning = true;
            yield return null;
        }
        else
        {
            introText.GetComponent<TypingEffect>().StopTypingVFX(); // Stop typing animation            
            introText.text = "Pong is a 2D sports game that simulates table tennis. The player controls an in-game paddle by moving it vertically across the left or right side of the screen. They can compete against another player controlling a second paddle on the opposing side. Players use the paddles to hit a ball back and forth. The goal is for each player to reach 5 points before the opponent; points are earned when one fails to return the ball to the other.";
            yield return new WaitForSeconds(1);
            PostTypingVFX();
            isTypingVFXRunning = false;
        }
    }

    void PostTypingVFX()
    {        
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(true);
    }

}
