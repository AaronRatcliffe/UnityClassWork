using UnityEngine;
using System.Collections;

public class AIScript_AaronRatcliffe : MonoBehaviour {

    public CharacterScript mainScript;

    public float[] bombSpeeds;
    public float[] buttonCooldowns;
    public float playerSpeed;
    public int[] beltDirections;
    public float[] buttonLocations;
    public float[] bombDistance;
    public int targetButtonIndex = 4;

    // Use this for initialization
    void Start () {
        mainScript = GetComponent<CharacterScript>();

        if (mainScript == null)
        {
            print("No CharacterScript found on " + gameObject.name);
            this.enabled = false;
        }

        buttonLocations = mainScript.getButtonLocations();

        playerSpeed = mainScript.getPlayerSpeed();
	}

	// Update is called once per frame
	void Update () {

        buttonCooldowns = mainScript.getButtonCooldowns();
        beltDirections = mainScript.getBeltDirections();
        bombSpeeds = mainScript.getBombSpeeds();
        bombDistance = mainScript.getBombDistances();

        //Your AI code goes here
        int nextTarget = 0;
        int bestHuristic = 0;
        int curentHuristic = 0;

        float distanceToButton;

        int buttonsBeforTarget = 0;
        float buttonPushDelay = 0.7f;
        float distanceToTarget = Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[targetButtonIndex]);

        if (distanceToTarget <= 0.5)
        {
            for (int i = 0; i < buttonLocations.Length; i++)
        {
            //Debug.Log("---------------");
            //print("cheking button " + i);
            curentHuristic = 0;
            distanceToButton = Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[i]);
            buttonsBeforTarget = (int)(distanceToButton);
            //cheking to see if the button is targetable
            if (buttonCooldowns[i] <= 0 && beltDirections[i] != 1) {
                curentHuristic += 10;
                //cheking to see if you can get to the button befor the bomb explodes
                if ((distanceToButton / playerSpeed) + (buttonsBeforTarget * buttonPushDelay) < (bombDistance[i] / bombSpeeds[i]))
                {
                    curentHuristic += 10;
                    //compairing how close the bomb is to exploding
                    curentHuristic += 20 - (int)(bombDistance[i] / bombSpeeds[i]);

                    //cheking to see wether you are player officivle or devinsivly
                    if (Mathf.Abs(mainScript.getCharacterLocation() - mainScript.getOpponentLocation()) < 4) {
                        //print("Aggressive Script");
                        
                        //scailing an addition by how far aways the button is from the player
                        curentHuristic += Mathf.Abs(targetButtonIndex - i);

                    }
                    else
                    {
                        //print ("Definsive Script");
                        curentHuristic += 8 - Mathf.Abs(targetButtonIndex - i);
                    }
                }
            }
            //print("Huresitc = " + curentHuristic);
            //setting the next target to the button with the hightest huristic
            if (curentHuristic > bestHuristic)
            {
                //print("next target = "+ i);
                nextTarget = i;
                bestHuristic = curentHuristic;
            }

        }

            //mainScript.push();
            //print ("setting new target to next target index of = "+ nextTarget);
            targetButtonIndex = nextTarget;
        }

        if (buttonLocations[targetButtonIndex] < mainScript.getCharacterLocation())
        {
            mainScript.moveDown(); 
        }
        else
        {
            mainScript.moveUp();
        }

        mainScript.push();
    }
}
