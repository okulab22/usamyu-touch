using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ゲームシーンUI制御
/// </summary>
public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI DisplayTime;

    [SerializeField]
    private TextMeshProUGUI DisplayUsamyu;

    [SerializeField]
    private TextMeshProUGUI DisplayScore;

    // カウントダウンテキスト関係
    [SerializeField]
    private GameObject stateMessageUI;
    [SerializeField]
    private TextMeshProUGUI stateMessageText;

    [SerializeField]
    private GameObject Result;

    [SerializeField]
    private TextMeshProUGUI UsamyuResult;

    [SerializeField]
    private TextMeshProUGUI ScoreResult;


    // Update is called once per frame
    void Start()
    {
        // 常時表示しないUIを無効化
        Result.SetActive(false);
        stateMessageUI.SetActive(false);

        DisplayTime.text = $"Remainig Time :{GameManager.remainingTime}";
        DisplayUsamyu.text = "Usamyu : 0";
        DisplayScore.text = "Score : 0";
    }

    void Update()
    {
        DisplayTime.text = $"Remainig Time :{GameManager.remainingTime}";
        DisplayUsamyu.text = $"Usamyu :  {ScoreManager.sum}";
        DisplayScore.text = $"Score : {ScoreManager.score}";
    }

    /// <summary>
    /// カウントダウンテキスト更新
    /// </summary>
    /// <param name="countNum">カウントダウン値</param>
    public void UpdateCountNumText(int countNum)
    {
        stateMessageUI.SetActive(true);
        if (countNum > 0)
            stateMessageText.text = countNum.ToString();
        else
            stateMessageText.text = "START!";
    }

    /// <summary>
    /// タイムアップ時のテキスト表示
    /// </summary>
    public void ShowFinishMessage()
    {
        stateMessageUI.SetActive(true);
        stateMessageText.text = "FINISH!";
    }

    /// <summary>
    /// カウントダウンUIを非表示にする
    /// </summary>
    public void HideStateMessageUI()
    {
        stateMessageUI.SetActive(false);
    }

    /// <summary>
    /// リザルト画面を隠す
    /// </summary>
    public void HideResult()
    {
        Result.SetActive(false);
    }

    /// <summary>
    /// リザルト画面を表示
    /// </summary>
    public void ShowResult()
    {
        Result.SetActive(true);
        UsamyuResult.text = $"Usamyu : {ScoreManager.sum}";
        ScoreResult.text = $"Score : {ScoreManager.score}";
    }
}
