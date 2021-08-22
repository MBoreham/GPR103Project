using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


/// <summary>
/// This script is to be attached to a GameObject called GameManager in the scene. It is to be used to manager the settings and overarching gameplay loop.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Scoring")]
    public int currentScore = 0; //The current score in this round.
    public int highScore = 0; //The highest score achieved either in this session or over the lifetime of the game.
    public TMP_Text currentScoreUI;

    [Header("Playable Area")]
    public float levelConstraintTop; //The maximum positive Y value of the playable space.
    public float levelConstraintBottom; //The maximum negative Y value of the playable space.
    public float levelConstraintLeft; //The maximum negative X value of the playable space.
    public float levelConstraintRight; //The maximum positive X value of the playablle space.

    [Header("Gameplay Loop")]
    public bool isGameRunning; //Is the gameplay part of the game current active?
    public float totalLives = 5; //The maximum amount of time or the total time avilable to the player.
    public float gameTimeRemaining; //The current elapsed time

    [Header("UI")]
    // returns either the win or lose message
    public TMP_Text uiGameOverMessage;
    public TMP_Text uiScore; // returns the current score

    public MainMenu myUI; // object of Main Menu class

    public void Awake()
    {
        myUI = FindObjectOfType<MainMenu>();
    }

    
    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(-currentScore); // change score
        currentScoreUI.text = "0";
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    // updates and outputs the score
    public void UpdateScore(int scoreAount)
    {
        currentScore += scoreAount;
        currentScoreUI.text = currentScore.ToString();
    }


    // displays a message based upon if the player has won or lost
    public void GameOver(bool isWin)
    {
        if(isWin == true)
        {
            uiGameOverMessage.text = "You Won!";
        }
        else
        {
            uiGameOverMessage.text = "You Lost!";
        }
        myUI.uiGameOverWindow.SetActive(true);
    }
}
