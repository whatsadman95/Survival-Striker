
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public float waitToRespawn;
	public PlayerController thePlayer;

	public GameObject deathSplosion;

	public int coinCount;
    private int coinBonusLifeCount;


	public Text coinText;

	public Image heart1;
	public Image heart2;
	public Image heart3;

	public Sprite heartFull;
	public Sprite heartHalf;
	public Sprite heartEmpty;

	public int maxHealth;
	public int healthCount;

	private bool respawning;
	public ResetOnRespawn[] objectsToReset;    //array is here and this array captures objects from the "ResetToRespawn.cs" Script

    public bool invincible;

    public Text livesText;
    public int startingLives;
    public int currentLives;
    public GameObject gameOverScreen;
    public int bonusLifeThreshold;

    public AudioSource coinSound;

    public AudioSource levelMusic;
    public AudioSource gameOverMusic;


	// Use this for initialization
	void Start () {
		thePlayer = FindObjectOfType<PlayerController> ();

		

		healthCount = maxHealth;

		objectsToReset = FindObjectsOfType<ResetOnRespawn> ();   //finding objects to reset through External reset script attached with objects

        if(PlayerPrefs.HasKey("Coincount"))
        {
            coinCount = PlayerPrefs.GetInt("CoinCount");
        }

        if(PlayerPrefs.HasKey("PlayerLives"))
        {
            currentLives = PlayerPrefs.GetInt("PlayerLives");
        }
        else
        {
            currentLives = startingLives;
        }

        coinText.text = "Coins: " + coinCount;  //displays coin counts in the UI text

        currentLives = startingLives;
        livesText.text = " Lives x " + currentLives;  //display lives info in UI
	}
	
	// Update is called once per frame
	void Update () {

		if (healthCount <= 0 && !respawning) {  //respawn if health is zero
		
			Respawn ();
			respawning = true;
		}

        if (coinBonusLifeCount >= bonusLifeThreshold)
        {
            currentLives += 1;
            livesText.text = " Lives x " + currentLives;
            coinBonusLifeCount -= bonusLifeThreshold; //taking away 100 coins , this is secret counter for bonus life! and it is NOT related with main coin counter
        }
	}

	public void Respawn(){

        currentLives -= 1;
        livesText.text = " Lives x " + currentLives;   //display lives after respawn info in UI

        if(currentLives > 0)
        {
            StartCoroutine("RespawnCo");   //redirecting to coroutine function

        } else
        {
            thePlayer.gameObject.SetActive(false);
            gameOverScreen.SetActive(true);
            levelMusic.Stop();
            gameOverMusic.Play();

            

        }

        


	}

	public IEnumerator RespawnCo()     //coroutine for delay in respawn
	{
		thePlayer.gameObject.SetActive(false);

		Instantiate (deathSplosion, thePlayer.transform.position, thePlayer.transform.rotation);

		yield return new WaitForSeconds (waitToRespawn); //if you want to freeze the game for second, **THIS IS THE CODE** , "waitToRespawn" is variable may set by user.

		healthCount = maxHealth;
		respawning = false;
		UpdateHeartMeter ();

		coinCount = 0;
        
		coinText.text = "Coins: " + coinCount;
        coinBonusLifeCount = 0;

        thePlayer.transform.position = thePlayer.respawnPosition;
		thePlayer.gameObject.SetActive(true);

		for (int i = 0; i < objectsToReset.Length; i++) 
		{
            objectsToReset[i].gameObject.SetActive(true);
            objectsToReset[i].ResetObject();
  
		}

	}

	public void AddCoins(int coinsToAdd)   //coin add function 
	{
		coinCount += coinsToAdd;
        coinBonusLifeCount += coinsToAdd;

		coinText.text = "Coins: " + coinCount;  //displays coin amount in UI
        coinSound.Play();
	}

	public void HurtPlayer (int damageToTake)  //hurting function
	{   

        if(!invincible)
        { 

		    healthCount -= damageToTake;
		    UpdateHeartMeter ();

            thePlayer.Knockback();

            thePlayer.hurtSound.Play();
        }

    }

    public void GiveHealth(int healthToGive)
    {
        healthCount += healthToGive;  //adds bonus health to current health 

        if (healthCount > maxHealth)
        {
            healthCount = maxHealth;
        }
        UpdateHeartMeter();  //updates visual heartmeter 

        coinSound.Play();
    }


	public void UpdateHeartMeter()
	{
		switch (healthCount) 
		{
		case 6:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			return;

		case 5:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartHalf;
			return;

		case 4:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartEmpty;
			return;

		case 3:
			heart1.sprite = heartFull;
			heart2.sprite = heartHalf;
			heart3.sprite = heartEmpty;
			return;

		case 2:
			heart1.sprite = heartFull;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			return;
			
		case 1:
			heart1.sprite = heartHalf;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			return;

		case 0:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			return;

		default:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			return;
			

		}

	}

    public void AddLives(int livesToAdd)
    {
        coinSound.Play();
        currentLives += livesToAdd;
        livesText.text = " Lives x " + currentLives;
        
    }

}
