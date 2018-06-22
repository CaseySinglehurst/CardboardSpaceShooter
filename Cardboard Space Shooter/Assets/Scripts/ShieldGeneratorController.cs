using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGeneratorController : MonoBehaviour {

    GameController gc;
    private float shieldAmount = 10;

	// Use this for initialization
	void Start () {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gc.maxShield += shieldAmount;
	}
	
	
}
