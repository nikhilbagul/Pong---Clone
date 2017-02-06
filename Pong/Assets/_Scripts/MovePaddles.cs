using UnityEngine;
using System.Collections;

public class MovePaddles : MonoBehaviour {

    public float speed = 15.0f;
    public float padding = 1.0f;
    public string axis = "Vertical";

    private float boundaryTopEdge, boundaryBottomEdge;

    // Use this for initialization
    void Start ()
    {
        Camera camera = Camera.main;
        float distanceFromCamera = transform.position.z - camera.transform.position.z;
        boundaryBottomEdge = camera.ViewportToWorldPoint(new Vector3(1, 1, distanceFromCamera)).y - padding;
        boundaryTopEdge = camera.ViewportToWorldPoint(new Vector3(0, 0, distanceFromCamera)).y + padding;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float directionOfMovement = Input.GetAxisRaw(axis);

        if (directionOfMovement == 1.0f)
        {
            transform.position = new Vector3(
                   transform.position.x,
                   Mathf.Clamp(transform.position.y + speed * Time.deltaTime, boundaryTopEdge, boundaryBottomEdge),
                   transform.position.z
               );
        }

        if (directionOfMovement == -1.0f)
        {
            transform.position = new Vector3(
                   transform.position.x,
                   Mathf.Clamp(transform.position.y - speed * Time.deltaTime, boundaryTopEdge, boundaryBottomEdge),
                   transform.position.z
               );
        }

    }

    

}
