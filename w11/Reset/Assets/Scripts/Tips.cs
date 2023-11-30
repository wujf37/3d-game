using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{
    public static Tips Instance;
    public GameObject tipsObj;
    public Text tipsText;
    public Text tipsScore;

    private int score;

    private void Start()
    {
        Instance = this;
    }

    public void SetText(string str, int score)
    {
        tipsObj.SetActive(true);
        tipsText.text = "射中" + str + "本次得分为:" + score;
        this.score += score;
        tipsScore.text = "总得分:" + this.score.ToString();
        Invoke("Close", 1.5f);
    }

    public void Close()
    {
        tipsText.text = string.Empty;
        tipsObj.SetActive(false);
    }
}
