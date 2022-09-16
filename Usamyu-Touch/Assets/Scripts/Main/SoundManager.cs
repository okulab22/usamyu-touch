using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音声管理クラス
/// </summary>
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource GameBGM, fever;
    [SerializeField] private AudioSource SoundEffect;

    // うさみゅ～
    [SerializeField] private AudioClip[] countDownSE;
    [SerializeField] private AudioClip finishSE;
    [SerializeField] private AudioClip[] usamyuSpawnSE;
    [SerializeField] private AudioClip[] usamyuSE;
    [SerializeField] private AudioClip excellentSE;

    // その他
    [SerializeField] private AudioClip damageSE;
    //リザルトからタイトルに戻る時のSE
    [SerializeField] private AudioClip returnSE;
    [SerializeField] private GameObject bgmObject, feverLoopObject, soundEffectObject;

    static public SoundManager instance;

    void Awake()
    {
        // シングルトン
        if (instance == null)
        {
            instance = this;

            // 自身とAudioSourceはシーン間で保持
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(bgmObject);
            DontDestroyOnLoad(feverLoopObject);
            DontDestroyOnLoad(soundEffectObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// BGMを再生
    /// </summary>
    public void PlayBGM()
    {
        GameBGM.Play();
    }

    /// <summary>
    /// BGMを停止
    /// </summary>
    public void StopBGM()
    {
        GameBGM.Stop();
    }

    public void PlayCountDownSE(int count)
    {
        SoundEffect.PlayOneShot(countDownSE[count]);
    }

    public void PlayFinishSE()
    {
        SoundEffect.PlayOneShot(finishSE);
    }

    public void playExcellentSE()
    {
        SoundEffect.PlayOneShot(excellentSE);
    }

    public void PlayUsamyuSpawnSE()
    {
        int n = Random.Range(0, usamyuSpawnSE.Length);
        SoundEffect.PlayOneShot(usamyuSpawnSE[n]);
    }

    /// <summary>
    /// うさみゅ～の鳴き声を再生
    /// </summary>
    public void PlayUsamyuSE()
    {
        int n = Random.Range(0, usamyuSE.Length);
        SoundEffect.PlayOneShot(usamyuSE[n]);
    }

    public void PlayFeverSE()
    {
        StartCoroutine(LoopFeverSE());
    }

    public void PlayDamageSE()
    {
        SoundEffect.PlayOneShot(damageSE);
    }

    /// <summary>
    /// タイトル画面に戻るSEを再生
    /// </summary>
    public void PlayReturnSE()
    {
        SoundEffect.PlayOneShot(returnSE);
    }

    IEnumerator LoopFeverSE()
    {
        fever.Play();
        yield return new WaitForSeconds(10f);
        fever.Stop();
    }
}

