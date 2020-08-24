using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumUI : MonoBehaviour
{
    [SerializeField]
    private GameObject floatingNumV;
    [SerializeField]
    private GameObject floatingNumH;
    private bool readyToShow = false;
    private int num = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(readyToShow)
        {
            if(Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                floatingNumH.GetComponent<ScoreAnimation>().SetNumber(num);
                floatingNumH.SetActive(true);
            }
            else
            {
                floatingNumV.GetComponent<ScoreAnimation>().SetNumber(num);
                floatingNumV.SetActive(true);
            }
            
            readyToShow = false;
        }
    }
    public void ShowNum(int num)
    {
        this.num = num;
        readyToShow = true;
    }
}
