using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CommandList : MonoBehaviour {

    public static CommandList current;
    public GameObject MoveListPrefab, DropListPrefab, LoopListPrefab, EndLoopListPrefab, GrabListPrefab, SyncListPrefab, TurnLeftListPrefab, TurnRightListPrefab, WaitListPrefab;
    
    private Dictionary<InstructionType, GameObject> dict;
    private bool isItemHeld;
    private int itemHeldIndex;

    void Awake()
    {
        current = transform.GetComponent<CommandList>();
        dict = new Dictionary<InstructionType, GameObject>();
        dict.Add(InstructionType.Move, MoveListPrefab);
        dict.Add(InstructionType.Drop, DropListPrefab);
        dict.Add(InstructionType.Loop, LoopListPrefab);
        dict.Add(InstructionType.EndLoop, EndLoopListPrefab);
        dict.Add(InstructionType.Grab, GrabListPrefab);
        dict.Add(InstructionType.Sync, SyncListPrefab);
        dict.Add(InstructionType.TurnLeft, TurnLeftListPrefab);
        dict.Add(InstructionType.TurnRight, TurnRightListPrefab);
        dict.Add(InstructionType.Wait, WaitListPrefab);
    }

	// Use this for initialization
	void Start () {
        Refresh();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            return;
        }

        if (isItemHeld)
        {
            Refresh();
        }
        isItemHeld = false;
	}

    public void Refresh()
    {
        foreach (Transform t in transform)
        {
            GameObject.Destroy(t.gameObject);
        }

        Game.CurrentRobot.instructionSet.Cleanup();

        int n = 0;
        foreach (Instruction i in Game.CurrentRobot.instructionSet.Instructions)
        {
            GameObject g = GameObject.Instantiate(dict[i.Type]) as GameObject;
            g.GetComponent<RectTransform>().SetParent(this.transform,false);
            g.GetComponent<InstructionHandle>().instruction = i;
            g.GetComponent<InstructionItem>().index = n;
            if (g.transform.Find("Quantity") != null)
                g.transform.Find("Quantity").GetComponent<Text>().text = i.Parameter.ToString();

            n++;
        }
    }

    public void OnItemClicked(int index)
    {
        itemHeldIndex = index;
        isItemHeld = true;
    }

    public void OnPointerEnterItem(int index)
    {
        if (!isItemHeld)
        {
            return;
        }
        if (itemHeldIndex == index)
        {
            return;
        }

        Transform childA = transform.GetChild(index);
        Transform childB = transform.GetChild(itemHeldIndex);
        childA.SetSiblingIndex(itemHeldIndex);
        childB.SetSiblingIndex(index);
        childA.GetComponent<InstructionItem>().index = itemHeldIndex;
        childB.GetComponent<InstructionItem>().index = index;
        

        var l = Game.CurrentRobot.instructionSet.Instructions;
        Instruction temp = l[index];
        l[index] = l[itemHeldIndex];
        l[itemHeldIndex] = temp;

        itemHeldIndex = index;
    }
}
