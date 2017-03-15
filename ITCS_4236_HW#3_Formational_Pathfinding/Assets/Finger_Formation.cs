using UnityEngine;
using System.Collections;

public class Finger_Formation : MonoBehaviour
{
    int finger;
    GameObject hand;
    public Vector3 p_offset;
    public Vector3 r_offset;
    Vector3 p_direction;
    Rigidbody rb;
    Vector3 p_target;
    Vector3 r_target;
    //public float targetRoation = -90f;
    public float maxSpeed = 10f;
    public float satRadius = 1f;
    public float timeToTarget = .20f;
    public float turnSpeed = 4.5f;
    public float sightRange = 10.0f;
    bool move = false;
    bool dodge = false;


    // Use this for initialization
    void Start()
    {
        //inisalizing the hand object that is leading you
        hand = GameObject.Find("Hand");
        //hand = GameObject.Find("Finger_1");
        //inisheating the finger you are on
        switch (this.name)
        {
            case "Finger_0":
                rb = GameObject.Find("Finger_0").GetComponent<Rigidbody>();
                //r_offset = transform.rotation * r_offset;//the direction the finger should always be faceing or turning to
                //p_offset = hand.transform.position - transform.position;//rotaing so vector is ponting twards the desiered location
                finger = 0;
                break;
            case "Finger_1":
                rb = GameObject.Find("Finger_1").GetComponent<Rigidbody>();
                //r_offset = transform.rotation * r_offset;//the direction the finger should always be faceing or turning to
                //p_offset = hand.transform.position - transform.position;//rotaing so vector is ponting twards the desiered location
                finger = 1;
                break;
            case "Finger_2":
                rb = GameObject.Find("Finger_2").GetComponent<Rigidbody>();
                //r_offset = transform.rotation * r_offset;//the direction the finger should always be faceing or turning to
                //p_offset = hand.transform.position - transform.position;//rotaing so vector is ponting twards the desiered location
                finger = 2;
                break;
            case "Finger_3":
                rb = GameObject.Find("Finger_3").GetComponent<Rigidbody>();
                //r_offset = transform.rotation * r_offset;//the direction the finger should always be faceing or turning to
                //p_offset = hand.transform.position - transform.position;//rotaing so vector is ponting twards the desiered location
                finger = 3;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //adjusting the offsets from the hand
        switch (finger)
        {
            case 0:
                r_offset = Quaternion.Euler(new Vector3(0f, -90f, 0f)) * hand.transform.forward;//the direction the finger should always be faceing or turning to
                //r_offset = transform.rotation * r_offset;//the direction the finger should always be faceing or turning to
                //p_offset += hand.transform.forward;
                p_offset = hand.transform.forward * 3.0f;// extending the vector 
                p_offset = Quaternion.Euler(new Vector3(0f, -140, 0f)) * p_offset;//rotaing so vector is ponting twards the desiered location
                break;
            case 1:
                r_offset = Quaternion.Euler(new Vector3(0f, 0f, 0f)) * hand.transform.forward;//the direction the finger should always be faceing or turning to
                                                                                              //r_offset = transform.rotation * r_offset;//the direction the finger should always be faceing or turning to
                                                                                              //p_offset += hand.transform.forward;
                p_offset = hand.transform.forward * 1f;// extending the vector 
                p_offset = Quaternion.Euler(new Vector3(0f, 180f, 0f)) * p_offset;//rotaing so vector is ponting twards the desiered location
                break;
            case 2:
                r_offset = Quaternion.Euler(new Vector3(0f, 45f, 0f)) * hand.transform.forward;//the direction the finger should always be faceing or turning to
                                                                                               //r_offset = transform.rotation * r_offset;//the direction the finger should always be faceing or turning to
                                                                                               //p_offset += hand.transform.forward;
                p_offset = hand.transform.forward * 3.0f;// extending the vector 
                p_offset = Quaternion.Euler(new Vector3(0f, 140f, 0f)) * p_offset;//rotaing so vector is ponting twards the desiered location
                break;
            case 3:
                r_offset = Quaternion.Euler(new Vector3(0f, 180f, 0f)) * hand.transform.forward;//the direction the finger should always be faceing or turning to
                                                                                                //r_offset = transform.rotation * r_offset;//the direction the finger should always be faceing or turning to
                                                                                                //p_offset += hand.transform.forward;
                p_offset = hand.transform.forward * 5.0f;// extending the vector 
                p_offset = Quaternion.Euler(new Vector3(0f, 155f, 0f)) * p_offset;//rotaing so vector is ponting twards the desiered location
                break;
        }
        //Debug.Log(p_offset);
        //r_target = hand.transform.rotation.eulerAngles + r_offset;
        r_target = r_offset;

        //adding the offset to the position of the hand
        p_target = hand.transform.position + p_offset;

        //stoping avoidence jitter after finger reaches the destonation and there is an Obstacle near by.
        if (move)
        {
            //determoning if you need to shift left or right to avoid Obstacles and then adjusting the p_target accordingly
            p_target = ObstacleDetect();
        }
        //getting the target to go to
        //the direction that the character needs to go
        var steering = steeringDirection();
        //limmiting the stearing vecor with a max speed
        steering /= timeToTarget;
        if (steering.magnitude > maxSpeed)
        {
            steering.Normalize();
            steering *= maxSpeed;
        }

        //exicuting a lurping turn twards the target rotation
        rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, Quaternion.LookRotation(r_target), Time.deltaTime * turnSpeed);

        if (move)
        {
            //moving allong the steering vector twards the target.
            rb.velocity = steering;
        }
    }
    //getting the vector the the finger needs to move allong
    public Vector3 steeringDirection()
    {
        //getting the direction that this object needs to turn to
        var steering = p_target - transform.position;

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
        move = true;
        return steering;
    }

