using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Luz : MonoBehaviour
{
    private Image Luces;
    public Transform imageTransform;
    public float rotationSpeed = 360f; // Velocidad de rotación en grados por segundo
    private void Awake()
    {
        Luces = GetComponent<Image>();
    }
    void Start()
    {
        OnandOff();
        imageTransform.DORotate(new Vector3(0f, 0f, 360f), rotationSpeed, RotateMode.FastBeyond360)
                     .SetLoops(-1, LoopType.Restart);
    }
    public void OnandOff()
    {
        Sequence offandon = DOTween.Sequence();
        float randomvalue = Random.Range(0.4f,0.7f);
        offandon.Append(Luces.DOFade(0.5f, randomvalue)).Append(Luces.DOFade(1f, randomvalue)).OnComplete(() => { OnandOff(); });
    }
}
