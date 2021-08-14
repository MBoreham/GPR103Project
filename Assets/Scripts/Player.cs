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
                transform.Translate(new Vector2(0, 1));
                gameAudio.PlayOneShot(jumpSound);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y > myGameManager.levelConstraintBottom)
            {
                transform.Translate(new Vector2(0, -1));
                gameAudio.PlayOneShot(jumpSound);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > myGameManager.levelConstraintLeft)
            {
                transform.Translate(new Vector2(-1, 0));
                gameAudio.PlayOneShot(jumpSound);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < myGameManager.levelConstraintRight)
            {
                transform.Translate(new Vector2(1, 0));
                gameAudio.PlayOneShot(jumpSound);
            }
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
