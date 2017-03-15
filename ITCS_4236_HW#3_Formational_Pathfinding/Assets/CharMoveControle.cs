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
    public int ObstacleLayerNum;

    A_Star_Controler A_Star;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(leaderLayerNum, ObstacleLayerNum);
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
            move = true;

            //setting the A* target to the new target
            //Debug.Log("Creat instance of A* with \n startPoint = " + GameObject.Find("Hand").transform.position + "\n endPoint = " + target);
            A_Star = new A_Star_Controler(GameObject.Find("Hand").transform.position, target);
            if (A_Star.movesToMake.Count > 0)
            {
                //pop off the first movment location from the stack of moves
                target = (Vector3)A_Star.movesToMake.Pop();
            }
        }
        try
        {
            var temp = target - transform.position;
            if (temp.magnitude < .005f && A_Star.movesToMake.Count > 0)
            {
                //pop off the first movment location from the stack of moves
                target = (Vector3)A_Star.movesToMake.Pop();
            }
        }
        catch(System.NullReferenceException e)
        {

        }
        target.y = .5f;

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
            if (A_Star.movesToMake.Count > 0)
            {
                //pop off the first movment location from the stack of moves
                target = (Vector3)A_Star.movesToMake.Pop();
            }
            else
            {
                //Debug.Log(steering);
                move = false;
                return transform.position;
                //return new Vector3(0,0,0);
            }
        }
        //Debug.Log(steering);
        return steering;
    }
}
