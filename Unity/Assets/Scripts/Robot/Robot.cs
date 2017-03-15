using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Robot : MonoBehaviour 
{

	#region Fields

    public GameObject RockPrefab;
    public GameObject SolidRockPrefab;
    public InstructionSet instructionSet;

    public Vector2 spawn;
    public Quaternion spawnRotation;

    private Animator animator;

    private Dictionary<InstructionType, Action<float>> progressActions;
    private Dictionary<InstructionType, Action> stepActions;
    private Dictionary<InstructionType, Action> startActions;

    private Instruction currentInstruction;
    private StepState stepState;

    private bool isHolding;
    private bool isGrabbing;
    private bool isDropping;
    private Transform Rock;

    private GameObject selection;
    private GameObject grabber;
	
	#endregion
	
	
	#region Constants
	
	
	
	#endregion
	
	
	#region Properties
	
	
	
	#endregion
	

	#region Behaviours

	private void Awake()
	{
        progressActions = new Dictionary<InstructionType, Action<float>>
        {
            {InstructionType.Move, MoveProgress},
            {InstructionType.Grab, GrabProgress},
            {InstructionType.Drop, DropProgress},
            {InstructionType.TurnLeft, f => TurnProgress(f, 1)},
            {InstructionType.TurnRight, f => TurnProgress(f, -1)},
        };

        stepActions = new Dictionary<InstructionType, Action>
        {
            {InstructionType.Move, MoveStep},
            {InstructionType.Grab, GrabStep},
            {InstructionType.Drop, DropStep},
            {InstructionType.TurnLeft, () => TurnStep(1)},
            {InstructionType.TurnRight, () => TurnStep(-1)},
        };

        startActions = new Dictionary<InstructionType, Action>
        {
            {InstructionType.Grab, StartGrab},
            {InstructionType.Drop, StartDrop},
        };

        instructionSet = new InstructionSet();
        animator = GetComponent<Animator>();
        spawn = transform.position;
        spawnRotation = transform.rotation;

        selection = transform.Find("Selection").gameObject;
        grabber = transform.Find("Grabber").gameObject;
	}

    private void Update()
    {
        selection.SetActive(Game.CurrentRobot == this);
        grabber.SetActive(isGrabbing);
    }

    private void OnMouseDown()
    {
        Game.CurrentRobot = this;
        GameObject.Find("HUD Canvas").BroadcastMessage("Refresh");
    }

    private void Restart()
    {
        isHolding = false;
        transform.position = spawn;
        transform.rotation = spawnRotation;
        currentInstruction = null;
        instructionSet.Restart();
        Step();
        Pause();
    }

    private void Step()
    {
        if (currentInstruction != null && stepActions.ContainsKey(currentInstruction.Type))
        {
            stepActions[currentInstruction.Type]();
        }

        currentInstruction = instructionSet.Next();
        if (currentInstruction == null)
        {
            Animate();
            return;
        }

        stepState = new StepState()
        {
            Position = transform.position2D(),
            Rotation = transform.rotation
        };

        if (startActions.ContainsKey(currentInstruction.Type))
        {
            startActions[currentInstruction.Type]();
        }

        Animate();
    }

    #region Start

    private void StartGrab()
    {
        isGrabbing = false;
        if (isHolding) return;

        foreach (Vector2 direction in Enum.GetValues(typeof(CardinalDirections)).Cast<CardinalDirections>().Select(c => c.AsVector2()))
        {
            Vector2 grabPosition = transform.position2D() + direction;
            Collider2D collide = Physics2D.OverlapPoint(grabPosition);
            if (collide == null) continue;
            
            IGrabbable grabbable = collide.GetComponent(typeof(IGrabbable)) as IGrabbable;

            if (grabbable != null && grabbable.CanGrab())
            {
                stepState.GrabPosition = grabPosition;
                isGrabbing = true;
                Rock = ((GameObject) GameObject.Instantiate(RockPrefab, grabPosition, Quaternion.identity)).transform;
                grabbable.OnGrab();
                break;
            }
        }
    }

    private void StartDrop()
    {
        isDropping = false;
        if (!isHolding) return;

        Vector2 dropPosition = transform.position + transform.up;
        stepState.DropTarget = dropPosition;
        isDropping = true;
        Rock = ((GameObject)GameObject.Instantiate(RockPrefab, transform.position, Quaternion.identity)).transform;
    }

    #endregion

    #region Progress
    private void StepProgress(float progress)
    {
        if (currentInstruction != null && progressActions.ContainsKey(currentInstruction.Type))
        {
            progressActions[currentInstruction.Type](progress);
        }
    }

    private void MoveProgress(float progress)
    {
        transform.position = stepState.Position + (Vector2) (transform.rotation * Vector2.up * progress);
    }

    private void GrabProgress(float progress)
    {
        if (isGrabbing)
        {
            Rock.position = Vector2.Lerp(stepState.GrabPosition, transform.position, progress);
        }
    }

    private void DropProgress(float progress)
    {
        if (isDropping)
        {
            Rock.position = Vector2.Lerp(transform.position, stepState.DropTarget, progress);
        }
    }

    private void TurnProgress(float progress, int direction)
    {
        transform.rotation = stepState.Rotation * Quaternion.Euler(0, 0, 90 * progress * direction);
    }
    #endregion

    #region Step
    private void MoveStep()
    {
        transform.position = stepState.Position + (Vector2) (transform.rotation * Vector2.up);
    }

    private void GrabStep()
    {
        if (isGrabbing)
        {
            GameObject.Destroy(Rock.gameObject);
            isHolding = true;
        }
        isGrabbing = false;
    }

    private void DropStep()
    {
        if (!isDropping) return;
        isDropping = true;

        Collider2D collide = Physics2D.OverlapPoint(stepState.DropTarget);
        isHolding = false;

        if (collide == null)
        {
            Rock.SendMessage("SetSolid", true);
        }
        else
        {
            IDroppable droppable = collide.GetComponent(typeof(IDroppable)) as IDroppable;
            if (droppable != null)
            {
                StepManager.OnEndOfStep += (Action) (() =>
                {
                    GameObject.Destroy(Rock.gameObject);
                    droppable.OnDrop();
                });
            }
            else
            {
                Game.Fail();
            }
        }
    }

    private void TurnStep(int direction)
    {
        transform.rotation = stepState.Rotation * Quaternion.Euler(0, 0, 90 * direction);
    }
    #endregion

    #endregion


    #region Public Methods



    #endregion


    #region Private Methods

    private void Pause()
    {
        animator.SetBool("IsMoving", false);
        animator.SetInteger("TurnDirection", 0);
    }

    private void Play()
    {
        Animate();
    }

    private void Animate()
    {
        if (currentInstruction != null)
        {
            animator.SetBool("IsMoving", currentInstruction.Type == InstructionType.Move);

            int turn = 0;
            if (currentInstruction.Type == InstructionType.TurnRight) turn=1;
            if (currentInstruction.Type == InstructionType.TurnLeft) turn=-1;
            animator.SetInteger("TurnDirection", turn);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetInteger("TurnDirection", 0);
        }
    }

    private IEnumerator DropLate(IDroppable droppable)
    {
        yield return new WaitForEndOfFrame();
    }

    #endregion


    private class StepState
    {
        public Vector2 Position;
        public Quaternion Rotation;
        public Vector2 GrabPosition;
        public Vector2 DropTarget;
    }
}

