using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音声管理クラス
/// </summary>
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource GameBGM;
    [SerializeField] private AudioSource SoundEffect;
    [SerializeField] private AudioClip[] countDownSE;
    [SerializeField] private AudioClip finishSE;
    //鳴き声は配列にする．
    [SerializeField] private AudioClip[] usamyuSE;
    //リザルトからタイトルに戻る時のSE
    [SerializeField] private AudioClip returnSE;
    [SerializeField] private GameObject bgmObject, soundEffectObject;

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

    /// <summary>
    /// うさみゅ～の鳴き声を再生
    /// </summary>
    public void PlayUsamyuSE()
    {
        int n = Random.Range(0, usamyuSE.Length);
        SoundEffect.PlayOneShot(usamyuSE[n]);
    }

    /// <summary>
    /// タイトル画面に戻るSEを再生
    /// </summary>
    public void PlayReturnSE()
    {
        SoundEffect.PlayOneShot(returnSE);
    }
}

