using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControlUI : MonoBehaviour
{
    [SerializeField]
    Transform UIPannel;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text levelText;
    ScoreCounter scoreCounter;
    GameController gameController;
    bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        UIPannel.gameObject.SetActive(false);
        isPaused = false;
        scoreCounter = GameObject.Find("GameController").GetComponent<ScoreCounter>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "分数:" + scoreCounter.score;
        levelText.text = "等级:" + gameController.difficulty;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            setPauseAction();

        }
            
    }
    public void setPauseAction()
    {
        if (!isPaused)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
    }
    public void Pause()
    {
        isPaused = true;
        UIPannel.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void UnPause()
    {
        isPaused = false;
        UIPannel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ReStart()
    {
        UnPause();
        SceneManager.LoadScene(0);
    }
}
