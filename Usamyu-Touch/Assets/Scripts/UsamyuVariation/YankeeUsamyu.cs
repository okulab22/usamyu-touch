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
    private float speed = 0.5f;

    /// <summary>
    /// うさみゅ～の移動処理
    /// フレーム毎に呼び出される
    /// </summary>
    /// <returns>Vector2(x, y) 移動先のViewport座標</returns>
    protected override Vector2 Move()
    {
        return new Vector2(basePosition.x, basePosition.y - speed*elapsedTime);
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

