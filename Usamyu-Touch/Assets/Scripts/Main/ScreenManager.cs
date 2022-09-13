using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Viewport座標からゲーム内スクリーン座標へ変換などを行うクラス
/// </summary>
public class ScreenManager : MonoBehaviour
{
    static private Vector3 frontBottomLeftPoint = Vector3.zero;
    private Vector3 frontTopRightPoint = Vector3.zero;
    static private Vector2 frontScreenSize = Vector2.zero;

    void Awake()
    {
        // スクリーン左下に該当する座標と奥行きを取得
        RaycastHit hit;
        Ray bottomLeftRay = Camera.main.ViewportPointToRay(new Vector3(0, 0, 10));
        if (Physics.Raycast(bottomLeftRay, out hit))
        {
            frontBottomLeftPoint = hit.point;
            frontBottomLeftPoint.z -= .01f; // 少し手前に表示するため
        }

        // スクリーン右上に該当する座標を取得
        frontTopRightPoint = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // スクリーンサイズ算出
        frontScreenSize =
            new Vector2(frontTopRightPoint.x - frontBottomLeftPoint.x,
                        frontTopRightPoint.y - frontBottomLeftPoint.y);
    }

    /// <summary>
    /// Viewport座標からゲーム内座標に変換
    /// </summary>
    /// <param name="viewportPos"></param>
    /// <param name="isFront"></param>
    /// <returns></returns>
    public static Vector3 ViewportToGamePosition(Vector2 viewportPos, bool isFront)
    {
        Vector3 delta, gamePos;
        if (isFront)
        {
            delta = Vector2.Scale(frontScreenSize, viewportPos);
            gamePos = delta + frontBottomLeftPoint;
        }
        else
        {
            // TODO: 床実装時は床の座標に置き換える
            delta = Vector2.Scale(frontScreenSize, viewportPos);
            gamePos = delta + frontBottomLeftPoint;
        }

        return gamePos;
    }

    /// <summary>
    /// ゲーム座標からViewport座標に変換
    /// </summary>
    /// <param name="gamePos"></param>
    /// <returns></returns>
    public static Vector2 GamePositionToViewportPosition(Vector3 gamePos)
    {
        Vector2 viewportPos = gamePos / frontScreenSize;

        // 画面中央が原点なので0.5加算
        viewportPos.x += .5f;

        return viewportPos;
    }
}
