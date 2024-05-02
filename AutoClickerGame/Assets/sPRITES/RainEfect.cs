using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEfect : MonoBehaviour
{
    public static RainEfect Instance;
    public GameObject raindrops;
    public int numberOfRaindrops = 30;
    public float fallDuration = 2f;
    public RectTransform canvasRect;
    public bool CanSpawn = true;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    public IEnumerator SpawnRaindrops()
    {
        while (true) // Bucle infinito
        {
            for (int i = 0; i < numberOfRaindrops; i++)
            {
                GameObject raindrop = Instantiate(raindrops, canvasRect);
                RectTransform rectTransform = raindrop.GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector3(Random.Range(-500f, 500f), 1000, 0);
                rectTransform.DOMoveY(-10f, fallDuration).SetEase(Ease.Linear).SetDelay(Random.Range(0f, 1f)).OnComplete(() => { Destroy(raindrop); });
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0f); // Espera entre cada iteración del bucle
        }
    }
}
