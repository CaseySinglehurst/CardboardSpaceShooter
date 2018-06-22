using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurretPlacer : MonoBehaviour {

    bool placingTurret;
    GameObject turretPlacer;
    GameObject turret;
    int turretPrice;
    public GameObject placementGrid;
    int placeLayer ;

    

    #region turrets & placers

    public GameObject flakCannon, flakCannonPlacer;
    public int flakCannonPrice;

    public GameObject shieldGenerator, shieldGeneratorPlacer;
    public int shieldGeneratorPrice;

    #endregion

    GameController gc;

    // Use this for initialization
    void Start () {
        placeLayer = LayerMask.GetMask("TurretPlacement");
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    }
	
	// Update is called once per frame
	void Update () {

        if (placingTurret)
        {
            if (!placementGrid.activeInHierarchy) {  placementGrid.SetActive(true); }
            Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f); // center of the screen
            float rayLength = 500f;
            // actual Ray
            Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
            // debug Ray
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayLength, placeLayer))
            {
                if (!turretPlacer.activeInHierarchy)
                    turretPlacer.SetActive(true);
                turretPlacer.transform.position = hit.point;
                turretPlacer.transform.position = new Vector3(turretPlacer.transform.position.x, turretPlacer.transform.position.y + .5f, turretPlacer.transform.position.z);
                turretPlacer.transform.position = new Vector3(hit.point.x, turretPlacer.transform.position.y, turretPlacer.transform.position.z);

                if (Input.touchCount > 0 || Input.GetMouseButton(0))
                {
                    if (!turretPlacer.GetComponentInChildren<TowerPlacement>().CheckIfTouching())
                    { 
                        Instantiate(turret, turretPlacer.transform.position, turretPlacer.transform.rotation);
                        Destroy(turretPlacer);
                        gc.money -= turretPrice;
                        placingTurret = false;
                    }
                }
            }
            else
            {
                if (turretPlacer.activeInHierarchy) { turretPlacer.SetActive(false); }
                if (Input.touchCount > 0)
                {
                    if ((Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButton(0)) && placingTurret)
                    {
                        Destroy(turretPlacer);
                        placingTurret = false;
                    }
                }
            }
        }
        else
        {
            if (placementGrid.activeInHierarchy) { placementGrid.SetActive(false); }
        }
        
        
    }

    

    public void SelectFlakCannon()
    {
        
        if (gc.money >= flakCannonPrice)
        {
            
            turretPrice = flakCannonPrice;
            GameObject placer = Instantiate(flakCannonPlacer);
            turretPlacer = placer;
            turret = flakCannon;
            placingTurret = true;
        }
    }

    public void SelectShieldGenerator()
    {
        if (gc.money >= shieldGeneratorPrice)
        {

            turretPrice = shieldGeneratorPrice;
            GameObject placer = Instantiate(shieldGeneratorPlacer);
            turretPlacer = placer;
            turret = shieldGenerator;
            placingTurret = true;
        }
    }

}
