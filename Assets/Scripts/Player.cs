using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script must be used as the core Player script for managing the player character in the game.
/// </summary>
public class Player : MonoBehaviour
{
    

    public Vector2 startingPosition;
    public int playerTotalLives = 5; //Players total possible lives.
    public int playerLivesRemaining; //PLayers actual lives remaining.
    public int playerScore;
   
    public bool playerIsAlive = true; //Is the player currently alive?
    public bool playerCanMove = false; //Can the player currently move?

    public bool isOnPlatform = false; // is the playe on a platform
    public bool isInWater = false; // Is the player in the water
    public bool hasKey = false; // has the player obtained a key
    public bool isWin = false; // has the player won
   

    public AudioClip jumpSound; // plays upon jump input
    public AudioClip deathSound;// plays when player dies

    public GameObject explosionEffect; // plays when player dies

    private GameManager myGameManager; //A reference to the GameManager in the scene.

    private AudioSource gameAudio;
    // Start is called before the first frame update
    public MainMenu myUI;

    void Start()
    {
        myGameManager = GameObject.FindObjectOfType<GameManager>();
        gameAudio = GetComponent<AudioSource>();

        transform.position = startingPosition;

        playerTotalLives = 5;
        playerScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        // player movement
            if (playerIsAlive && playerTotalLives > 0)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y < myGameManager.levelConstraintTop)
                {
                    transform.Translate(new Vector2(0, 0.92f));
                    gameAudio.PlayOneShot(jumpSound);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y > myGameManager.levelConstraintBottom)
                {
                    transform.Translate(new Vector2(0, -0.92f));
                    gameAudio.PlayOneShot(jumpSound);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > myGameManager.levelConstraintLeft)
                {
                    transform.Translate(new Vector2(-0.92f, 0));
                    gameAudio.PlayOneShot(jumpSound);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < myGameManager.levelConstraintRight)
                {
                    transform.Translate(new Vector2(0.92f, 0));
                    gameAudio.PlayOneShot(jumpSound);
                }

                // test for player winning
                if (playerScore == 5050)
                {
                    isWin = true;
                    myGameManager.GameOver(isWin);
                }
            }
        
        
    }

    private void LateUpdate()
    {
        if(playerIsAlive)
        {
            if (isInWater && !isOnPlatform) // kill player in the water
                KillPlayer();

        }
    }

    // tests various scenarios where the player encounters objects
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerIsAlive)
        {
            if (collision.transform.GetComponent<Vehicle>() != null) // collides with verhicle object
            {
                // kill player and restart his position
                transform.position = startingPosition;
                //KillPlayer();
                GetComponent<SpriteRenderer>().enabled = false; // disable the sprite
                Instantiate(explosionEffect, transform.position, Quaternion.identity); // explode effect
                gameAudio.PlayOneShot(deathSound); // death sound plays
                playerTotalLives--; // reduce lives by one
                if (playerTotalLives != 0)
                    GetComponent<SpriteRenderer>().enabled = true; // reenabl sprite
                else
                    myGameManager.GameOver(isWin); // show win screen

            }
            else if (collision.transform.GetComponent<Platform>() != null) // check if colliding with platform
            {
                transform.SetParent(collision.transform);
                isOnPlatform = true;
            }
            else if(collision.transform.tag == "Water") // set player to is in water
            {
                //KillPlayer();
                isInWater = true;
            }
            else if(collision.transform.tag == "Key") // player has collided with a key
            {
                if(hasKey == false)
                {
                    // update score and destroy object
                    myGameManager.UpdateScore(10);
                    playerScore += 10;
                    hasKey = true;
                    Destroy(collision.gameObject);
                }
                hasKey = true;
            }

            // check if player is in goal
            else if (collision.transform.tag == "Goal" && hasKey == true)
            {
                myGameManager.UpdateScore(1000);// update score
                playerScore += 1000;
                hasKey = false;
                Destroy(collision.gameObject);

                transform.position = startingPosition;
            }
        } 
    }

    // checking if player is in the water when leaving a platform
    void OnTriggerExit2D(Collider2D collision)
    {
        if (playerIsAlive)
        {
            if (collision.transform.GetComponent<Platform>() != null)
            {
                transform.SetParent(null);
                isOnPlatform = false;
            }
            else if (collision.transform.tag == "Water")
            {
                //KillPlayer();
                isInWater = false;
            }
        }
    }

    // restart function
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }


    // kills player
    void KillPlayer()
    {
        
        if (playerIsAlive)
        {
            GetComponent<SpriteRenderer>().enabled = false; // disable sprite
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            gameAudio.PlayOneShot(deathSound);
            
            playerTotalLives--;
            

        }
        if (playerTotalLives != 0)
        {
            playerIsAlive = false;
            playerCanMove = false;

        }
        else
            myGameManager.GameOver(isWin);



    }

}
