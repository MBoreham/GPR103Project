using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

/// <summary>
/// This script must be used as the core Player script for managing the player character in the game.
/// </summary>
public class Player : MonoBehaviour
{
    public string playerName = ""; //The players name for the purpose of storing the high score
   
    public int playerTotalLives; //Players total possible lives.
    public int playerLivesRemaining; //PLayers actual lives remaining.
   
    public bool playerIsAlive = true; //Is the player currently alive?
    public bool playerCanMove = false; //Can the player currently move?

    public bool isOnPlatform = false;
    public bool isInWater = false;
    public bool hasKey = false;

    public AudioClip jumpSound;
    public AudioClip deathSound;

    public GameObject explosionEffect;

    private GameManager myGameManager; //A reference to the GameManager in the scene.

    private AudioSource gameAudio;
    // Start is called before the first frame update
    void Start()
    {
        myGameManager = GameObject.FindObjectOfType<GameManager>();
        gameAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsAlive)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y < myGameManager.levelConstraintTop)
            {
                transform.Translate(new Vector2(0, 0.92f));
                gameAudio.PlayOneShot(jumpSound);
                myGameManager.UpdateScore(1);
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
        }
    }

    private void LateUpdate()
    {
        if(playerIsAlive)
        {
            if (isInWater && !isOnPlatform)
                KillPlayer();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerIsAlive)
        {
            if (collision.transform.GetComponent<Vehicle>() != null)
            {
                KillPlayer();
            }
            else if (collision.transform.GetComponent<Platform>() != null)
            {
                transform.SetParent(collision.transform);
                isOnPlatform = true;
            }
            else if(collision.transform.tag == "Water")
            {
                //KillPlayer();
                isInWater = true;
            }
            else if(collision.transform.tag == "Key")
            {
                myGameManager.UpdateScore(10);
                hasKey = true;
                Destroy(collision.gameObject);
            }
        } 
    }

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


    void KillPlayer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        gameAudio.PlayOneShot(deathSound);
        playerIsAlive = false;
        playerCanMove = false;
        print("dead mother fucker!");
    }

}
