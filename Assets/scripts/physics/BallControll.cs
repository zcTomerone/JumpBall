using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControll : MonoBehaviour
{
    [SerializeField]
    private GameObject audioPlayer;
    [SerializeField]
    private Camera camera;
    SphereCollider sphereCollider;
    public Color materialColor;
    public float bounceSpeed;
    public float gravity;
    public float maxSpeed = 1;
    public Color hitColor;
    private ScoreCounter scoreCounter;
    float radius = 0;
    float speed;

    void Start()
    {
        scoreCounter = GameObject.Find("GameController").GetComponent<ScoreCounter>();
        materialColor = this.GetComponent<Renderer>().material.color;
        hitColor = this.GetComponent<Renderer>().material.color;
        sphereCollider = GetComponent<SphereCollider>();
        radius = sphereCollider.radius;

    }
    //球掉落移动计算
    void Drop()
    {
        float oldSpeed = speed;
        //每帧减去模拟重力带来的速度影响
        speed -= gravity * Time.deltaTime;
        //限制球能达到的最大速度
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

        transform.position += new Vector3(0, (speed + oldSpeed) / 2 * Time.deltaTime, 0);
    }
    void Bounce()
    {
        //当球反弹回升的时候跳过检测
        if (speed >= 0)
        {
            return;
        }
        ////射线检测，小球穿过后可能会被弹回，不好
        //Vector3 bottomPos = transform.position + new Vector3(0, -radius, 0);
        //Vector3 direction = transform.up;
        //if (Physics.Raycast(bottomPos, direction,20, LayerMask.GetMask("Ground")))
        //{
        //    speed = bounceSpeed;
        //}

        Vector3 p = transform.position + new Vector3(0, -radius/2, 0);
        Vector3 size = new Vector3(radius * 0.5f, radius , radius * 0.5f);
        Collider[] colliders = Physics.OverlapBox(p, size, Quaternion.identity, LayerMask.GetMask("Ground"));
        if (colliders.Length > 0)
        {
            // start with 1 scores per drop
            scoreCounter.ResetScoreStep();
            // change the color
            this.GetComponent<Renderer>().material.color = materialColor;
            bool deduce = false;
            foreach (var col in colliders)
            {
                if (col.gameObject.CompareTag("DeathLoop"))
                {
                    deduce = true;
                    scoreCounter.DeduceScore();
                    this.GetComponent<Renderer>().material.color = hitColor;
                    break;
                }
            }
            if(deduce)
            {
                camera.GetComponent<CameraController>().readyToShake = true;
                audioPlayer.GetComponent<AudioPlayer>().PlayBadBounceAudio();
            }
            else
            {
                audioPlayer.GetComponent<AudioPlayer>().PlayBounceAudio();
            }
            // adjust the ball's position every time
            GameObject obj = colliders[0].gameObject;
            LoopMesh loopMesh = obj.GetComponent<LoopMesh>();
            Vector3 pos = transform.position;
            pos.y = obj.transform.position.y + loopMesh.height / 2 + radius;
            transform.position = pos;
            speed = bounceSpeed;
        }


    }
    private void Update()
    {
        //Drop();
    }
    void LateUpdate()
    {
        
        //Bounce();
        
    }
    private void FixedUpdate()
    {
        Bounce();
        Drop();
    }


}