using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpIn : MonoBehaviour {

    float warpSpeed = 2500, warpScale = 35; // speed when warping ¦¦ model stretch factor when warping
     public float  brakeSpeed = 20; // start slowing down when this far away from "warpInLookingAt" ¦¦ how fast ship slows down
    float speed; // current speed of ship

    public float warpDistance, distanceTravelled; // The distance from spawn to warp point ¦¦ the distance ship has travelled away from spawn point


    public float regularSpeed = 5; // speed of ship when no longer warping

    [HideInInspector]
    public bool warped = false; // if the ship is warping
    private bool stopping = false;

    Vector3 pPos; // position of the player


    public Transform shipParent; // transform of parent container for ship
    [HideInInspector]
    public Vector3 warpInLookingAt,spawnAt; // target position for ship to warp into ¦¦ position ship spawns at

    public AudioSource warpSound;
    bool soundPlayed = false;

    // Use this for initialization
    void Start () {
        pPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        shipParent.LookAt(warpInLookingAt);
        speed = warpSpeed;
        shipParent.localScale = new Vector3(1, 1, warpScale); // scale the ship to the warp factor scale
        spawnAt = shipParent.transform.position;
        warpDistance = Vector3.Distance(spawnAt, warpInLookingAt);
    }
	
	// Update is called once per frame
	void Update () {
        shipParent.Translate(Vector3.forward * speed * Time.deltaTime);
        handleWarp();
    }

    //Scales the ship when warping & checks when in range of target to stop warping
    void handleWarp()
    {
        distanceTravelled = Vector3.Distance(spawnAt, shipParent.transform.position);
        
        if (distanceTravelled > warpDistance ) // IF ship has gone past warp in target
        {
            if (!stopping) // if we're already currently stopping
            {
                shipParent.transform.position = warpInLookingAt; // ship position is warp target
                stopping = true; // start stopping
            }
            else
            {
                speed = Mathf.Lerp(speed, regularSpeed, brakeSpeed * Time.deltaTime); // slow ship down to regularSpeed
                shipParent.localScale = new Vector3(1, 1, Mathf.Lerp(shipParent.localScale.z, 1, brakeSpeed * Time.deltaTime)); // scale ship back to normal scale
                warpSound.pitch = (brakeSpeed / 10) + Random.Range(-.1f, .1f); 
                if (!soundPlayed) { warpSound.Play(); soundPlayed = true; } // play warp speed 
            }
        }
        if (Mathf.Abs(speed - regularSpeed) < 0.1) // IF ship is normal speed IE warped has stoppeds
        {
            warped = true;
            
        }
    }
}
