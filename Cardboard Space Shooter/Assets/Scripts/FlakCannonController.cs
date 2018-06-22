using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakCannonController : MonoBehaviour {


    //Targeting Variables
    public Transform cannon; // barrel of the gun
    public Transform ship; // capital ship that the player is on
    public Transform target; // current target of the cannon
    float rotSpeed = 5, cannonSpeed = 5f; // how fast the housing rotates ¦¦ how fast the cannon rotates
    //Targeting Variables

    //Shooting Variables
    public GameObject flak; // projectile
    private float flakCooldown = 0.2f, currentCooldown = 0,flakDamage, flakLife = 10; // how fast the gun can fire ¦¦ current time left to next shot ¦¦ how long the projectile survives
    float readyToFire = 2; // whether the cannon close enough to facing the target to fire
    public LaserFirer laserFirer;

    //Shooting Variables


    // Use this for initialization
    void Start () {
        ship = GameObject.FindGameObjectWithTag("CapitalShip").transform;
        laserFirer = GameObject.FindGameObjectWithTag("Player").GetComponent<LaserFirer>();
        readyToFire = 2;

	}
	
	// Update is called once per frame
	void Update () {
        flakCooldown = laserFirer.laserCooldown;
        flakDamage = laserFirer.laserDamage;
        currentCooldown -= Time.deltaTime;
        if (target != null){ RotateHousing(target.position); RotateCannon(target.position); readyToFire -= Time.deltaTime; } // if we have a target, rotate to thtat
        else { RotateHousing(ship.position); RotateCannon(ship.position); readyToFire = 2; } // if we dont have a target, rotate towards middle of ship (idle position)
        HandleFiring(readyToFire);

    }
    // rotate the base of the cannon so the front of the cannon is facing the enemy (only moves horizontally)
    void RotateHousing(Vector3 rotateTarget)
    {
        Vector3 lookPoint = rotateTarget - transform.position; // get vector pointing towards target
        Quaternion rot = Quaternion.LookRotation(new Vector3(lookPoint.x, transform.position.y, lookPoint.z), Vector3.up); // gives rot quaternion that looks at the target
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotSpeed * Time.deltaTime); // interpolate cannon rotation between cannon rotation and rot
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); // make sure cannon is only turning horizontally
    }
    //rotates barrel of the cannon so the barel is facing the enemy (only moves vertically)
    void RotateCannon(Vector3 rotateTarget)
    {
        float distance = Vector3.Magnitude(rotateTarget - cannon.position); //gets the distance to target (hypotenuse)
        float heightDifference = rotateTarget.y - cannon.position.y ; // gets height difference between cannon and target (opposite)
        float cannonAngle = Mathf.Rad2Deg * Mathf.Asin(heightDifference / distance); // gets angle of rotation (sin(angle) = o/h -> angle = sin-1 o/h)
        cannon.localRotation = Quaternion.Lerp(cannon.localRotation, Quaternion.Euler(new Vector3(0f, -cannonAngle, 0)), cannonSpeed * Time.deltaTime); // interpolate barrel rotation between barrel rotation and cannon angle while making sure barrel only moves vertically
        
    }
    // if enemy is in sphere collider representing range of cannon
    private void OnTriggerStay(Collider other)
    {
        if(target == null && other.tag == "Enemy")
        {
            readyToFire = 2;
            target = other.transform;
        }
    }
    // checks if cannon can fire, and if so does
    void HandleFiring(float canIFire)
    {
        if (canIFire <= 0 && currentCooldown <= 0) // IF housing is rotated towards target AND cooldown is 0 or less
        {
            GameObject f = Instantiate(flak, cannon.position, cannon.rotation * Quaternion.Euler(0,90,0));
            f.GetComponent<LaserController>().laserDamage = flakDamage;
            Destroy(f, flakLife);
            currentCooldown = flakCooldown; // reset the firing cooldown
        }
    }


}
