using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleShipMineController : MonoBehaviour {

    public float speed;
    GameObject target;

    float timeUntilMoveToTarget = 3;

    public GameObject kamikazeExplosion; // gameobject of death explosion and explosion if ship causes damage to the player

    float damage = 10;

    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Target");
    }
	
	// Update is called once per frame
	void Update () {
        timeUntilMoveToTarget -= Time.deltaTime;

        if (timeUntilMoveToTarget > 0) { transform.parent.Translate(Vector3.left * speed * Time.deltaTime); }
        else { MoveTowardsTarget(); }
        CheckIfKamikaze();
	}

    void CheckIfKamikaze()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 3) // IF ship is within 3 units of capital ship
        {
            GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            gc.DoDamage(damage); // reduce player health
            for (int i = 0; i < 3; i++) // create 3 big explosions
            {
                GameObject e = Instantiate(kamikazeExplosion, new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z + Random.Range(-1f, 1f)), transform.rotation);
                Destroy(e, 5f);
            }
            Destroy(transform.parent.gameObject);
        }
    }

    void MoveTowardsTarget()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // move forward
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position),  Time.deltaTime); // rotate ship towards capital ship
    }
}
