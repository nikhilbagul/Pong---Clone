using UnityEngine;
using System.Collections;

public class MovePaddles : MonoBehaviour {

    public float paddleMovementSpeed = 70.0f;           //used to tweak the paddle movement speeds from the inspector
    public float padding = 1.0f;                        //used to tweak the Top/Bottom padding from the inspector

    private bool isAIMode = false;
    private string playerControllerAxis;                 //used to assign the player control axis depending on the player
    private float boundaryTopEdge, boundaryBottomEdge;  //container to store top and bottom bounds of the screen/play space
    private GameObject Ball;
    private Rigidbody2D ballRigidBody;

    void Start ()
    {
        //calculate the top and bottom bounds of the screen based on Camera viewport bounds which is screen size dependent
        Camera camera = Camera.main;
        float distanceFromCamera = transform.position.z - camera.transform.position.z;                          //obtain the z depth of camera from the plane contaiing the Paddles
        boundaryTopEdge = camera.ViewportToWorldPoint(new Vector3(1, 1, distanceFromCamera)).y - padding;
        boundaryBottomEdge = camera.ViewportToWorldPoint(new Vector3(0, 0, distanceFromCamera)).y + padding;

        if (this.gameObject.tag == "PlayerLeft")
        {
            playerControllerAxis = "PlayerLeft";           
        }

        if (this.gameObject.tag == "PlayerRight")
        { playerControllerAxis = "PlayerRight"; }

        if (this.gameObject.tag == "PlayerAI")
        { isAIMode = true; }

        if (Ball == null)
        {
            Ball = GameObject.FindGameObjectWithTag("Ball");
            if (Ball != null)
            {
                ballRigidBody = Ball.GetComponent<Rigidbody2D>();
            }            
        }
        
    }
	
	void Update ()
    {
        if (!isAIMode)
        {
            float directionOfMovement = Input.GetAxisRaw(playerControllerAxis);             //obtain the current direction of movement of the paddle
            if (directionOfMovement == 1.0f)
            {
                //clamp the paddle movement within the screen top and bottom bounds
                //use Time.deltaTime to make the paddle movement Frame Rate independent
                transform.position = new Vector3(
                       transform.position.x,
                       Mathf.Clamp(transform.position.y + paddleMovementSpeed * Time.deltaTime,  boundaryBottomEdge, boundaryTopEdge),
                       transform.position.z
                   );
            }

            if (directionOfMovement == -1.0f)
            {
                //clamp the paddle movement within the screen top and bottom bounds
                //use Time.deltaTime to make the paddle movement Frame Rate independent
                transform.position = new Vector3(
                       transform.position.x,
                       Mathf.Clamp(transform.position.y - paddleMovementSpeed * Time.deltaTime, boundaryBottomEdge, boundaryTopEdge),
                       transform.position.z
                   );
            }

           
        }

        if (isAIMode)
        {
            if (Ball == null)
            {
                //print("No ball");
                Ball = GameObject.FindGameObjectWithTag("Ball");
                if (Ball != null)
                {
                    print("ball in scene");
                    ballRigidBody = Ball.GetComponent<Rigidbody2D>();
                }
            }

            if (Ball != null)
            {
                //print("ball in scene");
                if (ballRigidBody.velocity.x < 0.0f)
                {
                    if (Ball.transform.position.x < Random.Range(0.0f, 30.0f))              //adjust the min & max value of reaction zone to increase/decrease difficulty
                    {
                        float step = paddleMovementSpeed * Time.deltaTime;
                        Vector3 temp = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, Ball.transform.position.y), step);
                        transform.position = new Vector3(
                                             temp.x,
                                             Mathf.Clamp(temp.y, boundaryBottomEdge, boundaryTopEdge),
                                             temp.z);
                    }
                }                
            }
        }

    }   

}
