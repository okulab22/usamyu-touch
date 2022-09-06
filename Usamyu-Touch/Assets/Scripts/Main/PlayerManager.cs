using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// 姿勢位置の入力管理
/// </summary>
public class PlayerManager : MonoBehaviour
{
    // Rayの方向と長さ
    [SerializeField] private float rayDistanceY;
    [SerializeField] private float rayDistanceZ;

    // 姿勢の各点のオブジェクト
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftFoot;
    [SerializeField] private GameObject rightFoot;

    private GameObject[] posePointList;

    void Awake()
    {
        // 姿勢位置のオブジェクト配列初期化
        posePointList = new GameObject[] { leftHand, rightHand, leftFoot, rightFoot };
    }

    /// <summary>
    /// 姿勢位置によるタッチ判定
    /// </summary>
    public void TouchedFromPose()
    {
        RaycastHit hitObject;

        // 頂点からレイを飛ばしてオブジェクトを取得
        foreach (GameObject point in posePointList)
        {
            Debug.DrawRay(point.transform.position, Vector3.down * rayDistanceY, Color.red);
            Debug.DrawRay(point.transform.position, Vector3.forward * rayDistanceZ, Color.red);
            Debug.DrawRay(point.transform.position, Vector3.back * rayDistanceZ, Color.red);

            if (Physics.Raycast(point.transform.position, Vector3.down, out hitObject, rayDistanceY))
            {
                Debug.Log(hitObject.collider.name);
                onCollisionRay(hitObject.collider.tag, hitObject.collider.gameObject);
            }

            if (Physics.Raycast(point.transform.position, Vector3.forward, out hitObject, rayDistanceZ))
            {
                Debug.Log(hitObject.collider.name);
                onCollisionRay(hitObject.collider.tag, hitObject.collider.gameObject);
            }

            if (Physics.Raycast(point.transform.position, Vector3.back, out hitObject, rayDistanceZ))
            {
                Debug.Log(hitObject.collider.name);
                onCollisionRay(hitObject.collider.tag, hitObject.collider.gameObject);
            }
        }
    }

    /// <summary>
    /// 画面クリックによるタッチ判定
    /// </summary>
    public void TouchedFromScreen()
    {
        RaycastHit hitObject;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitObject))
        {
            onCollisionRay(hitObject.collider.tag, hitObject.collider.gameObject);
        }
    }

    /// <summary>
    /// タッチ時の処理
    /// </summary>
    /// <param name="tag">オブジェクトのタグ</param>
    /// <param name="usamyuObj">Rayが衝突したGameObject</param>
    private void onCollisionRay(string tag, GameObject collisionObj)
    {
        // 各タグの処理
        switch (tag)
        {
            case "StartGame":
                TitleManager.StartGame();
                break;
            case "BackTitle":
                GameManager.BackToTitleScene();
                break;
            case "Usamyu":
                Usamyu usamyu = collisionObj.GetComponent<Usamyu>();
                usamyu.Delete();
                break;
            default:
                break;
        }
    }
}
