using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCruiserController : MonoBehaviour {

    EnemyStats stats; // stats script that hold warp script & health etx
    GameController gc; // game controller for money and health stats

    LineRenderer beam; // laser renderer
    Transform player; // position of camera

    bool isFiring = false; // whether the laser should be visible and doing damage

    public float beamDuration = 2, currentBeamDuration, beamDamage = 20, beamCooldown = 10, currentBeamCooldown; // duration of the laser ¦ how long the laser has been enabled for ¦ how much damage the laser does over its duration ¦ how long in between laser shots ¦ how long it has been since the last laser shot

	// Use this for initialization
	void Start () {
        stats = GetComponent<EnemyStats>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        beam = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentBeamDuration = beamDuration; // current laser life is full laser life
        currentBeamCooldown = beamCooldown; // current laser cooldown is full laser cooldown
	}
	
	// Update is called once per frame
	void Update () {

        if (stats.warped) { HandleBeam(); } // IF enemy has warped in, begin the laser behaviour

        Vector3 beamTarget;
        if (isFiring) { beamTarget = new Vector3(player.position.x , player.position.y - 10, player.position.z + 20); } // IF firing, laser vector is slightly infront of player
        else { beamTarget = transform.position; } // else its within the ship, meaning no laser
        
        beam.SetPosition(0, transform.position); // set the first point of the laser to be the middle of the ship
        beam.SetPosition(1, beamTarget); // set the last point of the laser to be the current target decided above
	}
    //Fires the beam if cooldown is 0 and handles beam behaviour ie. damage, duration
    void HandleBeam()
    {
        currentBeamCooldown -= Time.deltaTime;
        if (currentBeamCooldown <= 0) // if no time left before next beam
        {
            isFiring = true;
            currentBeamDuration = beamDuration;
            currentBeamCooldown = beamCooldown;
        }
        if (isFiring)
        {
            DealDamage();
            HandleBeamFX();
            currentBeamDuration -= Time.deltaTime;
            if (currentBeamDuration <= 0) // if laser has finished firing
            {
                isFiring = false;
                currentBeamDuration = beamDuration; // cooldown goes back to full
            }
        }

    }

    //deals damage to the player relative to the duration 
    void DealDamage()
    {

        float deltaDamage = (beamDamage / beamDuration) * Time.deltaTime; // deal damage as a function of total damage over time
        gc.DoDamage(deltaDamage); // decrease player health by delta damage
    }
    // deals with the look of the beam: laser jitter
    void HandleBeamFX()
    {
        float beamWidth = Random.Range(3f, 5f);
        beam.endWidth = beamWidth; // jitter the laser width randomly between two points
    }


}
