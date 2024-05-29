using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public enum StateMultiplier
{
    None,
    Dos,
    Cinco
}
public class AutoClicker : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI PerClick;
    public TextMeshProUGUI PerSecond;
    public TextMeshProUGUI countdownAndBoostText;
    public Transform Ajolote;
    public GameObject clickValueTextPrefab;
    public RectTransform canvasRect;
    public Transform Moveraqui;
    public Image BackgroundAjolote;
    public List<UpgradesSO> upgrades = new List<UpgradesSO>();
    public List<UpgradeData> upgradeData = new List<UpgradeData>();

    public float moneyPerClick = 1f;
    public float moneyMultiplier = 1f;
    public float autoClickRate = 1f;

    public float money = 0f;

    public bool boostActive = false;
    public float boostDuration = 60f;
    private float originalMoneyPerClick;
    private float originalMoneyPerSecond;
    public float multiplicador = 2f;

    public GameObject spritePrefab;
    public float upwardForce = 1f;
    public float sideForce = 1f;
    public float fadeDuration = 3f;
    public TextMeshProUGUI TEXTOX;
    public static AutoClicker instance;
    public StateMultiplier stateMultiplier;
    public bool BoostCinco;
    private Coroutine anim;
    public List<Sound> Sounds;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }
    // Método para cargar datos del jugador
    public void CargarDatos()
    {
        PlayerData playerData = SaveManager.LoadPlayerData();
        moneyPerClick = playerData.moneyPerClick;
        autoClickRate = playerData.autoClickRate;
        money = playerData.money;
        upgradeData = playerData.upgradeData; // Cambio aquí
    }

    // Método para guardar datos del jugador
    public void GuardarDatos()
    {
        SaveManager.SavePlayerData(this);
    }
    private void Start()
    {
        // Cargar datos del jugador
        if (SaveManager.LoadPlayerData() == null)
        {
            SaveManager.SavePlayerData(this);
        }
        else
        {
            CargarDatos();
        }
        Sound soundUno = AudioManager.instance.GetSound("FootSteep1");
        Sound soundDos = AudioManager.instance.GetSound("FootSteep2");
        Sounds.Add(soundUno);
        Sounds.Add(soundDos);

        stateMultiplier = StateMultiplier.None;
        AudioManager.instance.Play("Music");
        originalMoneyPerClick = moneyPerClick;
        originalMoneyPerSecond = autoClickRate;
        Application.targetFrameRate = 90;

        InvokeRepeating("GuardarDatos", 1.0f, 1.0f);
    }

    void Update()
    {
        moneyText.text = string.Format($"Stars: { NumberAbbreviator.AbbreviateNumber(money)}");
        PerClick.text = string.Format($"Click: {NumberAbbreviator.AbbreviateNumber(originalMoneyPerClick)}");
        PerSecond.text = string.Format($"Second: {NumberAbbreviator.AbbreviateNumber(originalMoneyPerSecond)}");
        
        if (boostActive == false)
        {
            moneyPerClick = originalMoneyPerClick;
            autoClickRate = originalMoneyPerSecond;
        }

        float moneyGeneratedThisFrame = autoClickRate * moneyMultiplier * Time.deltaTime;

        money += moneyGeneratedThisFrame;
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
    public void ChangeState(StateMultiplier state)
    {
        stateMultiplier = state;

        switch (state)
        {
            case StateMultiplier.None:
                moneyMultiplier = 1f;
                StopAllCoroutines();
                RainEfect.Instance.CanSpawn = true;
                break;
            case StateMultiplier.Dos:
                moneyMultiplier = 2f;
                if (RainEfect.Instance.CanSpawn)
                {
                    StartCoroutine(RainEfect.Instance.SpawnRaindrops());
                    RainEfect.Instance.CanSpawn = false;
                }
                break;
            case StateMultiplier.Cinco:
                BoostCinco = true;
                moneyMultiplier = 5f;
                TEXTOX.text = "X5";
                if (RainEfect.Instance.CanSpawn)
                {
                    StartCoroutine(RainEfect.Instance.SpawnRaindrops());
                    RainEfect.Instance.CanSpawn = false;
                }
                break;
        }
    }
    public void ActiveAndDesactiveBoost()
    {
        boostActive = !boostActive;
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
        float numrand = Random.Range(2f, 3f);
        int soundRand = Random.Range(0, Sounds.Count);
        Sound sound = Sounds[soundRand];
        sound.source.pitch = numrand;
        sound.source.Play();
        money += moneyPerClick * moneyMultiplier;
        Vector3 clickPosition = Input.mousePosition;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, clickPosition, null, out localPoint);
        GameObject clickValueText = Instantiate(clickValueTextPrefab, canvasRect);
        clickValueText.transform.localPosition = localPoint;
        TextMeshProUGUI textMesh = clickValueText.GetComponent<TextMeshProUGUI>();
        textMesh.text = "+" + NumberAbbreviator.AbbreviateNumber(moneyPerClick * moneyMultiplier);
        AnimDes(localPoint);
        Sequence mySequence = DOTween.Sequence();
        clickValueText.transform.localScale = Vector3.zero;
        textMesh.DOFade(0f,3f);
        mySequence.Append(clickValueText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack))
            .Append(clickValueText.transform.DOMove(Moveraqui.transform.position, 1f).SetEase(Ease.OutQuad)
            .OnComplete(() => { Destroy(clickValueText); }));
    }
    public void AnimDes(Vector2 localPoint)
    {
        if (canvasRect == null)
        {
            Debug.LogWarning("CanvasRect es nulo. No se puede animar el sprite.");
            return;
        }

        GameObject spriteObject = Instantiate(spritePrefab, canvasRect);
        spriteObject.transform.localPosition = localPoint;

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
    public void AjolotePunch()
    {
        if (Ajolote.localScale == Vector3.one)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(Ajolote.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f).OnComplete(() => { Ajolote.localScale = Vector3.one;}));
        }
    }
    public void EliminarDatos()
    {
        SaveManager.DeletePlayerData();
    }
}
