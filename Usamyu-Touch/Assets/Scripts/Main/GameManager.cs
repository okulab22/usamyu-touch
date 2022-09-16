using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// ゲーム全体制御
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private UsamyuManager usamyuManager;
    [SerializeField] private GameUIManager gameUIManager;
    [SerializeField] private int gameTime;
    [SerializeField] private GameObject backTitleBtForPose;

    // 残り・経過時間
    public static int remainingTime = 0;
    public static int elapsedTime = 0;

    private IEnumerator countGameTime;

    public enum GameState
    {
        Start,
        Prepare,
        Playing,
        End
    }

    private GameState currentState;


    void Awake()
    {
        remainingTime = gameTime;
        elapsedTime = 0;

        backTitleBtForPose.SetActive(false);

        playerManager.OnGameOver.AddListener(GameOver);
    }

    void Start()
    {
        // 各Manager初期化
        ScoreManager.InitializeScore();
        usamyuManager.Init();

        // ゲームの初期状態を設定
        SetCurrentState(GameState.Start);
    }

    void Update()
    {
        // 各ゲーム状態の常時処理
        switch (currentState)
        {
            case GameState.Start:
                break;
            case GameState.Prepare:
                break;
            case GameState.Playing:
                // タッチ入力
                playerManager.TouchedFromPose();
                playerManager.TouchedFromScreen();
                break;
            case GameState.End:
                playerManager.TouchedFromPose();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ゲーム状態の変更
    /// </summary>
    /// <param name="state">遷移先のゲーム状態</param>
    private void SetCurrentState(GameState state)
    {
        currentState = state;
        onStateChanged(currentState);
    }

    /// <summary>
    /// ゲーム状態が遷移した時の処理
    /// </summary>
    /// <param name="state">遷移先のゲーム状態</param>
    private void onStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                // 時間を初期化
                remainingTime = gameTime;
                elapsedTime = 0;

                backTitleBtForPose.SetActive(false);

                SetCurrentState(GameState.Prepare);
                break;
            case GameState.Prepare:
                StartCoroutine(CountDown());
                break;
            case GameState.Playing:
                // カウントダウンテキストを非表示にする
                gameUIManager.HideStateMessageUI();
                // ポーズ時にカウントを停止できるように参照を取っておく
                countGameTime = CountGameTime();
                StartCoroutine(countGameTime);

                usamyuManager.StartSpawn();
                SoundManager.instance.PlayBGM();
                break;
            case GameState.End:
                usamyuManager.StopSpawn();
                usamyuManager.DeleteAllUsamyu();
                SoundManager.instance.StopBGM();
                SoundManager.instance.PlayFinishSE();

                // タイムアップ時のUI表示
                StartCoroutine(OnTimeIsUp());
                break;
            default:
                break;
        }
    }

    private void GameOver()
    {
        StopCoroutine(countGameTime);
        SetCurrentState(GameState.End);
    }

    /// <summary>
    /// カウントダウンを行い状態をPlayingにする
    /// </summary>
    IEnumerator CountDown()
    {
        for (int i = 3; i >= 0; i--)
        {
            // カウントダウンSE
            SoundManager.instance.PlayCountDownSE(i);
            // カウントダウン表示
            gameUIManager.UpdateCountNumText(i);

            yield return new WaitForSeconds(1);
        }

        SetCurrentState(GameState.Playing);
    }

    /// <summary>
    /// ゲーム時間のカウント
    /// </summary>
    IEnumerator CountGameTime()
    {
        while (true)
        {
            elapsedTime++;
            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// タイムアップ時のUI表示切替
    /// </summary>
    IEnumerator OnTimeIsUp()
    {
        // タイムアップ時のメッセージ表示
        gameUIManager.ShowGameOverMessage();
        yield return new WaitForSeconds(2);

        // リザルト表示
        gameUIManager.HideStateMessageUI();
        gameUIManager.ShowResult();
        backTitleBtForPose.SetActive(true);
    }

    /// <summary>
    /// タイトルへ戻る
    /// </summary>
    public static void BackToTitleScene()
    {
        SoundManager.instance.PlayReturnSE();
        SceneManager.LoadScene("Title");
    }
}
