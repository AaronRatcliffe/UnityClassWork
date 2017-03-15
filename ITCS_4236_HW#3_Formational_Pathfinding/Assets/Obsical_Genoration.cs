using UnityEngine;
using System.Collections;

public class Obsical_Genoration : MonoBehaviour {
    public Vector3 target;
    public Camera camera;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //setting target of Obstacle instanceation to mouse position when mouse is cliced
        if (Input.GetMouseButtonDown(1))
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
            //cheking to see if it hit the ground
            if (hit.collider.tag != "Ground")
            {
                return;
            }
            //make the target location where the raycast hit
            target = hit.point;
            target.y = 1.0f;
            //instanceate instance of Obstacle at target location
            Instantiate(Resources.Load("Obstacle"), target, new Quaternion(0, 0, 0, 1));
            //cheking each ground tile to see what is in the location now
            GetComponent<Tile_Board>().Update_Board();
        }
    }
}
