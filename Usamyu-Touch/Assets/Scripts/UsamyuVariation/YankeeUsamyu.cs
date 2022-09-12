using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// むらさきのうさみゅ～
/// </summary>
public class YankeeUsamyu : Usamyu
{
    // 生存時間 [s]
    // PrefabのInspectorで設定する
    [SerializeField] private int survivalTime;

    // 移動演算に必要な変数
    private float x, y;
    private float radius = 0.15f;
    private float speed = 3f;

    /// <summary>
    /// うさみゅ～の移動処理
    /// フレーム毎に呼び出される
    /// </summary>
    /// <returns>Vector2(x, y) 移動先のViewport座標</returns>
    protected override Vector2 Move()
    {
        x = radius * Mathf.Sin(Time.time * speed);
        y = radius * Mathf.Cos(Time.time * speed);

        // 片方を縦横比で割る
        // こうしないと楕円になる
        // y基準にすると 16:9 = x:1, x ~= 1.78
        x /= 1.78f;

        // 画面比率の問題で楕円運動になってしまっている
        return new Vector2(basePosition.x + x, basePosition.y + y);
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

