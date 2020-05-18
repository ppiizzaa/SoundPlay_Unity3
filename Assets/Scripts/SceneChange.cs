using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    public Animator buttonAnimator;
    public Text text;

    public void MainSceneChange()
    {
        ShowRewardedAd();
    }

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1920, 1080, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize("", false);
        Debug.Log("Init Ads");
    }

    private void ShowRewardedAd()
    {

        /*
        if (Advertisement.IsReady("rewardedVideo"))
        {
            text.text = "START\nMAKE SOUND";
            Debug.Log("Read Ads");
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            text.text = "Loading...";
        }
        */
        text.text = "Loading...";

        while (!Advertisement.IsReady("rewardedVideo"))
        {

        }

        text.text = "START\nMAKE SOUND";
        var options = new ShowOptions { resultCallback = HandleShowResult };
        Advertisement.Show("rewardedVideo", options);
        SceneManager.LoadScene("MainScene");
    }

    private void HandleShowResult(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Finished:
                Debug.Log("사용자가 광고를 성공적으로 보았습니다.");
                break;

            case ShowResult.Skipped:
                Debug.Log("사용자가 광고가 끝나기 전에 스킵했습니다.");
                break;

            case ShowResult.Failed:
                Debug.Log("광고가 보여지는 과정에서 오류가 발생했습니다.");
                break;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
