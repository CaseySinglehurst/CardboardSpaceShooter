using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    public bool doesntWarp;

    public float maxHealth;
    [HideInInspector]
    public float health; // health enemy starts with ¦¦ current health
    public int reward; // score and money player recieves after killing enemy
    public GameObject healthMaterial; // every ship has a material somewhere on them to convey their health that goes from green to red as the enemy loses health

    [HideInInspector]
    public bool warped = false; // if ship is still warping (controlled by warpin script)
    WarpIn warpedScript; // reference to the warpin script

    public GameObject explosion;

    private float HealthIndicatorChangeSpeed = 1;

    private void Start()
    {
        health = maxHealth;
        if (!doesntWarp) { warpedScript = GetComponent<WarpIn>(); }
    }

    private void Update()
    {
        if (healthMaterial != null) { HandleHealthMaterial(); }
        if (!doesntWarp) { CheckIfWarped(); }
        CheckIfDead();
    }


    // as the enemy loses health, healthmaterial gets less green and more red
    private void HandleHealthMaterial()
    {
        float newRed =255 - ((health/maxHealth) * 255);
        float newGreen =  ((health / maxHealth) * 255);
        healthMaterial.GetComponent<Renderer>().material.color = new Color (newRed/255,newGreen/255,0);
    }

    //check if the warpedin script has finished its warp animation behaviour
    void CheckIfWarped()
    {
        if (warpedScript.warped)
        {
            Destroy(warpedScript);
            warped = true;
        }
    }

    // check if the ship has no health left
    void CheckIfDead()
    {
        if (health <= 0)
        {

            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().money += reward; // give player money
            GameObject e = Instantiate(explosion, transform.position, transform.rotation); // create the death explosion
            Destroy(e, 5f);
            Destroy(transform.parent.gameObject);
        }
    }

}
