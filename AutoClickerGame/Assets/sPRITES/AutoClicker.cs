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
    public TextMeshProUGUI countdownAndBoostText;
    public Transform Ajolote;
    public GameObject clickValueTextPrefab;
    public RectTransform canvasRect;
    public Transform Moveraqui;
    public Image BackgroundAjolote;

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
    public float fadeDuration = 1f;
    public TextMeshProUGUI TEXTOX;
    public static AutoClicker instance;
    public StateMultiplier stateMultiplier;
    public bool BoostCinco;
    private Coroutine anim;

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
    private void Start()
    {
        stateMultiplier = StateMultiplier.None;
        AudioManager.instance.Play("Music");
        originalMoneyPerClick = moneyPerClick;
        originalMoneyPerSecond = autoClickRate;
        Application.targetFrameRate = 90;
    }

    void Update()
    {
       
        moneyText.text = string.Format($"Ajolotes: { NumberAbbreviator.AbbreviateNumber(money)}");
        
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
        }else
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
                break;
            case StateMultiplier.Dos:
                moneyMultiplier = 2f;
                StartCoroutine(RainEfect.Instance.SpawnRaindrops());
                break;
            case StateMultiplier.Cinco:
                BoostCinco = true;
                moneyMultiplier = 5f;
                TEXTOX.text = "X5";
                StartCoroutine(RainEfect.Instance.SpawnRaindrops());
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
        }else
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
    public void HackerAnim()
    {
        if (Ajolote.localScale == Vector3.one)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(BackgroundAjolote.DOFade(1f, 0f)).Append(Ajolote.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f).OnComplete(() => { Ajolote.localScale = Vector3.one; BackgroundAjolote.DOFade(0f, 0f); }));
        }
    }
}
