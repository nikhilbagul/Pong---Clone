using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallBehavior : MonoBehaviour {

    public float launchSpeed;                                       //ball launch speed
    public float spawnPointPadding = 25.0f;                         //spawnPoint range padding from Top and bottom bounds of screen    
    public AudioClip wallCollisionSFX, paddleContactSFX, dieSFX;    //obtain the SFX audio clips
    public float SFXVolume = 0.25f;                                 //used totweak SFX volume from the editor
    public float ballResetDelay = 0.3f;                             //tweak this value to change the reset/respawn time of the ball
    public float speedIncreaseCoefficient = 2.5f;                   //tweak this value to change the speed bump on each paddle hit

    public delegate void scoreUpdateEvent();                        //delagate to trigger events based on score update
    public static event scoreUpdateEvent leftPlayerScoreUpdate;
    public static event scoreUpdateEvent rightPlayerScoreUpdate;

    private AudioSource mainAudioSource;                            //container for the MAIN audio source in the scene
    private Vector2 ballVelocity;                                   //container for ball velocity vector
    private string caller;                                          //container to store name of player
    private float spawnPointTopBoundary, spawnPointBottonBoundary;  //container to store top and bottom bounds of the screen/play space

    void Start()
    {
        //calculate the top and bottom bounds of the screen based on Camera viewport bounds which is screen size dependent
        Camera camera = Camera.main;
        float distanceFromCamera = transform.position.z - camera.transform.position.z;
        spawnPointTopBoundary = camera.ViewportToWorldPoint(new Vector3(1, 1, distanceFromCamera)).y - spawnPointPadding;
        spawnPointBottonBoundary = camera.ViewportToWorldPoint(new Vector3(0, 0, distanceFromCamera)).y + spawnPointPadding;

        mainAudioSource = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();       //Find the MAIN audio source instance in the scene

        //set the initial Velocity of ball
        int choice = Random.Range(0, 2);
        if (choice == 0)
            GetComponent<Rigidbody2D>().velocity = (Vector2.right + Vector2.up) * launchSpeed;
        else
            GetComponent<Rigidbody2D>().velocity = (Vector2.left + Vector2.down) * launchSpeed;
    }

 
    void OnCollisionEnter2D(Collision2D collision)                              //trigger events based on Ball's collision with other scene objects
    {
        if (collision.gameObject.tag == "PlayerRight")                          
        {
            //play audio SFX on contact with the paddle
            mainAudioSource.PlayOneShot(paddleContactSFX, SFXVolume);           
            //calculate the reflection angle
            float reflectionAngle = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;
            //set angle and speed of the new velocity vector using the normalised vector of the reflection angle
            Vector2 newDirection = new Vector2(-1, reflectionAngle).normalized;
            GetComponent<Rigidbody2D>().velocity = (newDirection * GetComponent<Rigidbody2D>().velocity.magnitude) + newDirection* speedIncreaseCoefficient;    //adds a speed bump of 1.5f in the direction of the new vector       
        }

        if (collision.gameObject.tag == "PlayerLeft" || collision.gameObject.tag == "PlayerAI")
        {
            mainAudioSource.PlayOneShot(paddleContactSFX, SFXVolume);
            float reflectionAngle = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;
            Vector2 newDirection = new Vector2(1, reflectionAngle).normalized;
            GetComponent<Rigidbody2D>().velocity = (newDirection * GetComponent<Rigidbody2D>().velocity.magnitude) + newDirection * speedIncreaseCoefficient;
        }

        if (collision.gameObject.tag == "LeftWall")
        {
            //play audio SFX on contact with the Left/Right bounds
            mainAudioSource.PlayOneShot(dieSFX, SFXVolume);     //play desired SFX on collision
            //increment the respective player's score
            GameManager.rightPlayerScore++;
            if (rightPlayerScoreUpdate != null)
            {
                rightPlayerScoreUpdate();                   //trigger the event that is reponsible for invoking player Score Update
            }
            if (GameManager.rightPlayerScore >= 5)          //Destroy the ball as soon as Game is over
                Destroy(this.gameObject);
            else
            {
                caller = "rightPlayer";                     //if not game over, reset the ball and assign the caller name
                StartCoroutine("ResetBall");                //invoke the coroutine responsible for reseting the ball position and velocity
            }                
        }

        if (collision.gameObject.tag == "RightWall")
        {
            mainAudioSource.PlayOneShot(dieSFX, SFXVolume);
            GameManager.leftPlayerScore++;
            if (leftPlayerScoreUpdate != null)
            {
                leftPlayerScoreUpdate();
            }

            if (GameManager.leftPlayerScore >= 5)
                Destroy(this.gameObject);
            else
            {
                caller = "leftPlayer";
                StartCoroutine("ResetBall");
            }                
        }

        if (collision.gameObject.tag == "TopWall" || collision.gameObject.tag == "BottomWall")
        {
            mainAudioSource.PlayOneShot(wallCollisionSFX, SFXVolume);               //play SFX on ball bounce at top/bottom bounds
        }
    }

    public IEnumerator ResetBall()
    {
        Color temp = GetComponent<SpriteRenderer>().color;                      //fade out the ball on reset
        temp.a = 0f;
        GetComponent<SpriteRenderer>().color = temp;

        gameObject.transform.position = new Vector2(0.0f, Random.Range(spawnPointBottonBoundary, spawnPointTopBoundary));       //set the new launch position of the ball
        GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);                                                         //stop the ball before launching
        
        yield return new WaitForSeconds(ballResetDelay);                  //delay of 0.3 secs before launching the ball, so that players get a moment to reset
                                                                         

        temp.a = 1f;
        GetComponent<SpriteRenderer>().color = temp;                        //make the ball visible again before launching

        if (caller == "rightPlayer")                                        //launch the ball from a choice of predefined launch angles depending on the player who scored the last point            
        {
            int choice = Random.Range(0, 2);
            if(choice == 0)
                GetComponent<Rigidbody2D>().velocity = (Vector2.right + Vector2.up) * launchSpeed;
            else
                GetComponent<Rigidbody2D>().velocity = (Vector2.right + Vector2.down) * launchSpeed;
        }

        else if (caller == "leftPlayer")
        {
            int choice = Random.Range(0, 2);
            if (choice == 0)
                GetComponent<Rigidbody2D>().velocity = (Vector2.left + Vector2.up) * launchSpeed;
            else
                GetComponent<Rigidbody2D>().velocity = (Vector2.left + Vector2.down) * launchSpeed;
        }        
    }
}

//ballVelocity = GetComponent<Rigidbody2D>().velocity;
////print(ballVelocity.magnitude);
//print(collision.rigidbody.velocity);
//Vector2 tweak = ballVelocity + collision.rigidbody.velocity;
//GetComponent<Rigidbody2D>().velocity = tweak;
