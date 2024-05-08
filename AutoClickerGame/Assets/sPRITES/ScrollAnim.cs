using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScrollAnim : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool isVisible = false;
    private ScrollRect scrollRect;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        scrollRect = GetComponentInParent<ScrollRect>();
    }

    void Update()
    {
        bool newVisibility = IsVisibleInViewport(rectTransform, scrollRect.viewport);

        if (newVisibility != isVisible)
        {
            isVisible = newVisibility;

            if (isVisible)
            {
                // Cuando el objeto se vuelve visible en el viewport del ScrollView, hacer que su escala pase a 1
                transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            }
            else
            {
                // Cuando el objeto sale del viewport del ScrollView, hacer que su escala pase a 0
                transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
            }
        }
    }
    bool IsVisibleInViewport(RectTransform rt, RectTransform viewport)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Vector2 viewportSize = Vector2.Scale(viewport.rect.size, viewport.lossyScale);
        Rect viewportRect = new Rect(viewport.position.x, viewport.position.y, viewportSize.x, viewportSize.y);

        foreach (Vector3 corner in corners)
        {
            Vector2 viewportPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, corner, null, out viewportPoint);

            if (!viewportRect.Contains(viewportPoint))
            {
                return false;
            }
        }

        return true;
    }
}
