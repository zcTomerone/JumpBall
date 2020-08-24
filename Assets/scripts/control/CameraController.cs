using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool readyToShake { get; set; }
    Transform ball;
    float camOffsetY;
    public Transform camTransform;

    //持续抖动的时长
    public float shakeTime;
    private float shake = 0f;

    // 抖动幅度（振幅）
    //振幅越大抖动越厉害
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    Vector3 normalPos;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Player").transform;
        camTransform = GetComponent<Transform>();
        camOffsetY = ball.transform.position.y - transform.position.y;
        normalPos = camTransform.localPosition;
    }
    private void Update()
    {
        // 单向移动
        //当球的位置低于偏移值时让摄像机和球一起动
        float diffY = ball.transform.position.y - transform.position.y - camOffsetY;
        if (diffY < 0)
        {
            // 比必要的位置再低一些，可以防止抖动
            normalPos += new Vector3(0, diffY - 0.10f, 0);
        }
        if (readyToShake)
        {
            readyToShake = false;
            shake = shakeTime;
        }
        if (shake > 0)
        {
            camTransform.localPosition = camTransform.localPosition = normalPos + Random.insideUnitSphere * shakeAmount;

            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0f;
            camTransform.localPosition = normalPos;
        }
     
    }
    private void LateUpdate()
    {
        
    }
}