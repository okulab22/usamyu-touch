using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル画面の音声管理クラス
/// </summary>
public class TitleSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource SoundEffect;
    // スタートボタン押下時のSE
    [SerializeField] private AudioClip startSE;
    [SerializeField] private GameObject bgmObject, soundEffectObject;

    static public TitleSoundManager instance;

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
        BGM.Play();
    }

    /// <summary>
    /// BGMを停止
    /// </summary>
    public void StopBGM()
    {
        BGM.Stop();
    }

    /// <summary>
    /// タイトル画面に戻るSEを再生
    /// </summary>
    public void PlayStartSE()
    {
        SoundEffect.PlayOneShot(startSE);
    }
}

