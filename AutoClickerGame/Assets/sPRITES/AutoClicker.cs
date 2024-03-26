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
    public GameObject clickValueTextPrefab;
    public RectTransform canvasRect;
    public Transform Moveraqui;

    public float moneyPerClick = 1f;
    public float moneyMultiplier = 1f;
    public Color Azul;
    public Image luces;

    public float money = 0f;

    private bool gameEnded = false; // Variable para controlar si el juego ha terminado
    private float countdownTimer = 300f; // Contador regresivo de 5 minutos (300 segundos)
    private bool boostActive = false; // Variable para controlar si el impulso de moneyPerClick está activo
    private float boostDuration = 60f; // Duración del impulso de 1 minuto (60 segundos)
    public float originalMoneyPerClick; // Almacena el valor original de moneyPerClick
    private float boostTimer = 0f; // Temporizador para rastrear el tiempo restante del boost

    private void Start()
    {
        AudioManager.instance.Play("Type");
        originalMoneyPerClick = moneyPerClick; // Almacenamos el valor original de moneyPerClick al inicio del juego
    }

    void Update()
    {
        if (!gameEnded && Input.GetMouseButtonDown(0)) // Solo actualizamos si el juego no ha terminado y se hace clic
        {
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
                .OnComplete(() => { Destroy(clickValueText); Click(); }));
        }

        moneyText.text = "Money: " + money.ToString("C0");

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
                AudioManager.instance.Stop("Music");
                luces.color = Azul;
            }
        }

        // Actualizamos el texto del contador regresivo y el tiempo de boost
        UpdateCountdownAndBoostText();
    }

    void Click()
    {
        money += moneyPerClick * moneyMultiplier;
    }

    IEnumerator ActivateBoost()
    {
        boostActive = true; // Marcamos que el impulso está activo
        moneyPerClick = 10f; // Ajustamos moneyPerClick a 10
        AudioManager.instance.Play("Music");
        luces.color = Color.red;
        boostTimer = boostDuration; // Inicializamos el temporizador del boost
        yield return new WaitForSeconds(boostDuration); // Esperamos la duración del impulso
        boostActive = false; // Marcamos que el impulso ha terminado
        moneyPerClick = originalMoneyPerClick; // Restauramos el valor original de moneyPerClick
        countdownTimer = 300f; // Reiniciamos el contador regresivo
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
        this.countdownAndBoostText.text = boostActive ? boostText : countdownText;
    }
}
