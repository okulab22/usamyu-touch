using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤー関連
/// 姿勢位置の入力と状態管理
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

    // プレイヤーの状態
    public enum State
    {
        Normal, Damaged, Fever
    }

    private State nowState = State.Normal;
    public static int playerLife = 0;

    // ライフ更新イベント
    public UnityEvent OnUpdateLife = new UnityEvent();

    // ゲームオーバーイベント
    public UnityEvent OnGameOver = new UnityEvent();

    void Awake()
    {
        // 姿勢位置のオブジェクト配列初期化
        posePointList = new GameObject[] { leftHand, rightHand, leftFoot, rightFoot };
    }

    void Start()
    {
        playerLife = 5;
        OnUpdateLife.Invoke();
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
                OnTouchedUsamyu(usamyu);
                usamyu.Delete();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// プレイヤーダメージ時の一定時間無敵処理
    /// </summary>
    /// <returns></returns>
    IEnumerator NoDamageCountDown()
    {
        yield return new WaitForSeconds(3);
        setState(State.Normal);
    }

    /// <summary>
    /// うさみゅ～タッチ時の処理
    /// </summary>
    /// <param name="usamyu"></param>
    private void OnTouchedUsamyu(Usamyu usamyu)
    {
        if (usamyu.type == "damage" && nowState == State.Normal)
        {
            setState(State.Damaged);
        }
    }

    /// <summary>
    /// プレイヤーがダメージを受けた時の処理
    /// </summary>
    private void OnDamaged()
    {
        playerLife--;
        OnUpdateLife.Invoke();
        Debug.Log($"Player Damaged! Life Remain: {playerLife}");

        if (playerLife <= 0)
        {
            OnGameOver.Invoke();
            Debug.Log("Game Over!!!");
        }
        else
        {
            StartCoroutine(NoDamageCountDown());
        }
    }

    /// <summary>
    /// プレイヤーの状態を設定
    /// </summary>
    /// <param name="state"></param>
    public void setState(State state)
    {
        nowState = state;
        onStateChanged(state);
        Debug.Log($"Player State Changed. {state}");
    }

    /// <summary>
    /// プレイヤーの状態変更時の処理
    /// </summary>
    /// <param name="state"></param>
    private void onStateChanged(State state)
    {
        switch (state)
        {
            case State.Normal:
                break;
            case State.Damaged:
                OnDamaged();
                break;
            case State.Fever:
                break;
            default:
                break;
        }
    }
}
