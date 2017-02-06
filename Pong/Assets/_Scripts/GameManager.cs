using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static int leftPlayerScore, rightPlayerScore;
    public Text leftPlayerScoreText, rightPlayerScoreText, leftPlayerWinText, rightPlayerWinText;
    public GameObject ballPrefab;

    private bool gameOver;

    void OnEnable()
    {
        BallBehavior.leftPlayerScoreUpdate += leftPlayerScoreTextUpdate;
        BallBehavior.rightPlayerScoreUpdate += rightPlayerScoreTextUpdate;
    }
    void OnDisable()
    {
        BallBehavior.leftPlayerScoreUpdate -= leftPlayerScoreTextUpdate;
        BallBehavior.rightPlayerScoreUpdate -= rightPlayerScoreTextUpdate;
    }

    // Use this for initialization
    void Start ()
    {
        gameOver = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (leftPlayerScore == 5)
        {
            leftPlayerWinText.gameObject.SetActive(true);
            leftPlayerScore++;
            gameOver = true;
        }

        if (rightPlayerScore == 5)
        {
            rightPlayerWinText.gameObject.SetActive(true);
            rightPlayerScore++;
            gameOver = true;
        }

        if ( gameOver && (Input.GetKeyDown(KeyCode.Return)))
        {
            RestartGame();        
        }

        if (gameOver && (Input.GetKeyDown(KeyCode.Space)))
        {
            SceneManager.LoadScene("MainMenu");
        }

    }

    void leftPlayerScoreTextUpdate()
    {
        leftPlayerScoreText.text = leftPlayerScore.ToString();
    }

    void rightPlayerScoreTextUpdate()
    {
        rightPlayerScoreText.text = rightPlayerScore.ToString();
    }

    void RestartGame()
    {
        gameOver = false;

        GameObject Ball = (GameObject)Instantiate(ballPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        leftPlayerScore = 0;
        rightPlayerScore = 0;

        leftPlayerWinText.gameObject.SetActive(false);
        rightPlayerWinText.gameObject.SetActive(false);
        leftPlayerScoreText.text = leftPlayerScore.ToString();
        rightPlayerScoreText.text = rightPlayerScore.ToString();


    }
}
