using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// タイトル管理クラス
/// </summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;

    void Start()
    {
        TitleSoundManager.instance.PlayBGM();
    }

    void Update()
    {
        playerManager.TouchedFromPose();
    }

    /// <summary>
    /// ゲームシーンへ遷移
    /// </summary>
    public static void StartGame()
    {
        TitleSoundManager.instance.StopBGM();
        TitleSoundManager.instance.PlayStartSE();
        SceneManager.LoadScene("Main");
    }

    public void EndGame()
    {
    }
}
