using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleShipController : MonoBehaviour {

    public Transform[] cannonPoints = new Transform[5];
    public GameObject mine, cannonExplosion;


    public float mineCooldown;
    float currentMineCooldown;

    EnemyStats stats; // stats script that hold warp script & health etx

    // Use this for initialization
    void Start () {
        currentMineCooldown = mineCooldown;
        stats = GetComponent<EnemyStats>();
    }
	
	// Update is called once per frame
	void Update () {


        if (stats.warped) { currentMineCooldown -= Time.deltaTime; }

        if (currentMineCooldown <= 0)
        {
            fireMines();
            currentMineCooldown = mineCooldown;
        }
	}


    void fireMines()
    {
        for (int i = 0; i <= 4; i++)
        {
            Instantiate(mine, cannonPoints[i].position, cannonPoints[i].rotation);
            GameObject e = Instantiate(cannonExplosion, cannonPoints[i].position, cannonPoints[i].rotation);
            Destroy(e, 5f);
        }
    }
}
