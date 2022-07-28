using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スコア管理クラス
/// </summary>
public static class ScoreManager
{
    public static int score; //得点
    public static int sum; //うさみゅ～をタッチした数

    /// <summary>
    /// 初期化処理
    /// </summary>
    public static void InitializeScore()
    {
        sum = 0; score = 0;
    }

    /// <summary>
    /// スコア加算
    /// </summary>
    /// <param name="receivedScore"></param>
    public static void AddScore(int receivedScore)
    {
        sum++;
        score += receivedScore;
    }
}
