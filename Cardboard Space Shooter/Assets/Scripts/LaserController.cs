using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    float laserSpeed = 240;
    public float laserDamage = 0;

    AudioSource laserSound;


    private void Start()
    {
        laserSound = GetComponent<AudioSource>();
        laserSound.pitch = Random.Range(.5f, 1.5f);
        laserSound.Play();
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.forward * laserSpeed * Time.deltaTime);
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.GetComponent<EnemyStats>().health -= laserDamage; // do damage based on LaserFirer damage (set by Laser Firer)
            Destroy(transform.gameObject);
        }
    }

    
}
