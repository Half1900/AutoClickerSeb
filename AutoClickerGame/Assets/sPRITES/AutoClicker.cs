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

    public float moneyPerClick = 1f;
    public float moneyMultiplier = 1f;
    public float autoClickRate = 1f;

    public float money = 0f;

    private float countdownTimer = 120f;
    public bool boostActive = false;
    public float boostDuration = 60f;
    private float originalMoneyPerClick;
    private float originalMoneyPerSecond;
    private float boostTimer = 0f; 
    private float autoClickTimer = 0f;
    public float multiplicador = 2;

    public GameObject spritePrefab;
    public float upwardForce = 1f;
    public float sideForce = 1f;
    public float fadeDuration = 1f;
    public TextMeshProUGUI TEXTOX;
    public static AutoClicker instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        AudioManager.instance.Play("Music");
        originalMoneyPerClick = moneyPerClick; // Almacenamos el valor original de moneyPerClick al inicio del juego
        originalMoneyPerSecond = autoClickRate;
        Application.targetFrameRate = 60;
    }

    void Update()
    {

        moneyText.text = string.Format($"Ajolotes: { AbbreviateNumber(money)}\nAjolotes per second: {AbbreviateNumber(autoClickRate)}\nAjolotes per Click: {AbbreviateNumber(moneyPerClick)}");

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
            countdownTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                boostTimer = 0;
                boostActive = false; // Desactivamos el boost cuando el tiempo restante llega a cero
                moneyPerClick = originalMoneyPerClick; // Restauramos el valor original de moneyPerClick
                autoClickRate = originalMoneyPerSecond;
            }
        }

        float moneyGeneratedThisFrame = autoClickRate * moneyMultiplier * Time.deltaTime;

        money += moneyGeneratedThisFrame;

        UpdateCountdownAndBoostText();
    }

    public void AddClickPerSecond(float bonus)
    {
        if (boostActive == true)
        {
            originalMoneyPerSecond += bonus;
            autoClickRate = originalMoneyPerSecond * multiplicador;
        }
        else
        {
            autoClickRate = originalMoneyPerSecond;
            autoClickRate += bonus;
            originalMoneyPerSecond = autoClickRate;
        }
    }

    public void AddClickPerTouch(float bonus)
    {
        if (boostActive == true)
        {
            originalMoneyPerClick += bonus;
            moneyPerClick = originalMoneyPerClick * multiplicador;
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
        textMesh.text = "+" + AbbreviateNumber(moneyPerClick * moneyMultiplier);
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
    public IEnumerator ActivateBoost()
    {
        boostActive = true;
        moneyPerClick *= multiplicador;
        autoClickRate *= multiplicador;
        countdownTimer = boostDuration;
        var coroutine = StartCoroutine(RainEfect.Instance.SpawnRaindrops());
        yield return new WaitForSeconds(boostDuration); 
        boostActive = false;
        multiplicador = 2;
        TEXTOX.text = "X2";
        boostDuration = 60f;
        moneyPerClick = originalMoneyPerClick;
        autoClickRate = originalMoneyPerSecond;
        countdownTimer = 120f;
        StopAllCoroutines();
    }

    public void UpdateCountdownAndBoostText()
    {
        // Calculamos los minutos y segundos restantes para el contador regresivo
        int minutes = Mathf.FloorToInt(countdownTimer / 60f);
        int seconds = Mathf.FloorToInt(countdownTimer % 60f);
        string countdownText = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Calculamos los minutos y segundos restantes para el boost
        minutes = Mathf.FloorToInt(countdownTimer / 60f);
        seconds = Mathf.FloorToInt(countdownTimer % 60f);
        string boostText = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Actualizamos el texto para mostrar tanto la cuenta regresiva como el tiempo de boost
        countdownAndBoostText.text = boostActive ? boostText : countdownText;
    }
    string AbbreviateNumber(double number)
    {
        if (number >= 1e12) // Más de un billón
        {
            return (number / 1e12).ToString("0.###") + "T";
        }
        else if (number >= 1e9) // Más de mil millones
        {
            return (number / 1e9).ToString("0.###") + "B";
        }
        else if (number >= 1e6) // Más de un millón
        {
            return (number / 1e6).ToString("0.###") + "M";
        }
        else if (number >= 1e3) // Más de mil
        {
            return (number / 1e3).ToString("0.###") + "K";
        }
        else
        {
            return number.ToString("0");
        }
    }
}
