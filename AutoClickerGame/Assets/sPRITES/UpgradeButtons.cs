using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Upgrade
{
    public string name;
    public float cost;
    public float clickPerSecondBonus;
    public float clickPerTouchBonus;
}
public class UpgradeButtons : MonoBehaviour
{
    public List<Upgrade> upgrades;
    public TextMeshProUGUI moneyText;
    public AutoClicker autoClicker;


    private void Start()
    {
        UpdateUI();
    }

    public void PurchaseUpgrade(int index)
    {
        Upgrade upgrade = upgrades[index];
        if (autoClicker.money >= upgrade.cost)
        {
            AudioManager.instance.Play("Mejora");
            autoClicker.money -= upgrade.cost;
            autoClicker.AddClickPerSecond(upgrade.clickPerSecondBonus);
            autoClicker.AddClickPerTouch(upgrade.clickPerTouchBonus);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        moneyText.text = "Money: " + autoClicker.money.ToString("C0");
        // Actualizar visualización de mejoras
    }
}
