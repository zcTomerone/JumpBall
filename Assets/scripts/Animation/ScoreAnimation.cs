using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAnimation : MonoBehaviour
{
    public Image numImage;
    public List<Sprite> nums = new List<Sprite>();
    int num;

    void Start()
    {
        numImage = transform.Find("NumImage").GetComponent<Image>();
        this.gameObject.SetActive(false);
    }

    public void SetNumber(int n)
    {
        this.num = n;
        numImage.sprite = nums[num % 10];
        Color c = numImage.color;
        c.a = 1.0f;
        numImage.color = c;
        numImage.CrossFadeAlpha(1, 0.005f, false);

    }

    void Update()
    {

        //transform.Translate(new Vector3(0, 70 * Time.deltaTime, 0));
    }
    private void LateUpdate()
    {
        numImage.CrossFadeAlpha(0, 0.3f, false);
    }
}
