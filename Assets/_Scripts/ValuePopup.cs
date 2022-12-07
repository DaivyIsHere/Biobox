using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ValuePopup : MonoBehaviour
{
    [Header("Info")]
    public int displayValue;
    public ValuePopupType popupType;

    [Header("Reference")]
    [SerializeField] private TextMeshPro targetText;

    [Header("Values")]
    [SerializeField] private Vector3 punchScale;
    [SerializeField] private float punchDuration;
    [SerializeField] private int punchVibrato;
    [SerializeField] private float punchElast;
    [SerializeField] private float endPosY;
    [SerializeField] private float moveDuration;

    [Header("Damage")]
    [SerializeField] private Color damageStartColor;
    [SerializeField] private Color damageEndColor;

    [Header("Heal")]
    [SerializeField] private Color healStartColor;
    [SerializeField] private Color healEndColor;

    [Header("Shield")]
    [SerializeField] private Color shieldStartColor;
    [SerializeField] private Color shieldEndColor;

    void Start()
    {
        InitializeDisplay();
        switch (popupType)
        {
            case ValuePopupType.Damage:
                DisplayDamage();
                break;
            case ValuePopupType.Heal:
                DisplayHeal();
                break;
            case ValuePopupType.Shield:
                DisplayShield();
                break;
        }
        // TODO self Destroy
    }

    private void InitializeDisplay()
    {
        targetText.text = displayValue.ToString();
    }

    private void DisplayDamage()
    {
        targetText.color = damageStartColor;
        var sequence = DOTween.Sequence();
        sequence.Append(targetText.transform.DOPunchScale(punchScale, punchDuration, punchVibrato, punchElast));
        sequence.Join(targetText.transform.DOLocalMoveY(endPosY, moveDuration).SetEase(Ease.OutQuint));
        sequence.Join(targetText.DOColor(damageEndColor, moveDuration).SetEase(Ease.InQuint));
        sequence.OnComplete(() => SelfDestroy());
    }

    private void DisplayHeal()
    {
        targetText.color = healStartColor;
        var sequence = DOTween.Sequence();
        sequence.Append(targetText.transform.DOPunchScale(punchScale, punchDuration, punchVibrato, punchElast));
        sequence.Join(targetText.transform.DOLocalMoveY(endPosY, moveDuration).SetEase(Ease.OutQuint));
        sequence.Join(targetText.DOColor(healEndColor, moveDuration).SetEase(Ease.InQuint));
        sequence.OnComplete(() => SelfDestroy());
    }

    private void DisplayShield()
    {
        targetText.color = shieldStartColor;
        var sequence = DOTween.Sequence();
        sequence.Append(targetText.transform.DOPunchScale(punchScale, punchDuration, punchVibrato, punchElast));
        sequence.Join(targetText.transform.DOLocalMoveY(endPosY, moveDuration).SetEase(Ease.OutQuint));
        sequence.Join(targetText.DOColor(shieldEndColor, moveDuration).SetEase(Ease.InQuint));
        sequence.OnComplete(() => SelfDestroy());
    }

    private void SelfDestroy()
    {
        targetText.DOKill();
        Destroy(this.gameObject);
    }
}

public enum ValuePopupType
{
    Damage,
    Heal,
    Shield
}
