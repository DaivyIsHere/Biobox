using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ValuePopup : MonoBehaviour
{
    [Header("Info")]
    public int displayValue;
    public ValuePopupType type;

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

    void Start()
    {
        InitializeDisplay();
        switch (type)
        {
            case ValuePopupType.Damage:
                DisplayDamage();
                break;
            case ValuePopupType.Heal:
                DisplayHeal();
                break;
        }
        // TODO self Destroy
    }

    private void InitializeDisplay()
    {
        targetText.text = displayValue.ToString();
        if (type == ValuePopupType.Damage)
            targetText.color = damageStartColor;
        else if (type == ValuePopupType.Heal)
            targetText.color = healStartColor;
    }

    private void DisplayDamage()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(targetText.transform.DOPunchScale(punchScale, punchDuration, punchVibrato, punchElast));
        sequence.Join(targetText.transform.DOLocalMoveY(endPosY, moveDuration).SetEase(Ease.OutQuint));
        sequence.Join(targetText.DOColor(damageEndColor, moveDuration).SetEase(Ease.InQuint));
        sequence.OnComplete(() => SelfDestroy());
    }

    private void DisplayHeal()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(targetText.transform.DOPunchScale(punchScale, punchDuration, punchVibrato, punchElast));
        sequence.Join(targetText.transform.DOLocalMoveY(endPosY, moveDuration).SetEase(Ease.OutQuint));
        sequence.Join(targetText.DOColor(damageEndColor, moveDuration).SetEase(Ease.InQuint));
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
}
