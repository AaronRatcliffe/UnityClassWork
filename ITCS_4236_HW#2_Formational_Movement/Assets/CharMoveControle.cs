using UnityEngine;
using System.Collections;

public class CharMoveControle : MonoBehaviour
{
    public Vector3 target;
    public Camera camera;
    public Rigidbody rb;
    public float maxSpeed = 10f;
    public float satRadius = 1f;
    public float timeToTarget = 0.50f;
    public float turnSpeed = 2.0f;
    bool move = false;
    public int leaderLayerNum;
    public int obsticalLayerNum;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(leaderLayerNum, obsticalLayerNum);
        target = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //setting target of movment to mouse position when mouse is cliced
        if (Input.GetMouseButtonDown(0))
        {
            //raycast from the cameras position looking for the ground
            RaycastHit hit;
            //cheking to see if you have hit an object with your click
            if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                return;
            }
            //cheking to see if the object you hit has a colider
            if (!hit.transform)
            {
                return;
            }
            //make the target location where the raycast hit
            target = hit.point;
            target.y = .5f;
            move = true;
        }

        //the direction that the character needs to go
        var steering = steeringDirection();
        //limmiting the stearing vecor with a max speed
        steering /= timeToTarget;
        if (steering.magnitude > maxSpeed)
        {
            steering.Normalize();
            steering *= maxSpeed;
        }
        //exicuting a lurping turn twards the target
        if (move)
        {
            rb.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(steering), Time.deltaTime * turnSpeed);
            //rb.transform.rotation = Quaternion.LookRotation(steering);
            //moving twards the target
            rb.velocity = steering;
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    //getting the location the charactor needs to face
    public Vector3 steeringDirection()
    {
        //getting the direction that this object needs to turn to
        var steering = target - transform.position;
        //cheking if we are close enogh to target
        //Debug.Log(steering.magnitude < satRadius);
        if (steering.magnitude < satRadius)
        {
            //Debug.Log(steering);
            move = false;
            return transform.position;
            //return new Vector3(0,0,0);
        }
        //Debug.Log(steering);
        return steering;
    }
}
