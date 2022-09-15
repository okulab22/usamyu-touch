using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// うさみゅ～を管理するクラス
/// </summary>
public class UsamyuManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
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

    private enum SpawnMode
    {
        Normal,
        Fever
    }

    private SpawnMode spawnMode = SpawnMode.Normal;

    void Awake()
    {
        spawnMode = SpawnMode.Normal;

        playerManager.OnFeverStart.AddListener(StartFever);
        playerManager.OnFeverEnd.AddListener(EndFever);
    }

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
    /// 出現させるうさみゅ～を選択する
    /// </summary>
    /// <returns>うさみゅ～のindex</returns>
    private int SelectUsamyu()
    {
        int kindofUsamyu = 0;

        if (spawnMode == SpawnMode.Fever)
        {
            int randNum = Random.Range(0, 13); // 0～12の乱数を生成
            if (randNum <= 6)
                kindofUsamyu = 0; // 赤うさみゅ～
            else if (randNum <= 9) 
                kindofUsamyu = 1; // 水色うさみゅ～
            else if (randNum <= 12)
                kindofUsamyu = 2; // 緑うさみゅ～
        }
        else
        {
            int randNum = Random.Range(0, 20); // 0～19の乱数を生成
            if (randNum <= 6)
                kindofUsamyu = 0; // 赤うさみゅ～
            else if (randNum <= 10)
                kindofUsamyu = 1; // 水色うさみゅ～
            else if (randNum <= 14)
                kindofUsamyu = 2; // 緑うさみゅ～
            else if (randNum <= 18)
                kindofUsamyu = 3; // ヤンキーうさみゅ～
            else if (randNum == 19)
                kindofUsamyu = 4;  // 虹色うさみゅ～
        }

        return kindofUsamyu;
    }

    /// <summary>
    /// 指定したうさみゅ～をスポーンさせる
    /// </summary>
    /// <param name="kindofUsamyu">うさみゅ～のindex</param>
    private void Spawn(int kindofUsamyu)
    {
        Vector2 spawnPosition = Vector2.zero;

        // スポーン座標を決定
        if (kindofUsamyu != 3)
        { // ヤンキーうさみゅ～以外の場合
            spawnPosition.x = Random.Range(15, 85) * 0.01f;
            spawnPosition.y = Random.Range(15, 85) * 0.01f;
        }
        else
        { // うさみゅ～軍団の場合
            // プレイヤーの中心位置を取得
            Vector2 playerPos = ScreenManager.GamePositionToViewportPosition(PlayerManager.nosePos);

            spawnPosition.x = playerPos.x;
            spawnPosition.y = 1.5f;
        }

        // うさみゅ～をPrefabから読み込み
        GameObject usamyuObj =
            Instantiate(usamyuPrefabs[kindofUsamyu],
                ScreenManager.ViewportToGamePosition(spawnPosition, true),
                Quaternion.identity);

        // 管理Dictに登録
        appearedUsamyuDict.Add(usamyuId, usamyuObj);

        // フィールドに表示
        Usamyu usamyu = usamyuObj.GetComponent<Usamyu>();
        usamyu.Create(usamyuId, spawnPosition);

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
        GameObject targetUsamyu;
        try
        {
            targetUsamyu = appearedUsamyuDict[id];
        }
        catch (KeyNotFoundException)
        {
            Debug.Log($"Usamyu dict key:{id} is not found.");
            return;
        }


        if (isTouched)
        {
            if (GameManager.elapsedTime > 20) // 経過時間が20秒以降であれば、経過時間に応じてスコアを増加させる
                score = (int)(score * (1 + GameManager.elapsedTime * 0.01f));
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

    /// <summary>
    /// フィーバー開始
    /// </summary>
    private void StartFever()
    {
        spawnMode = SpawnMode.Fever;
        Debug.Log("Catch Start Fever.");
    }

    /// <summary>
    /// フィーバー終了
    /// </summary>
    private void EndFever()
    {
        spawnMode = SpawnMode.Normal;
        Debug.Log("Catch End Fever.");
    }

    void Update()
    {
        // モードによりスポーン上限と間隔を設定
        if (spawnMode == SpawnMode.Fever)
        {
            maxusamyu = 12;
            timeInterval = Random.Range(0, 1);
        }
        else
        {
            maxusamyu = 6;
            if (GameManager.elapsedTime < 20) // 経過時間が20秒以内であれば2秒～4秒の間隔で次のうさみゅ～を出現させる
            {
                timeInterval = Random.Range(2, 4);
            }
            else if (GameManager.elapsedTime <= 80) // 残り時間が10秒～70秒以内であれば2秒～3秒の間隔で次のうさみゅ～を出現させる
            {
                timeInterval = Random.Range(1, 2);
            }
        }

        //スポーンが許可されているか (true だったらスポーンさせる)
        if (!allowSpawn)
            return;

        // 出現数が上限の場合は新たにスポーンしない
        if (appearedUsamyuDict.Count >= maxusamyu)
            return;

        // 出現間隔以上の時間が経っているか
        if (elapsedTime < timeInterval) // 経過時間 < 出現間隔
            elapsedTime += Time.deltaTime;
        else
        {
            // うさみゅ～のスポーン
            int kindofUsamyu = SelectUsamyu();
            Spawn(kindofUsamyu);

            elapsedTime = 0;
        }
    }
}
