using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreBoard : MonoBehaviour {

    private int score1, score2, screenNum;
    public Text text;

    public float timeLimit;
    public int scoreLimit;

    public GameObject player1;
    public GameObject player2; 

    // Use this for initialization
    void Start ()
    {
        screenNum = 0;
        text.text = "";
        score1 = 0;
        score2 = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
		switch(screenNum)
        {
            case 0: TitleScreen();
                break;
            case 1: Scoreboard();
                break;
            case 2: FinalScreen(getWinner());
                break;
        }
	}

    private void TitleScreen()
    {
        Time.timeScale = 0;

        text.text = "Press any key to begin";

        if (Input.anyKey)
        {
            screenNum++;
            Time.timeScale = 1;
            text.text = screenNum.ToString();
        }
        
    }

    private void Scoreboard()
    {
            getScores();
            timeLimit -= Time.deltaTime;

            int time = ((int)timeLimit) + 1;

            text.text = "Team 1: " + score1.ToString() + "\t\tTime: " + time.ToString() + "\t\tTeam 2: " + score2.ToString();

            if (time == 0)
            {
                screenNum++;
            }

            if (score1 == scoreLimit || score2 == scoreLimit)
            {
                screenNum++;
            }
    }

    public void FinalScreen(int winner)
    {
        Time.timeScale = 0;

        if(winner == 0)
        {
            text.text = "Tied Game";
        }
        else
        {
            text.text = "The winner is Team " + winner + "!!!";
        }
    }

    public void FinalScreen(int winner, float d)
    {
        StartCoroutine(delay(winner, d));
    }

    public IEnumerator delay(int winner, float d)
    {
        yield return new WaitForSeconds(d);

        screenNum = 2;

        Time.timeScale = 0;

        if (winner == 0)
        {
            text.text = "Tied Game";
        }
        else
        {
            text.text = "The winner is Team " + winner + "!!!";
        }
    }

    public void setPlayers(GameObject p1, GameObject p2)
    {
        player1 = p1;
        player2 = p2;
    }

    private void getScores()
    {
        score1 = player1.GetComponent<PointControl>().getScore();
        score2 = player2.GetComponent<PointControl>().getScore();
    }

    private int getWinner()
    {
        getScores();
        int winner;

        if (score1 > score2)
        {
            winner = 1;
        }
        else if (score2 > score1)
        {
            winner = 2;
        }
        else
        {
            winner = 0;
        }

        return winner;
    }

}
