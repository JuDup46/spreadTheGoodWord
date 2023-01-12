using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class scoreManager : MonoBehaviour
{
    public static scoreManager instance;

    public Text scoreText;
    public Text highScoreText;

    public Ennemy ennemy;

    int score = 0;
    int highScore = 0;

    public void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString() + " points";
        highScoreText.text = "highscore : " + highScore.ToString();
    }

    // Update is called once per frame
    public void AddPoint(float size)
    {

        if (size < 0.75f)
        {
            score += 100;
            scoreText.text = score.ToString() + " points";
        }
        else if (size < 1.25f)
        {
            score += 50;
            scoreText.text = score.ToString() + " points";
        }
        else
        {
            score += 25;
            scoreText.text = score.ToString() + " points";
        }


        if (highScore < score)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
        
    }

    public void zeroPoint()
    {
        score = 0;
        scoreText.text = score.ToString() + " points";
    }
}
