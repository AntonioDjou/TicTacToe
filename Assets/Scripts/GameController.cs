using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] //Make it visible at the Inspector View.
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable] 
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{

    public Text[] buttonList; // Draw the text reffering to the play (X or O)

    //To include the Game Over situation
    public GameObject gameOverPanel;
    public Text gameOverText;

    private bool GameIsOver = false;


    public GameObject restartButton; //Restart Button
    //Assign the players.
    public Player playerX;
    public Player playerO;
    //Set colors for the (in)active player.
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    //Will tell the player to pick X or O to start playing.
    public GameObject startInfo;

    //Set the players depending on which one the player has picked.
    private string playerSide;
    private string computerSide;

    public bool playerMove; // Checks who's turn is to play.

    public float delay; //Delay before computer make its play.
    private int value; // The text that will be written to mark that play.

    private int moveCount; //To include Draw situation.
    //Amount of wins of each player.
    private int Plwins = 0;
    private int Pcwins = 0;

    private void Awake() //Make the preparations for the game to start.
    {
        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        moveCount = 0;
        restartButton.SetActive(false);
        playerMove = true;
    }

    private void Update() //Make computer's play
    {
        if (playerMove == false)
        {
            delay += delay * Time.deltaTime;
            if (delay >= 100)
            {
                if (moveCount < 9 || GameIsOver == false)
                {
                    ComputerAI();
                }
            }
        }
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++) // Reads all the spaces
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void SetStartingSide(string startingSide) // This function selects X or O for the player and the computer.
    {
        playerSide = startingSide; // In this case, the player always chooses with with one he will play.
        if (playerSide == "X")
        {
            computerSide = "O";
            SetPlayerColors(playerX, playerO); // Chooses corresponding colors to both player and computer.
        }
        else
        {
            computerSide = "X";
            SetPlayerColors(playerO, playerX);
        }
        StartGame();
    }

    void StartGame()
    {
        SetBoardInteractible(true); // Make all buttons active.
        SetPlayerButtons(false); // Unable to choose with which player you will be playing.
        startInfo.SetActive(false); // Makes the starting info disappear.
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public string GetComputerSide()
    {
        return computerSide;
    }

    public void EndTurn() //Check for combos to finish the game. If it's not finished, change the side for the next player.
    {
        moveCount++;
        if (moveCount >= 9) // To know when the number of moves is the max.
        {
            GameOver("draw"); //Write that the game ended in a Draw.
        }
        if ((buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide) ||
            (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide) ||
            (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide) ||
            (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide) ||
            (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide) ||
            (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide) ||
            (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide) ||
            (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide))
        {
            GameOver(playerSide);
        }
        else if ((buttonList[0].text == computerSide && buttonList[1].text == computerSide && buttonList[2].text == computerSide) ||
                 (buttonList[3].text == computerSide && buttonList[4].text == computerSide && buttonList[5].text == computerSide) ||
                 (buttonList[6].text == computerSide && buttonList[7].text == computerSide && buttonList[8].text == computerSide) ||
                 (buttonList[0].text == computerSide && buttonList[3].text == computerSide && buttonList[6].text == computerSide) ||
                 (buttonList[1].text == computerSide && buttonList[4].text == computerSide && buttonList[7].text == computerSide) ||
                 (buttonList[2].text == computerSide && buttonList[5].text == computerSide && buttonList[8].text == computerSide) ||
                 (buttonList[0].text == computerSide && buttonList[4].text == computerSide && buttonList[8].text == computerSide) ||
                 (buttonList[2].text == computerSide && buttonList[4].text == computerSide && buttonList[6].text == computerSide))
        {
            GameOver(computerSide);
        }
        else
        {
            ChangeSides();
            delay = Random.Range(10, 60);
        }
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer) //Changes the colors of (in)active players
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    void GameOver(string winningPlayer) // Check if it's a draw or who's the winner
    {
        SetBoardInteractible(false); // Makes the board uninteractible
        GameIsOver = true;
        if (winningPlayer == "draw")
        {
            SetGameOverText("It's a Draw!");
            SetPlayerColorsInactive();
        }
        else
        {
            if (winningPlayer == playerSide)
            {
                Plwins++;
            }
            else
            {
                Pcwins++;
            }
            SetGameOverText(winningPlayer + " Wins!");
        }
        restartButton.SetActive(true);
    }

    void ChangeSides()
    {
        playerMove = (playerMove == true) ? false : true;
        if (playerMove == true)
        {
            SetPlayerColors(playerX, playerO); //Apply the correct color to current player
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }

    //Game Over Logic
    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    //For you to be able to replay the game without closing it.
    public void RestartGame()
    {
        moveCount = 0; //Restarts the move counter
        gameOverPanel.SetActive(false); // Hides the game over panel.
        restartButton.SetActive(false); //Locks the Restart Button again.
        SetPlayerButtons(true); //Enable to choose with which player you will be playing next round.
        SetPlayerColorsInactive(); //Changes the button colors to inactive.
        startInfo.SetActive(true); //Shows the starting info telling to pick X or O to play.
        playerMove = true; // Enables player to make his move.
        delay = Random.Range(10, 60); // Sets a random value for the computer's play delay.
        GameIsOver = false;
        for (int i = 0; i < buttonList.Length; i++) //Reads through all the buttons.
        {
            buttonList[i].text = ""; //Remove all the X and O from the board.
        }

    }

    void SetBoardInteractible(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++) //Reads through all the buttons
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle; //Switches the board from active to inactive.
        }
    }

    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

    void ComputerAI()
    {
        if ((buttonList[4].text != playerSide) && (buttonList[4].text != computerSide))
        {
            value = 4; //Marks the middle, because it's the best place to play.
            Debug.Log("Center");
        }

        // Sets the deffensive stance for the computer
        else if ((buttonList[1].text == playerSide && buttonList[2].text == playerSide) ||
                 (buttonList[3].text == playerSide && buttonList[6].text == playerSide) ||
                 (buttonList[4].text == playerSide && buttonList[8].text == playerSide))
        {
            value = 0;
            Debug.Log(value);
        }
        else if ((buttonList[0].text == playerSide && buttonList[2].text == playerSide) ||
                 (buttonList[4].text == playerSide && buttonList[7].text == playerSide))
        {
            value = 1;
            Debug.Log(value);
        }

        else if ((buttonList[0].text == playerSide && buttonList[1].text == playerSide) ||
                 (buttonList[5].text == playerSide && buttonList[8].text == playerSide) ||
                 (buttonList[4].text == playerSide && buttonList[6].text == playerSide))
        {
            value = 2;
            Debug.Log(value);
        }

        else if ((buttonList[0].text == playerSide && buttonList[6].text == playerSide) ||
                 (buttonList[4].text == playerSide && buttonList[5].text == playerSide))
        {
            value = 3;
            Debug.Log(value);
        }

        else if ((buttonList[1].text == playerSide && buttonList[7].text == playerSide) ||
                 (buttonList[3].text == playerSide && buttonList[5].text == playerSide) ||
                 (buttonList[0].text == playerSide && buttonList[8].text == playerSide) ||
                 (buttonList[2].text == playerSide && buttonList[6].text == playerSide))
        {
            value = 4;
            Debug.Log(value);

        }

        else if ((buttonList[3].text == playerSide && buttonList[4].text == playerSide) ||
                 (buttonList[2].text == playerSide && buttonList[8].text == playerSide))
        {
            value = 5;
            Debug.Log(value);
        }

        else if ((buttonList[0].text == playerSide && buttonList[3].text == playerSide) ||
                 (buttonList[2].text == playerSide && buttonList[4].text == playerSide) ||
                 (buttonList[7].text == playerSide && buttonList[8].text == playerSide))
        {
            value = 6;
            Debug.Log(value);
        }

        else if ((buttonList[1].text == playerSide && buttonList[4].text == playerSide) ||
                 (buttonList[6].text == playerSide && buttonList[8].text == playerSide))
        {
            value = 7;
            Debug.Log(value);
        }

        else if ((buttonList[6].text == playerSide && buttonList[7].text == playerSide) ||
                 (buttonList[2].text == playerSide && buttonList[5].text == playerSide) ||
                 (buttonList[0].text == playerSide && buttonList[4].text == playerSide))
        {
            value = 8;
            Debug.Log(value);
        }
        else
        {
            Debug.Log("Else");
            value = Random.Range(0, buttonList.Length); // Pickup a place to spawn the marker at random
            Debug.Log(value);
        }
        if (buttonList[value].GetComponentInParent<Button>().interactable == true) // Checks if it's available to pick it now
        {
            buttonList[value].text = GetComputerSide(); // Selects the marker relative of the computer turn
            buttonList[value].GetComponentInParent<Button>().interactable = false; // Makes the computer unable to play again until the player's next move
            EndTurn();
        }
    }
}