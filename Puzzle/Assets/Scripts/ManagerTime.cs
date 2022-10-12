using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerTime : MonoBehaviour
{   
    public float time = 10f;
    private float _time;
    public Slider slider;
    public GameObject MenuOver;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreHightText;
    public GameObject gameObjects1;
    public GameObject gameObjects2;
    private int Score = 0;
    private int ScoreHight = 0;


    public static ManagerTime Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        _time = time;

        ScoreHight = PlayerPrefs.GetInt("ScoreHight");
        scoreHightText.text = ScoreHight + "";
    }

    void Start()
    {
        
    }

    void Update()
    {
        time -= Time.deltaTime;
        slider.value = time / _time;

        if(time >= _time)
        {
            time = _time;
        }


        if(time < 0)
        {
            time = 0;
            Time.timeScale = 0f;
            MenuOver.SetActive(true);
            Manager.Instance.IsShifting = true;
            scoreText.text = Score + "";
            if (checkHight(Score, ScoreHight))
            {
                scoreText.color = Color.red;
                scoreText.text = Score + " !";
            }
            gameObjects1.SetActive(false);
            gameObjects2.SetActive(false);
        }

    }
    private bool checkHight(int _score, int _hight)
    {
        if(_score > _hight)
        {
            PlayerPrefs.SetInt("ScoreHight", _score);
            return true;
        }
        return false;
    }
    public void reducedTime()
    {
        time -= 0.2f;
    }
    public void addScore(int _score)
    {
        Score += _score;
    }
    public void resetGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

}
