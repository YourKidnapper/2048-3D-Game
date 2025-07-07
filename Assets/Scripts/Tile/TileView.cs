using UnityEngine;
using TMPro;
using DG.Tweening;

public class TileView : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] allTexts;
    [SerializeField] private MeshRenderer meshRenderer;

    private Vector3 originalScale;
    private Quaternion originalRotation;
    private void Awake()
    {
        originalScale = transform.localScale;
        originalRotation = transform.localRotation;
    }

    public void ApplyVisual(int value)
    {
        string valueText = value.ToString();

        foreach (var text in allTexts)
            text.text = valueText;

        meshRenderer.material.color = GetColorByValue(value);

        AnimatePop();
        MaybeSpin();
    }

    private Color GetColorByValue(int value)
    {
        int power = (int)Mathf.Log(value, 2);
        float hue = (power * 0.1f) % 1f;
        return Color.HSVToRGB(hue, 0.6f, 1f);
    }

    private void AnimatePop()
    {
        transform.DOKill();
        transform.localScale = originalScale * 0.8f;
        transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack);
    }

    private void MaybeSpin()
    {
        float spinChance = 0.25f;
        if (Random.value > spinChance) return;

        transform.DORotate(transform.eulerAngles + RandomSpin(), 0.4f, RotateMode.FastBeyond360)
                 .SetEase(Ease.OutBack)
                 .OnComplete(() => transform.rotation = originalRotation); // reset for consistency
    }

    private Vector3 RandomSpin()
    {
        int axis = Random.Range(0, 3);
        float angle = 360f;

        return axis switch
        {
            0 => new Vector3(angle, 0, 0),
            1 => new Vector3(0, angle, 0),
            _ => new Vector3(0, 0, angle),
        };
    }

}
