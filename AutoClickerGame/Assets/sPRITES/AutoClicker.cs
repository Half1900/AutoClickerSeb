using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class AutoClicker : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public GameObject clickValueTextPrefab;
    public RectTransform canvasRect;
    public Transform Moveraqui;

    public float clickRate = 1f;
    public float moneyPerClick = 1f;
    public float moneyMultiplier = 1f;

    public float money = 0f;
    private float clickTimer = 0f;

    void Update()
    {
        clickTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Input.mousePosition;
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, clickPosition, null, out localPoint);
            GameObject clickValueText = Instantiate(clickValueTextPrefab, canvasRect);
            clickValueText.transform.localPosition = localPoint;
            TextMeshProUGUI textMesh = clickValueText.GetComponent<TextMeshProUGUI>();
            textMesh.text = "+" + (moneyPerClick * moneyMultiplier).ToString("F2");

            // Animación de escala y movimiento
            clickValueText.transform.localScale = Vector3.zero; // Inicialmente escala a cero
            clickValueText.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack); // Escala al tamaño normal con efecto OutBack
            clickValueText.transform.DOMove(Moveraqui.transform.position, 1f).SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    Destroy(clickValueText);
                    Click();
                    clickTimer = 0f;
                    //Handheld.Vibrate();
                });
            Click();
        }
        else if (clickTimer >= clickRate)
        {
            Click();
            clickTimer = 0f;
        }
        moneyText.text = "Money: $" + money.ToString("F2");
    }

    void Click()
    {
        money += moneyPerClick * moneyMultiplier;
    }

    public void UpgradeClickRate(float newRate)
    {
        clickRate = newRate;
    }

    public void UpgradeMoneyPerClick(float newMoneyPerClick)
    {
        moneyPerClick = newMoneyPerClick;
    }

    public void UpgradeMoneyMultiplier(float newMultiplier)
    {
        moneyMultiplier = newMultiplier;
    }
}
