using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// うさみゅ～を管理するクラス
/// </summary>
public class UsamyuManager : MonoBehaviour
{
    // うさみゅ～出現位置調整用
    [SerializeField] private ScreenManager screenManager;
    // 各うさみゅ～のPrefabを登録
    [SerializeField] private GameObject[] usamyuPrefabs;

    // 出現済みのうさみゅ～を管理するDict
    private static Dictionary<int, GameObject> appearedUsamyuDict = new Dictionary<int, GameObject>();
    // うさみゅ～のid（連番)
    private int usamyuId = 0;
    //スポーンを許可するかどうかを表す変数
    private bool allowSpawn = false;
    //うさみゅ～の最大出現数
    private int maxusamyu = 6;
    //うさみゅ～が出現してからの経過時間
    private float elapsedTime = 0;
    //次のうさみゅ～が発生するまでの時間間隔
    private int timeInterval;
    //うさみゅ～の種類を決定する乱数
    private int randNum;
    //うさみゅ～の種類
    private int kindofUsamyu;

    /// <summary>
    /// ゲーム開始前の初期化処理
    /// </summary>
    public void Init()
    {
        appearedUsamyuDict = new Dictionary<int, GameObject>();
        usamyuId = 0;
        timeInterval = Random.Range(2, 4);
    }

    /// <summary>
    /// 出現している全てのうさみゅ～を削除
    /// </summary>
    public void DeleteAllUsamyu()
    {
        foreach (GameObject usamyuObj in appearedUsamyuDict.Values)
        {
            Destroy(usamyuObj);
        }
        appearedUsamyuDict = new Dictionary<int, GameObject>();
    }

    /// <summary>
    /// うさみゅ～をスポーンさせる
    /// </summary>
    public void SpawnUsamyu()
    {
        // ランダム出現位置
        Vector2 randomPosition = new Vector2(Random.Range(15, 85) * 0.01f, Random.Range(15, 85) * 0.01f);

        randNum = Random.Range(0, 13); // 0～12の乱数を生成

        if(randNum >= 0 && randNum <= 4) // 生成された乱数が0～4の場合
            kindofUsamyu = 0; // 赤うさみゅ～
        else if(randNum >= 5 && randNum <= 7) // 生成された乱数が5～7の場合
            kindofUsamyu = 1; // 水色うさみゅ～
        else if (randNum >= 8 && randNum <= 9) // 生成された乱数が8～9の場合
            kindofUsamyu = 2; // 緑うさみゅ～
        else if(randNum >= 10 && randNum <= 11) // 生成された乱数が10～11の場合
            kindofUsamyu = 3; // 紫うさみゅ～
        else                                    // 生成された乱数が12の場合
            kindofUsamyu = 4; // 黄色うさみゅ～

        // うさみゅ～をPrefabから読み込み
        GameObject usamyuObj =
            Instantiate(usamyuPrefabs[kindofUsamyu],
                ScreenManager.ViewportToGamePosition(randomPosition, true),
                Quaternion.identity);

        // 管理Dictに登録
        appearedUsamyuDict.Add(usamyuId, usamyuObj);

        // フィールドに表示
        Usamyu usamyu = usamyuObj.GetComponent<Usamyu>();
        usamyu.Create(usamyuId, randomPosition);

        // idを更新
        usamyuId++;
    }

    /// <summary>
    /// うさみゅ～消滅時に呼び出す関数
    /// </summary>
    /// <param name="id">うさみゅ～が持つ固有id</param>
    /// <param name="isTouched">タッチによって消滅した場合はtrue</param>
    public static void NotifyDeleteUsamyu(int id, int score, bool isTouched)
    {
        GameObject targetUsamyu = appearedUsamyuDict[id];

        if (isTouched)
        {
            ScoreManager.AddScore(score);
        }

        // 管理Dictから削除
        appearedUsamyuDict.Remove(id);

        // フィールドから削除
        Destroy(targetUsamyu);
    }

    /// <summary>
    /// スポーンを開始
    /// </summary>
    public void StartSpawn()
    {
        allowSpawn = true;
    }

    /// <summary>
    /// スポーンを停止
    /// </summary>
    public void StopSpawn()
    {
        allowSpawn = false;
    }

    void Update()
    {
        //スポーンが許可されているか (true だったらスポーンさせる)
        if (allowSpawn == true)
        {
            // 残り時間が10秒以内であれば出現数の最大値を10にする
            if (GameManager.remainingTime <= 10)
                maxusamyu = 12;

            //出現数が上限以内か
            if (appearedUsamyuDict.Count < maxusamyu)
            {
                //出現間隔以上の時間が経っているか
                if (elapsedTime < timeInterval) //経過時間 < 出現間隔
                    elapsedTime += Time.deltaTime;
                else //経過時間 >= 出現間隔
                {
                    SpawnUsamyu(); // うさみゅ～のスポーン
                    elapsedTime = 0;
                    if (GameManager.remainingTime > 70) // 残り時間が70秒～90秒以内であれば2秒～4秒の間隔で次のうさみゅ～を出現させる
                    {
                        timeInterval = Random.Range(2, 4);
                    }
                    else if (GameManager.remainingTime <= 70 && GameManager.remainingTime > 10) // 残り時間が10秒～70秒以内であれば2秒～3秒の間隔で次のうさみゅ～を出現させる
                    {
                        timeInterval = Random.Range(1, 2);
                    }
                    else // 残り時間が10秒を切ったら1秒～2秒の間隔でうさみゅ～を出現させる
                    {
                        timeInterval = Random.Range(0, 1);
                    }
                }
            }
        }
    }
}
