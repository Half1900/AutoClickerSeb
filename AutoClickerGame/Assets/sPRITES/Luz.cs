using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Luz : MonoBehaviour
{
    private Image Luces;

    private void Awake()
    {
        Luces = GetComponent<Image>();
    }
    private void Start()
    {
        OnandOff();
    }
    public void OnandOff()
    {
        Sequence offandon = DOTween.Sequence();
        float randomvalue = Random.Range(0.3f,0.7f);
        offandon.Append(Luces.DOFade(0.3f, randomvalue)).Append(Luces.DOFade(1f, randomvalue)).OnComplete(() => { OnandOff(); });
    }
}