    //detecs an Obstacle is in the way and returns the shifted positon that you need to move twards to avoid it
    private Vector3 ObstacleDetect()
    {
        //raycasts to see if there is an Obstacle in this fingers way
        RaycastHit hit;
        float sightReduction;
        Vector3 newTarget;
        Vector3 sightLine;
        int ObstacleRight = 0;
        int ObstacleLeft = 0;
        Vector3[] offset = new Vector3 [62];
        int sightSections = offset.Length / 6;
        offset[0] = new Vector3(0f, 179f, 0f);
        //Vector3[] offset = {new Vector3(0f, 90f, 0f), new Vector3(0f, 75f, 0f), new Vector3(0f, 60f, 0f), new Vector3(0f, 45f, 0f),
        //    new Vector3(0f, 30f, 0f), new Vector3(0f, 15f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, -15f, 0f), new Vector3(0f, -30f, 0f),
        //    new Vector3(0f, -45f, 0f), new Vector3(0f, -60f, 0f), new Vector3(0f, -75f, 0f), new Vector3(0f, -90f, 0f)};

        //creating the array of offset used to dtermen the raycast angles
        for (int i = 1; i < offset.Length; i++)
        {

            offset[i] = new Vector3(0f, offset[i-1].y - 5.7f, 0f);
        }
        //cycaling thru all 62 rays
        for (int i = 0; i < offset.Length; i++)
        {
            //Debug.Log("Ray loop");
            //reducing sight range if ray is parrifary vison
            if (i < sightSections || i > offset.Length - sightSections)
            {
                sightReduction = 1.4f;
            }
            else if (i < sightSections * 2 || i > offset.Length - (sightSections * 2))
            {
                sightReduction = 1.6f;
            }
            else if (i < sightSections * 3 || i > offset.Length - (sightSections * 3))
            {
                sightReduction = 1.20f;
            }
            else if (i < sightSections * 4 || i > offset.Length - (sightSections * 4))
            {
                sightReduction = 1.10f;
            }
            else
            {
                sightReduction = 1f;
            }
            sightLine = Quaternion.Euler(offset[i]) * (p_target - transform.position);
            //sightLine = Quaternion.Euler(offset[i]) * hand.transform.forward;
            Ray sightRay = new Ray(transform.position, sightLine);
            Debug.DrawRay(transform.position, sightRay.direction * (sightRange / sightReduction), Color.red);
            //Debug.DrawRay(transform.position, sightRay.direction * (sightRange), Color.red);
            if (Physics.Raycast(sightRay, out hit, sightRange / sightReduction) && hit.collider.CompareTag("Obstacle"))
            //if (Physics.Raycast(sightRay, out hit, sightRange) && hit.collider.CompareTag("Obstacle"))
            {
                //add to the total amount to the left or right depending on how many rays were hit
                if (i < offset.Length / 2)
                {
                    ObstacleRight++;
                    Debug.DrawRay(transform.position, sightRay.direction * (sightRange), Color.green);
                }
                else
                {
                    ObstacleLeft++;
                    Debug.DrawRay(transform.position, sightRay.direction * (sightRange), Color.blue);
                }
                //Debug.Log("see an Obstacle");
            }
        }
        //determoning if target should be shifted left or right
        if(ObstacleLeft >= ObstacleRight && ObstacleLeft != 0)
        {
            //rotating the movment direction to the right 
            //moveDirection = Quaternion.Euler(new Vector3(0f, 20f*ObstacleLeft, 0f)) * moveDirection;

           //Debug.Log("slide right");
            //taking the curent target and adding a right facing vector of the leader to it inorder to shift the target right relotive to the leader
            //extending the vector to a greater digrie depending on how far right you want to go
            newTarget = p_target + (hand.transform.right * 1.5f);
            return newTarget;

        }
        else if(ObstacleRight > ObstacleLeft)
        {
            //rotating the movement direction to the left
            //moveDirection = Quaternion.Euler(new Vector3(0f, -20f*ObstacleRight, 0f)) * moveDirection;
            //Debug.Log("Slide left");
            //taking the curent target and adding a left facing vector of the leader to it inorder to shift the target left relotive to the leader
            //extending the vector to a greater digrie depending on how far seft you want to go
            newTarget = p_target + (hand.transform.right * -1.5f);


            return newTarget;
        }
        newTarget = p_target;
        return newTarget;
    }
}
