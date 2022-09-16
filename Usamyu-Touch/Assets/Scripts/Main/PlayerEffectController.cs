using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] Image frontEffect;
    [SerializeField] PlayerManager playerManager;

    void Awake()
    {
        frontEffect.color = Color.clear;
        playerManager.OnDamageStart.AddListener(StartDamageEffect);
    }

    private void StartDamageEffect()
    {
        frontEffect.color = new Color(0.6274752f, 0.07251684f, 0.9150943f, 0.5f);
        StartCoroutine(DamageEffect());
    }

    IEnumerator DamageEffect()
    {
        for (int i = 0; i < 300; i++)
        {
            frontEffect.color = Color.Lerp(new Color(0.72f, 0f, 0.78f, 0.5f), Color.clear, i / 300f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
