using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    
    public float health = 100, maxHealth = 100; // current player health ¦¦ health player starts with, maximum player can have
    public float shield = 0, maxShield = 0;
    private float shieldCooldown = 5, currentShieldCooldown, shieldRechargeRate = 1;

    public TextMeshPro moneyText; // text to display player score
    public Image healthBar, shieldBar; // image to represent player health
    public GameObject GameOverUI; // canvas displaying the game over text and replay button
    
    public int money; // player money

    public TextMeshProUGUI gameOverText;

    // Use this for initialization
	void Start () {
        Time.timeScale = 1; // set the game to play at regular speed
        shieldBar.fillAmount = 0;
	}

    private void Update()
    {
        UpdateShield();
    }

    // Update is called once per frame
    void LateUpdate () {
        moneyText.text = "Money: " + money.ToString(); // sets the score text to player score
        healthBar.fillAmount = health / maxHealth; // sets the healthbar to scale with the player health
        if (maxShield > 0) { shieldBar.fillAmount = shield / maxShield; } // sets the shield bar to scale with the player shield if the player has any shield
        CheckIfGameOver();
        
}
    // checks if the player has no health and if so stops the game and displays game over text and replay button
    void CheckIfGameOver()
    {
        if (health <= 0 && !GameOverUI.activeInHierarchy) // IF no health left AND game over canvas hasnt already been activated
        {
            GameOverUI.SetActive(true);
            Time.timeScale = 0; // stop the game
        }
    }

    public void YouWin()
    {
        GameOverUI.SetActive(true);
        gameOverText.text = "You Win!";
        Time.timeScale = 0;
    }

    // does damage H to the player, which damages shield first and then health if there is no shield left
    public void DoDamage(float h)
    {
        if (shield >= h) // if dealing less damage than current shield
        {
            shield -= h; // take damage away from shield
        }
        else
        {
            h -= shield; // find damage left after depleting shield
            shield = 0;
            health -= h; // do the rest of the damage to health
        }

        currentShieldCooldown = shieldCooldown;
    }
    public void AddShield(float s) { maxShield += s; }
    
    public void ReloadGame()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PlayScene");

        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null; // do nothing
        }
    }


    void UpdateShield()
    {
        currentShieldCooldown -= Time.deltaTime;
        if (currentShieldCooldown <= 0 && shield < maxShield)
        {
            shield += shieldRechargeRate * Time.deltaTime;
            if (shield > maxShield) { shield = maxShield; }
        }
    }

}
