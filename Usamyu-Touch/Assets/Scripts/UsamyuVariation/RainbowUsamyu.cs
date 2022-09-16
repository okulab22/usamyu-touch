using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 虹色うさみゅ～
/// </summary>
public class RainbowUsamyu : Usamyu
{
    // 生存時間 [s]
    // PrefabのInspectorで設定する
    [SerializeField] private int survivalTime;

    // 移動演算に必要な変数
    private float x, y;
    private float radius = 0.3f;
    private float speed = 10f;

    private int dirdecx, dirdecy;
    private readonly int[] direc = new int[] { -1, 1 };

    void Start(){
        dirdecx = Random.Range(0,2);
        dirdecy = Random.Range(0,2);
    }

    /// <summary>
    /// うさみゅ～の移動処理
    /// フレーム毎に呼び出される
    /// </summary>
    /// <returns>Vector2(x, y) 移動先のViewport座標</returns>
    protected override Vector2 Move()
    {
        
        x = direc[dirdecx] * radius * Mathf.Sin(Time.time * speed);
        y = direc[dirdecy] * radius * Mathf.Cos(Time.time * speed);

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

        // survivalTime秒待機
        yield return new WaitForSeconds(survivalTime);

        // この関数を呼び出すと自然消滅する
        DeleteNaturally();
    }
}
