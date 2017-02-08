using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static int leftPlayerScore, rightPlayerScore;                                            //stores the static instances of player scores 
    public Text leftPlayerScoreText, rightPlayerScoreText, leftPlayerWinText, rightPlayerWinText;   //stores the instance of UI text message display components
    public GameObject ballPrefab;                                                                   //stores the ball Prefab to spawn on Game restart

    private bool gameOver;
    private Transform playerRight, playerLeft;                                                      //stores the transforms of the left and right player paddles

    void OnEnable()
    {
        //subscribe the mothods to invoke, to score update events
        BallBehavior.leftPlayerScoreUpdate += leftPlayerScoreTextUpdate;            
        BallBehavior.rightPlayerScoreUpdate += rightPlayerScoreTextUpdate;
    }
    void OnDisable()
    {
        //unsubscribe the methods to invoke on disable
        BallBehavior.leftPlayerScoreUpdate -= leftPlayerScoreTextUpdate;
        BallBehavior.rightPlayerScoreUpdate -= rightPlayerScoreTextUpdate;
    }

    // Use this for initialization
    void Start ()
    {
        gameOver = false;
        playerRight = GameObject.FindGameObjectWithTag("PlayerRight").transform;        //Find and store the player paddle gameobject's transform

        if(GameObject.FindGameObjectWithTag("PlayerLeft") != null)
            playerLeft = GameObject.FindGameObjectWithTag("PlayerLeft").transform;
        else
            playerLeft = GameObject.FindGameObjectWithTag("PlayerAI").transform;

        leftPlayerScore = 0;
        rightPlayerScore = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (leftPlayerScore == 5)                                                       //check the win condition every frame    
        {
            leftPlayerWinText.gameObject.SetActive(true);                               //display the win message
            leftPlayerScore++;                                                          
            gameOver = true;                                                            //set the gameOver flag    
        }

        if (rightPlayerScore == 5)                                                       //check the win condition every frame    
        {
            rightPlayerWinText.gameObject.SetActive(true);                               //display the win message
            rightPlayerScore++;
            gameOver = true;                                                             //set the gameOver flag
        }

        if ( gameOver && (Input.GetKeyDown(KeyCode.Return)))                             //If player chooses to restart the game, start the Coroutine to reset the game
        {
            StartCoroutine("ResetGame");
            //SceneManager.LoadScene("GameScreen-AI");
        }

        if (gameOver && (Input.GetKeyDown(KeyCode.Space)))                              //If the game is over and player chooses to quit the game
        {
            SceneManager.LoadScene("MainMenu");
        }

    }

    void leftPlayerScoreTextUpdate()
    {
        leftPlayerScoreText.text = leftPlayerScore.ToString();                          //update the player score text
    }

    void rightPlayerScoreTextUpdate()
    {
        rightPlayerScoreText.text = rightPlayerScore.ToString();                        //update the player score text
    }

    public IEnumerator ResetGame()                                                    //used to reset the game state, flags and counters
    {

        yield return new WaitForSeconds(0.2f);                                         //delay/pause before restarting the game

        gameOver = false;
        GameObject Ball = (GameObject)Instantiate(ballPrefab, new Vector3(0, 0, 0), Quaternion.identity);       //Instantiate the ball Prefab as a GameObject   
        Ball.name = "Ball";

        //reset the player scores
        leftPlayerScore = 0;
        rightPlayerScore = 0;

        //disable player score and win messages
        leftPlayerWinText.gameObject.SetActive(false);
        rightPlayerWinText.gameObject.SetActive(false);
        leftPlayerScoreText.text = leftPlayerScore.ToString();
        rightPlayerScoreText.text = rightPlayerScore.ToString();

        //reset the Y position of the player paddles
        playerRight.position = new Vector2(playerRight.position.x, 0.0f);
        playerLeft.position = new Vector2(playerLeft.position.x, 0.0f);
    }
}
