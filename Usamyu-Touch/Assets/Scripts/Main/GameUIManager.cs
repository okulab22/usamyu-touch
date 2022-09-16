using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// ゲームシーンUI制御
/// </summary>
public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI DisplayTime;

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
    private TextMeshProUGUI playTime;

    [SerializeField]
    private TextMeshProUGUI UsamyuResult;

    [SerializeField]
    private TextMeshProUGUI ScoreResult;

    // ライフゲージ
    [SerializeField]
    private CarrotGaugeController carrotGaugeController;

    [SerializeField]
    private PlayerManager playerManager;

    void Awake()
    {
        playerManager.OnUpdateLife.AddListener(UpdateLife);
    }

    // Update is called once per frame
    void Start()
    {
        // 常時表示しないUIを無効化
        Result.SetActive(false);
        stateMessageUI.SetActive(false);

        DisplayTime.text = $"Playing Time : {GameManager.elapsedTime}";
        DisplayScore.text = "0";
    }

    void Update()
    {
        DisplayTime.text = $"Playing Time : {GameManager.elapsedTime}";
        DisplayScore.text = $"{ScoreManager.score}";
    }

    public void UpdateLife()
    {
        carrotGaugeController.setCarrotGauge(PlayerManager.playerLife);
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
            stateMessageText.text = "スタート!";
    }

    /// <summary>
    /// ゲームオーバー時のテキスト表示
    /// </summary>
    public void ShowGameOverMessage()
    {
        stateMessageUI.SetActive(true);
        stateMessageText.text = "ゲームオーバー!";
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
        // プレイ時間[s]をmm:ssにする
        TimeSpan span = new TimeSpan(0, 0, GameManager.elapsedTime);

        Result.SetActive(true);
        UsamyuResult.text = $"{ScoreManager.sum}";
        ScoreResult.text = $"{ScoreManager.score}";
        playTime.text = span.ToString(@"mm\:ss");
    }
}
