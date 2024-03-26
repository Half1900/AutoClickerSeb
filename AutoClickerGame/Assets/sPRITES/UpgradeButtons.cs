using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeButtons : MonoBehaviour
{
    public TextMeshProUGUI costText;
    public float cost = 10f;
    public float upgradeValue = 1f;
    public AutoClicker autoClicker;
    public bool Comprado;
    public Image imagen;
    private Transform tr;
    private Button boton;
    private void Awake()
    {
        boton = GetComponent<Button>();
        tr = GetComponent<Transform>();
    }
    void Start()
    {
        costText.text = "Cost: $" + cost.ToString("F2");
        Comprado = false;
    }

    public void PurchaseUpgrade()
    {
        if (Comprado == false)
        {
            if (autoClicker.money >= cost)
            {
                autoClicker.money -= cost;
                autoClicker.moneyText.text = "Money: $" + autoClicker.money.ToString("F2");
                Upgrade();
                Comprado = true;
                boton.interactable = false;
                AudioManager.instance.Play("pop");
                tr.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).OnComplete(() => { tr.localScale = Vector3.one; });
                tr.localScale = Vector3.one;
            }
            else
            {
                AudioManager.instance.Play("error");
            }
        }
        else
        {
            
            Debug.Log("Ya esta comprado!");
        }
    }

    private void Upgrade()
    {
        var texto = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        texto.text = "Comprado";
        imagen.color = Color.black;
        autoClicker.moneyPerClick = upgradeValue;
        autoClicker.originalMoneyPerClick = upgradeValue;
        cost *= 2f;
        costText.text = "Cost: $" + cost.ToString("F2");
        
    }
}
