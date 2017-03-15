using UnityEngine;
using System.Collections;

public class InstructionHandle : MonoBehaviour {

    public Instruction instruction;

    public void Increment()
    {
        instruction.Parameter++;
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }

    public void Decrement()
    {
        instruction.Parameter--;
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }

    public void Remove()
    {
        instruction.Parameter = 0;
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }
}
