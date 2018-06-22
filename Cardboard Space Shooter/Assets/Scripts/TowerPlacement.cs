using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour {

    public Material canPlaceMat, canNotPlaceMat;
    int amountOfCurrentCollisions = 0;

    private Renderer baseRenderer;
        private Renderer[] gunRenderer;

    bool canPlace = false;
    public bool isFlak;

	// Use this for initialization
	void Start () {
        baseRenderer = GetComponent<Renderer>();
        if (isFlak)
        { gunRenderer = GetComponentsInChildren<Renderer>(); }
	}

    private void OnEnable()
    {
        amountOfCurrentCollisions = 0;
    }

    // Update is called once per frame
    void Update () {
        if (CheckIfTouching() && canPlace)
        {
            
            baseRenderer.material = canNotPlaceMat;
            if (isFlak)
            { gunRenderer[1].material = canNotPlaceMat; }
            canPlace = false;
        }
        else if ( !CheckIfTouching() && !canPlace && isActiveAndEnabled)
        {
              
            baseRenderer.material = canPlaceMat;
            if (isFlak) { gunRenderer[1].material = canPlaceMat; }
            canPlace = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tower")
        {
            
            amountOfCurrentCollisions++;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tower")
        {
            
            amountOfCurrentCollisions--;
        }
    }

    public bool CheckIfTouching()
    {
        if (amountOfCurrentCollisions == 0) { return false; }
        else { return true; }
    }
}
