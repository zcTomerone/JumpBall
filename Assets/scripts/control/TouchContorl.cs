using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchContorl : MonoBehaviour
{
    public float maxRotateSpeed;
    private Vector3 lastMousePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool IsPaused()
    {
        if (Time.timeScale == 0)
            return true;
        return false;
    }
    void TouchRotate()
    {
        //android adjust-------------------------------------------------------------------------------------
        if (Input.touchCount>0)
        {
            Touch myTouch = Input.GetTouch(0);
            if (myTouch.phase == TouchPhase.Began)
                return;
            else
            {
                float rotateArc = myTouch.position.x - lastMousePos.x;
                Mathf.Clamp(rotateArc, 0.0f, maxRotateSpeed);
                transform.Rotate(0, -rotateArc, 0);
            }
        }
        //------------------------------------------------------------------------------------------------------
        else if(Input.GetMouseButton(0))
        {
            float rotateArc = (Input.mousePosition - lastMousePos).x;
            Mathf.Clamp(rotateArc, 0.0f, maxRotateSpeed);
            transform.Rotate(0, -rotateArc, 0);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (IsPaused())
            return;
        TouchRotate();
        lastMousePos = Input.mousePosition;
    }
}
