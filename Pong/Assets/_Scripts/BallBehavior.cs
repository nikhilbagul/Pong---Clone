using UnityEngine;
using System.Collections;

public class BallBehavior : MonoBehaviour {

    public float launchSpeed;

    public delegate void scoreUpdateEvent();
    public static event scoreUpdateEvent leftPlayerScoreUpdate;
    public static event scoreUpdateEvent rightPlayerScoreUpdate;

    private Vector2 ballVelocity;

    void Start()
    {
        // Initial Velocity
        int choice = Random.Range(0, 2);
        if (choice == 0)
            GetComponent<Rigidbody2D>().velocity = (Vector2.right + Vector2.up) * launchSpeed;
        else
            GetComponent<Rigidbody2D>().velocity = (Vector2.left + Vector2.down) * launchSpeed;
    }

    // Update is called once per frame
    void Update ()
    {
	    
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //ballVelocity = GetComponent<Rigidbody2D>().velocity;
            ////print(ballVelocity.magnitude);
            //print(collision.rigidbody.velocity);
            //Vector2 tweak = ballVelocity + collision.rigidbody.velocity;
            //GetComponent<Rigidbody2D>().velocity = tweak;

            //calculate the reflection angle
            float reflectionAngle = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;
            //set angle and speed
            Vector2 newDirection = new Vector2(-1, reflectionAngle).normalized;
            GetComponent<Rigidbody2D>().velocity = (newDirection * GetComponent<Rigidbody2D>().velocity.magnitude) + newDirection*1.5f;            
        }

        if (collision.gameObject.tag == "LeftWall")
        {
            GameManager.rightPlayerScore++;
            if (rightPlayerScoreUpdate != null)
            {
                rightPlayerScoreUpdate();
            }

            if (GameManager.rightPlayerScore >= 5)
                Destroy(this.gameObject);            
            else
                ResetBall("rightPlayer");
        }

        if (collision.gameObject.tag == "RightWall")
        {
            GameManager.leftPlayerScore++;
            if (leftPlayerScoreUpdate != null)
            {
                leftPlayerScoreUpdate();
            }

            if (GameManager.leftPlayerScore >= 5)
                Destroy(this.gameObject);
            else
                ResetBall("leftPlayer");            
        }
    }

    void ResetBall(string caller)
    {
        if (caller == "rightPlayer")
        {
            gameObject.transform.position = new Vector2(0.0f, 0.0f);

            int choice = Random.Range(0, 2);
            if(choice == 0)
                GetComponent<Rigidbody2D>().velocity = (Vector2.right + Vector2.up) * launchSpeed;
            else
                GetComponent<Rigidbody2D>().velocity = (Vector2.right + Vector2.down) * launchSpeed;
        }

        else if (caller == "leftPlayer")
        {
            gameObject.transform.position = new Vector2(0.0f, 0.0f);
            int choice = Random.Range(0, 2);
            if (choice == 0)
                GetComponent<Rigidbody2D>().velocity = (Vector2.left + Vector2.up) * launchSpeed;
            else
                GetComponent<Rigidbody2D>().velocity = (Vector2.left + Vector2.down) * launchSpeed;
        }
        
    }
}
