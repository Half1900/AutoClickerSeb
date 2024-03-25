using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtons : MonoBehaviour
{
    public TextMeshProUGUI costText;
    public float cost = 10f;
    public float upgradeValue = 1f;
    public AutoClicker autoClicker;
    public bool Comprado;
    public Image imagen;

    void Start()
    {
        costText.text = "Cost: $" + cost.ToString("F2");
        Comprado = false;
    }

    public void PurchaseUpgrade()
    {
        if (!Comprado)
        {
            if (autoClicker.money >= cost)
            {
                autoClicker.money -= cost;
                autoClicker.moneyText.text = "Money: $" + autoClicker.money.ToString("F2");
                Upgrade();
                Comprado = true;
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
        autoClicker.UpgradeMoneyPerClick(upgradeValue);
        cost *= 2f;
        costText.text = "Cost: $" + cost.ToString("F2");
    }
}
