using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AIscript_BrandonBost : MonoBehaviour {

    public CharacterScript mainScript;

    public float[] bombSpeeds;
    public float[] buttonCooldowns;
    public float playerSpeed;
    public int[] beltDirections;
    public float[] buttonLocations;
    public float opponentLoc;
    public float playerLoc;

    public uint directUp = 0b_0;
    public float lastLoc = 0;
    public int switchWait = 0;
    public int timer = 0;

	// Use this for initialization
	void Start () {
        mainScript = GetComponent<CharacterScript>();

        if (mainScript == null)
        {
            //Debug.Log("No CharacterScript found on " + gameObject.name);
            this.enabled = false;
        }

        buttonLocations = mainScript.getButtonLocations();
        playerSpeed = mainScript.getPlayerSpeed();
        playerLoc = mainScript.getCharacterLocation();
        //directUp = ~directUp;

	}

    void Movement(){
        if (directUp != 0b_0){
            mainScript.moveUp();
        } 
        else{
            mainScript.moveDown();
        }
    }

    void ButtonSearch(){
        int target = 0;
        bool picked = false;
        for(int i = 0; i < buttonLocations.Length; i++){
            Debug.Log("<color=purple> cuurent button cooldown"+buttonCooldowns[i]+"</color>");
            
            if (buttonCooldowns[i] <= 0 && beltDirections[i] == -1){
                target = i; // button at index become prefered state.
                picked = true;
            }
            if(Math.Abs(playerLoc - buttonLocations[i]) < 1 && beltDirections[i] != 1 && buttonCooldowns[i] <= 0){
                mainScript.push(); // push button if approaches a candidate enroute
            }
            else if (buttonLocations[target] < playerLoc && picked) {
                directUp = 0b_0;
            }
            else if (buttonLocations[target] > playerLoc && picked){
                directUp = 0b_1;
            }
        }
    }

	// Update is called once per frame
	void Update () {

        buttonCooldowns = mainScript.getButtonCooldowns();
        beltDirections = mainScript.getBeltDirections();
        opponentLoc = mainScript.getOpponentLocation();
        playerLoc = mainScript.getCharacterLocation();

        
        //Your AI code goes here
        /* 
        foreach(float x in buttonLocations){
            //Debug.Log("<color=green>"+x+"</color>");
        }
        
        */
        if (lastLoc == playerLoc){
            directUp = ~directUp;
        }

        Debug.Log("<color=green>"+directUp+"</color>");

        // prevent player lock in the case of no prefered targets
        if (timer > 7){
            Debug.Log("<color=red>"+playerLoc+ ", "+lastLoc+"</color>");
            lastLoc = playerLoc;
            timer = 0;
        }
        else{
            timer++;
            lastLoc = 0;
        }
        Movement();
        ButtonSearch();
	}
}
