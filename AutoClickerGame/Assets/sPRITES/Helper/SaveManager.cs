using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public static class SaveManager
{
    public static void SavePlayerData(AutoClicker player)
    {
        PlayerData playerData = new PlayerData(player);
        string dataPath = Application.persistentDataPath + "/player.save";
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, playerData);
        fileStream.Close();
    }
    public static PlayerData LoadPlayerData()
    {
        string dataPath = Application.persistentDataPath + "/player.save";

        if (File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            PlayerData playerData = (PlayerData)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return playerData;
        }
        else
        {
            return null;
        }
    }
    public static void DeletePlayerData()
    {
        string dataPath = Application.persistentDataPath + "/player.save";

        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
[System.Serializable]
public class UpgradeData
{
    public string nombre;
    public float cost;
    public float clickPerSecondBonus;
    public float clickPerTouchBonus;
    public bool Activado;
}

[System.Serializable]
public class PlayerData
{
    public float moneyPerClick;
    public float autoClickRate;
    public float money;
    public List<UpgradeData> upgradeData = new List<UpgradeData>();

    public PlayerData(AutoClicker player)
    {
        moneyPerClick = player.moneyPerClick;
        autoClickRate = player.autoClickRate;
        money = player.money;
        foreach (var upgrade in player.upgrades)
        {
            UpgradeData data = new UpgradeData();
            data.nombre = upgrade.nombre;
            data.cost = upgrade.cost;
            data.clickPerSecondBonus = upgrade.clickPerSecondBonus;
            data.clickPerTouchBonus = upgrade.clickPerTouchBonus;
            data.Activado = upgrade.Activado;
            upgradeData.Add(data);
        }
    }
}
