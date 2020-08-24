using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    private GameObject numUI;
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject audioPlayer;
    public int score = 0;
    public int scoreStep = 1;
    private float gap;
    public int hitDiductScore = -10;
    private float lastScoreTriggerHeight;
    // Start is called before the first frame update
    void Start()
    {
        gap = GameObject.Find("GameController").GetComponent<GameController>().loopGap;
        lastScoreTriggerHeight = ball.transform.position.y - gap / 2;
    }

    // Update is called once per frame
    void Update()
    {
        float newHeight = ball.transform.position.y;
        if ((lastScoreTriggerHeight - newHeight) > gap)
        {
            audioPlayer.GetComponent<AudioPlayer>().PlayScoreAwardAudio();
            numUI.GetComponent<NumUI>().ShowNum(scoreStep);
            score += scoreStep;
            scoreStep++;
            scoreStep = Mathf.Clamp(scoreStep, 1, 9);
            lastScoreTriggerHeight -= gap;
        }
    }
    public void DeduceScore()
    {
        // numUI.GetComponent<NumUI>().ShowNum(hitDiductScore);
        score += hitDiductScore;
    }
    public void ResetScoreStep()
    {
        scoreStep = 1;
    }
}
