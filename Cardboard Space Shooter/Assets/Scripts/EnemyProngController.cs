using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProngController : MonoBehaviour
{

    float speed = 5, rotateSpeed = 1f; // speed of the ship ¦¦ how fast the ship rotates
    GameObject target; // transform just above capital ship where the prong will explode
    EnemyStats stats; // stats of the enemy ie health etc
    public GameObject kamikazeExplosion; // gameobject of death explosion and explosion if ship causes damage to the player
    public Transform frontOfShip; // parent container for ship

    float damage = 10;

    // Use this for initialization
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        target = GameObject.FindGameObjectWithTag("Target");
    }
    // Update is called once per frame
    void Update()
    {
        if (stats.warped) // if we are no longer warping and using prong behavior
        {
            frontOfShip.Translate(Vector3.forward * speed * Time.deltaTime); // move forward
            frontOfShip.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), rotateSpeed * Time.deltaTime); // rotate ship towards capital ship
        }
        CheckIfKamikaze();
    }


    // check if ship is close enough to the capital ship to explode and do damage to the player
    void CheckIfKamikaze()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 3) // IF ship is within 3 units of capital ship
        {
            GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            gc.DoDamage(10); // reduce player health
            for (int i = 0; i < 3; i++) // create 3 big explosions
            {
                GameObject e = Instantiate(kamikazeExplosion, new Vector3(frontOfShip.position.x + Random.Range(-1f, 1f), frontOfShip.position.y + Random.Range(-1f, 1f), frontOfShip.position.z + Random.Range(-1f, 1f)), frontOfShip.rotation);
                Destroy(e, 5f);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
