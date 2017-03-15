using UnityEngine;
using System.Collections;

public class InstructionButtonEvents : MonoBehaviour {

    public void Move()
    {
        Instruction i = new Instruction(InstructionType.Move, 1);
        Game.CurrentRobot.instructionSet.Instructions.Add(i);
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }

    public void Grab()
    {
        Instruction i = new Instruction(InstructionType.Grab);
        Game.CurrentRobot.instructionSet.Instructions.Add(i);
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }

    public void TurnLeft()
    {
        Instruction i = new Instruction(InstructionType.TurnLeft);
        Game.CurrentRobot.instructionSet.Instructions.Add(i);
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }

    public void TurnRight()
    {
        Instruction i = new Instruction(InstructionType.TurnRight);
        Game.CurrentRobot.instructionSet.Instructions.Add(i);
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }

    public void Drop()
    {
        Instruction i = new Instruction(InstructionType.Drop);
        Game.CurrentRobot.instructionSet.Instructions.Add(i);
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }

    public void Loop()
    {
        Instruction i = new Instruction(InstructionType.Loop, 1);
        Game.CurrentRobot.instructionSet.Instructions.Add(i);
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }

    public void EndLoop()
    {
        Instruction i = new Instruction(InstructionType.EndLoop);
        Game.CurrentRobot.instructionSet.Instructions.Add(i);
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }

    public void Wait()
    {
        Instruction i = new Instruction(InstructionType.Wait, 1);
        Game.CurrentRobot.instructionSet.Instructions.Add(i);
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }

    public void Sync()
    {
        Instruction i = new Instruction(InstructionType.Sync);
        Game.CurrentRobot.instructionSet.Instructions.Add(i);
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }
	
	public void Clear(){
		Game.CurrentRobot.instructionSet.Instructions.Clear();
		GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
	}
}
