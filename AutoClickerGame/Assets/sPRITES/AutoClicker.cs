using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class AutoClicker : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI countdownAndBoostText;
    public Transform Hacker;
    public GameObject clickValueTextPrefab;
    public RectTransform canvasRect;
    public Transform Moveraqui;
    public Image BackgroundAjolote;
    public RainEfect rainefect;
    public Banner banner;

    public float moneyPerClick = 1f;
    public float moneyMultiplier = 1f;
    public float autoClickRate = 1f;

    public float money = 0f;

    private float countdownTimer = 120f;
    private bool boostActive = false;
    private float boostDuration = 60f;
    private float originalMoneyPerClick; 
    private float boostTimer = 0f; 
    private float autoClickTimer = 0f; 




    public GameObject spritePrefab;
    public float upwardForce = 1f;
    public float sideForce = 1f;
    public float fadeDuration = 1f;

    private void Start()
    {
        AudioManager.instance.Play("Music");
        originalMoneyPerClick = moneyPerClick; // Almacenamos el valor original de moneyPerClick al inicio del juego
        Application.targetFrameRate = 60;

        banner.LoadBanner();
    }

    void Update()
    {

        moneyText.text = string.Format($"Ajolotes: {money.ToString("N0")}\nAjolotes per second: {autoClickRate}");

        if (countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0)
            {
                countdownTimer = 0;
                StartCoroutine(ActivateBoost()); // Iniciamos un nuevo boost cuando el contador llegue a cero
            }
        }
        else if (boostActive)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                boostTimer = 0;
                boostActive = false; // Desactivamos el boost cuando el tiempo restante llega a cero
                moneyPerClick = originalMoneyPerClick; // Restauramos el valor original de moneyPerClick
                                                       //AudioManager.instance.Stop("Music"); // Detenemos la música cuando el boost termina
            }
        }

        float moneyGeneratedThisFrame = autoClickRate * moneyMultiplier * Time.deltaTime;

        money += moneyGeneratedThisFrame;

        UpdateCountdownAndBoostText();
    }

    public void AddClickPerSecond(float bonus)
    {
        autoClickRate += bonus;
    }

    public void AddClickPerTouch(float bonus)
    {
        if (boostActive == true)
        {
            originalMoneyPerClick += bonus;
            moneyPerClick = originalMoneyPerClick * 2;
        }
        else
        {
            moneyPerClick = originalMoneyPerClick;
            moneyPerClick += bonus;
            originalMoneyPerClick = moneyPerClick;
        }
    }

    public void Click()
    {
        var sound = AudioManager.instance.GetSound("Type");
        float numrand = Random.Range(1.5f, 3f);
        sound.source.pitch = numrand;
        sound.source.Play();
        money += moneyPerClick * moneyMultiplier;
        Vector3 clickPosition = Input.mousePosition;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, clickPosition, null, out localPoint);
        GameObject clickValueText = Instantiate(clickValueTextPrefab, canvasRect);
        clickValueText.transform.localPosition = localPoint;
        TextMeshProUGUI textMesh = clickValueText.GetComponent<TextMeshProUGUI>();
        textMesh.text = "+" + (moneyPerClick * moneyMultiplier).ToString("N0");
        AnimDes();
        Sequence mySequence = DOTween.Sequence();
        clickValueText.transform.localScale = Vector3.zero;
        textMesh.DOFade(0f,3f);
        mySequence.Append(clickValueText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack))
            .Append(clickValueText.transform.DOMove(Moveraqui.transform.position, 1f).SetEase(Ease.OutQuad)
            .OnComplete(() => { Destroy(clickValueText); }));
    }
    public void AnimDes()
    {
        if (canvasRect == null)
        {
            Debug.LogWarning("CanvasRect es nulo. No se puede animar el sprite.");
            return;
        }

        GameObject spriteObject = Instantiate(spritePrefab, canvasRect);

        RectTransform rectTransform = spriteObject.GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogWarning("RectTransform es nulo. No se puede animar el sprite.");
            Destroy(spriteObject);
            return;
        }

        Vector3 targetPosition = rectTransform.localPosition + Vector3.up * upwardForce +
                                 (Vector3)(Random.insideUnitCircle * sideForce); 

        Image image = spriteObject.GetComponent<Image>();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOLocalMove(targetPosition, fadeDuration).SetEase(Ease.OutQuad))
                .Join(image.DOFade(0f, fadeDuration * 2f)) 
                .Append(rectTransform.DOLocalMove(rectTransform.localPosition - Vector3.up * 50f, 1f).SetEase(Ease.InQuad)) 
                .OnComplete(() => { Destroy(spriteObject); }); 
    }
    public void HackerAnim()
    {
        if (Hacker.localScale == Vector3.one)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(BackgroundAjolote.DOFade(1f, 0f)).Append(Hacker.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f).OnComplete(() => { Hacker.localScale = Vector3.one; BackgroundAjolote.DOFade(0f, 0f); }));
        }
    }
    IEnumerator ActivateBoost()
    {
        boostActive = true;
        moneyPerClick *= 2;
        boostTimer = boostDuration;
        var coroutine = StartCoroutine(rainefect.SpawnRaindrops());
        yield return new WaitForSeconds(boostDuration); 
        StopCoroutine(coroutine);
        boostActive = false;
        moneyPerClick = originalMoneyPerClick;
        countdownTimer = 120f;
    }

    void UpdateCountdownAndBoostText()
    {
        // Calculamos los minutos y segundos restantes para el contador regresivo
        int minutes = Mathf.FloorToInt(countdownTimer / 60f);
        int seconds = Mathf.FloorToInt(countdownTimer % 60f);
        string countdownText = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Calculamos los minutos y segundos restantes para el boost
        minutes = Mathf.FloorToInt(boostTimer / 60f);
        seconds = Mathf.FloorToInt(boostTimer % 60f);
        string boostText = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Actualizamos el texto para mostrar tanto la cuenta regresiva como el tiempo de boost
        countdownAndBoostText.text = boostActive ? boostText : countdownText;
    }
}
