using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// うさみゅ～のベースとなるクラス
/// </summary>
public abstract class Usamyu : MonoBehaviour
{
    // フィールド生成時に渡される固有id
    private int id;
    // 生成時の初期位置
    protected Vector2 basePosition;

    //出現してからの経過時間
    protected float elapsedTime = 0f;

    private IEnumerator untilDespawn;

    // スコア
    [SerializeField] protected int baseScore; //初期スコア
    protected int currentScore; // 変動スコア

    // エフェクト
    [SerializeField] GameObject deleteEffect;

    // うさみゅ～のタイプ
    public string type;

    void Awake()
    {
        // 初期は非表示に
        this.gameObject.SetActive(false);
        currentScore = baseScore;
    }

    void Update()
    {
        // 自律移動
        this.transform.position = ScreenManager.ViewportToGamePosition(Move(), true);

        elapsedTime += Time.deltaTime; // 出現してからの経過時間

    }

    /// <summary>
    /// Instantiate後に呼び出してうさみゅ～を可視化する
    /// </summary>
    /// <param name="id">固有id</param>
    public virtual void Create(int id, Vector2 viewportPos)
    {
        // idと初期スポーン位置を保持
        this.id = id;
        basePosition = viewportPos;

        // 自律移動の初期位置を設定し，有効化
        this.transform.position = ScreenManager.ViewportToGamePosition(Move(), true);
        this.gameObject.SetActive(true);

        // うさみゅ～の鳴き声
        SoundManager.instance.PlayUsamyuSpawnSE();

        // 自然消滅までの処理開始
        untilDespawn = UntilDespawn();
        StartCoroutine(untilDespawn);
    }

    /// <summary>
    /// うさみゅ～を消去する
    /// </summary>
    public void Delete()
    {
        // うさみゅ～の鳴き声
        SoundManager.instance.PlayUsamyuSE();

        // エフェクト再生
        PlayDeleteEffect();

        // Managerに消去通知
        UsamyuManager.NotifyDeleteUsamyu(id, currentScore, true);

        // コルーチンをストップ
        StopCoroutine(untilDespawn);
    }

    private void PlayDeleteEffect()
    {
        GameObject effectObject = Instantiate(deleteEffect, this.transform.position, Quaternion.identity);
        ParticleSystem effect;

        foreach (Transform child in effectObject.transform)
        {
            effect = child.GetComponent<ParticleSystem>();
            if (effect != null)
            {
                effect.Play();
                Debug.Log("Play Effect");
            }
        }

        Destroy(effectObject, 1f);
    }

    /// <summary>
    /// うさみゅ～の自然消滅
    /// </summary>
    protected void DeleteNaturally()
    {
        // Managerに自然消滅通知
        UsamyuManager.NotifyDeleteUsamyu(id, 0, false);
    }

    /// <summary>
    /// 自律移動
    /// </summary>
    protected abstract Vector2 Move();

    /// <summary>
    /// 自然消滅までに行う処理
    /// </summary>
    protected abstract IEnumerator UntilDespawn();
}
