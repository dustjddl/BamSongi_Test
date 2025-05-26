using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class EndUiManager : MonoBehaviour
{
    public TMP_Text TextTotalScore = null;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextTotalScore.text = "Total Score : " + UiManager.Instance.Score.ToString();
    }

    public void ResetGame()
    {
        
    }
}
