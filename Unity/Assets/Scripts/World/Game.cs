using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public static Robot CurrentRobot;
    public static Game Current;

    public GameObject RobotPrefab;
    public GameObject RockPrefab;

    public static GameObject NextPuzzle;
    public GameObject Puzzle;

    public static Transform World;
    public static bool IsGameOver;
    private LayerMask robotLayer;
    
    private void Awake()
    {
        Current = this;

        World = GameObject.Find("World").transform;
    }

    private void Start()
    {

        GameObject puz = (NextPuzzle == null) ? GameObject.Instantiate(Puzzle) as GameObject
                                              : GameObject.Instantiate(NextPuzzle) as GameObject;
        puz.transform.parent = World;

        Transform spawn0 = puz.transform.Find("Spawn0");
        Transform spawn1 = puz.transform.Find("Spawn1");

        var r0 = GameObject.Instantiate(RobotPrefab, spawn0.position, spawn0.rotation) as GameObject;
        r0.transform.parent = World;
        r0.GetComponent<SpriteRenderer>().color = Color.red;

        if (spawn1 != null)
        {
            var r1 = GameObject.Instantiate(RobotPrefab, spawn1.position, spawn1.rotation) as GameObject;
            r1.transform.parent = World;
            r1.GetComponent<SpriteRenderer>().color = Color.blue;
        }

        CurrentRobot = r0.GetComponent<Robot>();
        IsGameOver = false;
    }

    public static void Fail()
    {
        GameObject.Find("Popup").GetComponent<Text>().text = "Failed";
        StepManager.Pause();
        IsGameOver = true;
    }

    public static void Win()
    {
        GameObject.Find("Popup").GetComponent<Text>().text = "Success";
        StepManager.Pause();
        IsGameOver = true;
    }

    public void MainMenu()
    {
		Application.LoadLevel ("MainMenu");
    }
}
