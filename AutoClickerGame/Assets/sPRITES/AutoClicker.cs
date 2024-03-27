using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class AutoClicker : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI countdownAndBoostText; // Texto para mostrar tanto la cuenta regresiva como el tiempo de boost
    public Transform Hacker;
    public GameObject clickValueTextPrefab;
    public RectTransform canvasRect;
    public Transform Moveraqui;

    public float moneyPerClick = 1f;
    public float moneyMultiplier = 1f;
    public float autoClickRate = 1f; // Cantidad de dinero que se genera automáticamente por segundo

    public float money = 0f;

    private bool gameEnded = false; // Variable para controlar si el juego ha terminado
    private float countdownTimer = 120f; // Contador regresivo de 2 minutos (120 segundos)
    private bool boostActive = false; // Variable para controlar si el impulso de moneyPerClick está activo
    private float boostDuration = 60f; // Duración del impulso de 1 minuto (60 segundos)
    private float originalMoneyPerClick; // Almacena el valor original de moneyPerClick
    private float boostTimer = 0f; // Temporizador para rastrear el tiempo restante del boost
    private float autoClickTimer = 0f; // Temporizador para el clic automático

    private void Start()
    {
        //AudioManager.instance.Play("Type");
        originalMoneyPerClick = moneyPerClick; // Almacenamos el valor original de moneyPerClick al inicio del juego
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        
        moneyText.text = "Ajolotes: " + money.ToString();

        // Actualizamos el tiempo de boost y la cuenta regresiva en el mismo texto
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

        // Actualizamos el temporizador de clics automáticos y generamos dinero automáticamente
        autoClickTimer += Time.deltaTime;
        if (autoClickTimer >= 1f / autoClickRate)
        {
            money += autoClickRate * moneyMultiplier;
            autoClickTimer = 0f; // Reiniciar el temporizador
        }

        // Actualizamos el texto del contador regresivo y el tiempo de boost
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
        float numrand = Random.Range(2f, 2.8f);
        sound.source.pitch = numrand;
        sound.source.Play();
        money += moneyPerClick * moneyMultiplier;
        // Crear texto de valor de clic
        Vector3 clickPosition = Input.mousePosition;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, clickPosition, null, out localPoint);
        GameObject clickValueText = Instantiate(clickValueTextPrefab, canvasRect);
        clickValueText.transform.localPosition = localPoint;
        TextMeshProUGUI textMesh = clickValueText.GetComponent<TextMeshProUGUI>();
        textMesh.text = "+" + (moneyPerClick * moneyMultiplier).ToString("C0");
        // Animación de escala y movimiento
        Sequence mySequence = DOTween.Sequence();
        clickValueText.transform.localScale = Vector3.zero;
        mySequence.Append(clickValueText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack))
            .Append(clickValueText.transform.DOMove(Moveraqui.transform.position, 1f).SetEase(Ease.OutQuad)
            .OnComplete(() => { Destroy(clickValueText); }));
    }
    public void HackerAnim()
    {
        if (Hacker.localScale == Vector3.one)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(Hacker.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f).OnComplete(() => { Hacker.localScale = Vector3.one; }));
        }
    }

    IEnumerator ActivateBoost()
    {
        boostActive = true; // Marcamos que el impulso está activo
        moneyPerClick *= 2; // Ajustamos moneyPerClick * 2
        boostTimer = boostDuration; // Inicializamos el temporizador del boost
        //AudioManager.instance.Play("Music"); // Iniciamos la música cuando comienza el boost
        yield return new WaitForSeconds(boostDuration); // Esperamos la duración del impulso
        boostActive = false; // Marcamos que el impulso ha terminado
        moneyPerClick = originalMoneyPerClick; // Restauramos el valor original de moneyPerClick
        countdownTimer = 120f; // Reiniciamos el contador regresivo
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
