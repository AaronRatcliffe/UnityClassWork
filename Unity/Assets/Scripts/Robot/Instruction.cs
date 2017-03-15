using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Instruction
{

	#region Fields

    public InstructionType Type;
    public int StepCount = 0;
    
    private int _parameter = -1;
    public int Parameter
    {
        get { return _parameter; }
        set
        {
            _parameter = value;

            switch (Type)
            {
                case InstructionType.Move:
                case InstructionType.Wait:
                    StepCount = Parameter;
                    break;
            }
        }
    }
	
	#endregion

    #region Constructors

    public Instruction()
    {

    }

    public Instruction(InstructionType Type)
    {
        this.Type = Type;
    }

    public Instruction(InstructionType Type, int Parameter)
    {
        this.Type = Type;
        this.Parameter = Parameter;

    }
    #endregion

}

public class InstructionSet
{
    public List<Instruction> Instructions = new List<Instruction>();
    private int index;
    private Stack<LoopInfo> loopInfos = new Stack<LoopInfo>();

    private List<Instruction> backupInstructions;

    private Instruction currentInstruction;
    private int stepIndex = 1;
    
    public static bool SyncLocked;
    public static int SyncIndex = 0;
    private bool waitingForSync;
    private int waitingForSyncIndex = 0;

    public void Restart()
    {
        index = 0;
        stepIndex = 1;
        loopInfos.Clear();
        SyncLocked = false;
        waitingForSync = false;
        SyncIndex = 0;
        waitingForSyncIndex = 0;
    }

    public Instruction Next()
    {
        if (index >= Instructions.Count) return null;
        
        currentInstruction = Instructions[index];
        switch (currentInstruction.Type)
        {
            case InstructionType.Loop:
                index++;
                loopInfos.Push(new LoopInfo(index, (int) currentInstruction.Parameter));
                return Next();
            case InstructionType.EndLoop:
                LoopInfo currentLoop = loopInfos.Peek();
                currentLoop.count--;
                if (currentLoop.count == 0)
                {
                    loopInfos.Pop();
                    index++;
                }
                else
                {
                    index = currentLoop.index;
                }
                return Next();
            case InstructionType.Sync:
                if (SyncLocked)
                {
                    if (waitingForSync)
                    {
                        if (waitingForSyncIndex == SyncIndex)
                        {
                            return currentInstruction;
                        }
                        else
                        {
                            waitingForSync = false;
                            index++;
                            return Next();
                        }
                    }
                    else
                    {
                        SyncLocked = false;
                        waitingForSync = false;
                        index++;
                        return Next();
                    }
                }
                else
                {
                    if (waitingForSync)
                    {
                        waitingForSync = false;
                        index++;
                        return Next();
                    }
                    else
                    {
                        SyncLocked = true;
                        SyncIndex++;
                        waitingForSync = true;
                        waitingForSyncIndex = SyncIndex;
                        return currentInstruction;
                    }
                }
        }

        if (stepIndex >= currentInstruction.StepCount)
        {
            index++;
            stepIndex = 1;
        }
        else
        {
            stepIndex++;
        }
        return currentInstruction;
    }

    public void Cleanup()
    {
        int loopTestDepth = 0;
        bool revert = false;
        foreach (Instruction i in Instructions)
        {
            if (i.Type == InstructionType.Loop) loopTestDepth++;

            if (i.Type == InstructionType.EndLoop)
            {
                if (loopTestDepth == 0)
                {
                    revert = true;
                    break;
                }
                loopTestDepth--;
            }
        }
        if (revert)
        {
            Instructions = backupInstructions;
            return;
        }

        for (int index = 0; index < Instructions.Count; index++)
        {
            Instruction inst = Instructions[index];
            if (inst.Parameter == 0)
            {
                if (inst.Type == InstructionType.Loop)
                {
                    int endIndex = index+1;
                    int loopDepth = 0;
                    while (true)
                    {
                        if (Instructions[endIndex].Type == InstructionType.Loop)
                        {
                            loopDepth++;
                        }
                        else if (Instructions[endIndex].Type == InstructionType.EndLoop)
                        {
                            if (loopDepth == 0) break;
                            loopDepth--;
                        }
                        endIndex++;
                    }

                    Instructions.RemoveAt(endIndex);
                }

                Instructions.RemoveAt(index);
                index--;
                continue;
            }
        }

        for (int index = 0; index < Instructions.Count; index++)
        {
            Instruction inst = Instructions[index];
            if (inst.Type != InstructionType.Loop && inst.Parameter > 0 && index < Instructions.Count - 1)
            {
                Instruction nextInst = Instructions[index + 1];
                if (inst.Type == nextInst.Type)
                {
                    inst.Parameter += nextInst.Parameter;
                    Instructions.RemoveAt(index + 1);
                }
            }
        }

        int loopCount = Instructions.Count(i => i.Type == InstructionType.Loop);
        int endLoopCount = Instructions.Count(i => i.Type == InstructionType.EndLoop);

        if (loopCount > endLoopCount)
        {
            Instructions.Add(new Instruction(InstructionType.EndLoop));
        }

        backupInstructions = new List<Instruction>(Instructions);
    }

    private class LoopInfo
    {
        public int index;
        public int count;
        public LoopInfo(int index, int count)
        {
            this.index = index;
            this.count = count;
        }
    }
}

public enum InstructionType
{
    Move,
    Grab,
    TurnLeft,
    TurnRight,
    Drop,
    Loop,
    EndLoop,
    Wait,
    Sync
}