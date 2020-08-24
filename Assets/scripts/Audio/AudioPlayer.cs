using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject ball;
    private AudioSource ballBounce;
    private AudioSource scoreAward;
    private AudioSource badBounce;
    // Start is called before the first frame update
    void Start()
    {
        scoreAward = ball.GetComponents<AudioSource>()[0];
        ballBounce = ball.GetComponents<AudioSource>()[1];
        badBounce = ball.GetComponents<AudioSource>()[2];

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayBounceAudio()
    {
        if (ballBounce.isPlaying)
            ballBounce.Stop();
        ballBounce.Play();
    }
    public void PlayScoreAwardAudio()
    {
        if (scoreAward.isPlaying)
            scoreAward.Stop();
        scoreAward.Play();
    }
    public void PlayBadBounceAudio()
    {
        if (badBounce.isPlaying)
            badBounce.Stop();
        badBounce.Play();
    }
}

