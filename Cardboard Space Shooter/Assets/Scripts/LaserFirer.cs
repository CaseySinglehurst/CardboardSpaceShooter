using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaserFirer : MonoBehaviour {

    public GameObject laserPrefab; // laser gameobject
    GameObject controller;

    public float laserCooldown = 1, laserDamage = 4; // how often player can fire ¦¦ how much damage the laser does to an enemy
    private float  currentCooldown = 0, laserLife = 10; //  current time left to be able to fire ¦¦ how long the laser lasts, representing range of laser 

    public TextMeshProUGUI damageText, cooldownText, damageButtonText,cooldownButtonText; // right hand side gui showing: damage of the laser ¦¦ cooldown of the laser

    int damageLevel = 0, cooldownLevel = 0;
    int damageUpgradeCost = 0, cooldownUpgradeCost = 0;
    


    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        UpgradeDamage();
        UpgradeCooldown();

    }

    // Update is called once per frame
    void Update () {
        currentCooldown -= Time.deltaTime; // decrease time left to fire
        if ((Input.touchCount > 0 || Input.GetMouseButton(0)) && currentCooldown <= 0) // if touching the screen, meaning when the player is pressing the cardboard viewer button (also if mouse click for testing purposes)
        {
            Transform facing = this.transform.Find("MainCamera").transform; // get the transform of the player camera, meaning where the player is looking
            GameObject laser = Instantiate(laserPrefab, facing.position, facing.rotation); // create a laser at the player position, with the same rotation as the player camera
            laser.GetComponent<LaserController>().laserDamage = laserDamage; // set the laser damage
            Destroy(laser, laserLife);
            currentCooldown = laserCooldown; // reset the laser cooldown
        }
        
	}

    public void UpgradeDamage()
    {
        if (controller.GetComponent<GameController>().money >= damageUpgradeCost && damageLevel < 10)
        {
            controller.GetComponent<GameController>().money -= damageUpgradeCost;
            damageLevel += 1;
            UpdateDamage(damageLevel);
            if (damageLevel == 10) {
                damageUpgradeCost = 1000000000;
                damageText.text = "X";
                
            }
            else
            {
                UpdateDamageCost(damageLevel);
                damageText.text = "Damage: " + laserDamage.ToString();
                damageButtonText.text = "Upgrade Damage (" + damageUpgradeCost + ")";
            }
        }

    }

    public void UpgradeCooldown()
    {
        if (controller.GetComponent<GameController>().money >= cooldownUpgradeCost && cooldownLevel < 10)
        {
            controller.GetComponent<GameController>().money -= cooldownUpgradeCost;
            cooldownLevel += 1;
            UpdateCooldown(cooldownLevel);
            if (cooldownLevel == 10)
            {
                cooldownUpgradeCost = 1000000000;
                cooldownText.text = "X";

            }
            else
            {

                
                UpdateCooldownCost(cooldownLevel);
                cooldownText.text = "Cooldown: " + laserCooldown.ToString();
                cooldownButtonText.text = "Upgrade Cooldown (" + cooldownUpgradeCost + ")";
            }
        }
    }

    private void UpdateDamage(int level)
    {
        laserDamage = Mathf.Floor(4f + 7.5f * Mathf.Log(level));
    }

    private void UpdateCooldown(int level)
    {
        laserCooldown = 1 - (( (float)level - 1f) / 10f);
    }

    private void UpdateDamageCost(int level)
    {
        damageUpgradeCost = Mathf.FloorToInt(50 * Mathf.Pow(level, 1.5f));
    }

    private void UpdateCooldownCost(int level)
    {
        cooldownUpgradeCost = Mathf.FloorToInt(50 * Mathf.Pow(level, 1.5f));
    }
}
