using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// うさみゅ～群
/// </summary>
public class YankeeUsamyu : Usamyu
{
    // 生存時間 [s]
    // PrefabのInspectorで設定する
    [SerializeField] private int survivalTime;

    // 移動演算に必要な変数
    private float x, y;
    private float speed = 0.5f; // 落下スピード
    private float stopTime; // 落下の一時停止を開始する時間

    public override void Create(int id, Vector2 viewportPos)
    {
        base.Create(id, viewportPos);

        /*落下スピード*/
        if (GameManager.elapsedTime > 80) // 80秒経過後にスピードを0.005増やす
            speed += 0.25f;
    }

    /// <summary>
    /// うさみゅ～の移動処理
    /// フレーム毎に呼び出される
    /// </summary>
    /// <returns>Vector2(x, y) 移動先のViewport座標</returns>
    protected override Vector2 Move()
    {
        /*停止開始時間*/
        if (GameManager.elapsedTime <= 80) // ゲーム経過時間が80秒以下の場合
            stopTime = 1.0f; // 落下開始から1秒後に落下を一時停止
        else                             // ゲーム開始から80秒経過した場合
            stopTime = 0.8f; // 落下開始から0.5秒後に落下を一時停止

        /*落下*/
        if (elapsedTime <= stopTime) // ヤンキーうさみゅ～発生からの経過時間がstopTime秒以下の場合
            y = basePosition.y - speed * elapsedTime; // 初期位置から落下
        else if (elapsedTime > stopTime && elapsedTime < 2) // ヤンキーうさみゅ～発生からの経過時間がstopTime秒～2秒の間の場合
            y = basePosition.y - speed * stopTime; // 落下せず、その場で停止
        else                            // ヤンキーうさみゅ～発生から2秒経過した場合
            y = basePosition.y - speed * (elapsedTime - (2 - stopTime)); //停止位置から落下を再開


        return new Vector2(basePosition.x, y);
    }


    /// <summary>
    /// 自然消滅までに行う処理
    /// </summary>
    protected override IEnumerator UntilDespawn()
    {

        yield return new WaitForSeconds(survivalTime); // 生存時間経過

        // この関数を呼び出すと自然消滅する
        DeleteNaturally();
    }
}

