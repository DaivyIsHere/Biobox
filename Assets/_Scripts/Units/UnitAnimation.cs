using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Unit))]
public class UnitAnimation : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Unit _unit;
    [SerializeField] private Transform _unitSprite;//the unit sprite
    [SerializeField] private Transform _unitObject;//the whole unit object
    [SerializeField] private SpriteRenderer _unitBG;//the unit BG

    [Header("DamageEffect")]
    [SerializeField] private ValuePopup _valuePopupPref;
    [SerializeField] private Vector3 _valuePopupOffset_Damage;
    [SerializeField] private Vector3 _valuePopupOffset_Shield;

    [Header("StartValue")]
    private Vector3 _startLocalPos;
    private Vector3 _startLocalRot;
    private Vector3 _startLocalScale;

    [Header("Values")]
    [SerializeField] private Vector3 _attackVector = new Vector3(0.5f, 0, 0);
    [SerializeField] private float _attackDuration = 0.5f;
    [SerializeField] private int _attackVibrato = 6;
    [SerializeField] private float _attackElast = 0.2f;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color exhaustedColor;

    [Header("Sequence")]
    private Sequence _sequence;

    void Start()
    {
        _startLocalPos = _unitSprite.localPosition;
        _startLocalRot = _unitSprite.localEulerAngles;
        _startLocalScale = _unitSprite.localScale;
    }

    //A method to prevent tween overlap that changes the start/end postion or scales
    public void ResetAllTweens()
    {
        _unitSprite.DOComplete();
        _unitSprite.localPosition = _startLocalPos;
        _unitSprite.localEulerAngles = _startLocalRot;
        _unitSprite.localScale = _startLocalScale;
    }

    public void PlayAttack()
    {
        //print(_unit.unitData.unitName + " "+ _unit.orderInBox + " play attack");
        Vector3 punchVector = _attackVector;
        if (_unit.unitLabel.boxSide == BoxSide.RightSide)
            punchVector *= -1;

        ResetAllTweens();
        _unitSprite.DOPunchPosition(punchVector, _attackDuration, _attackVibrato, _attackElast);
        PlayExhaust();
    }

    public void PlayTakeDamage(int damageValue, int healthAfter, bool causeDeath)
    {
        ValuePopup valuePopup = Instantiate(_valuePopupPref, transform.position + _valuePopupOffset_Damage, Quaternion.identity);
        valuePopup.popupType = ValuePopupType.Damage;
        valuePopup.displayValue = damageValue;

        _unit.UpdateHealthDisplay(healthAfter);
        CCommand.CommandExecutionComplete();

        //if(!causeDeath)
        //  PlayHurt();
        //else
        //  PlayDeath();
    }

    public void PlayShieldBreak(int shieldBreakValue, int shieldAfter)
    {
        ValuePopup valuePopup = Instantiate(_valuePopupPref, transform.position + _valuePopupOffset_Shield, Quaternion.identity);
        valuePopup.popupType = ValuePopupType.Shield;
        valuePopup.displayValue = shieldBreakValue;

        _unit.UpdateShieldDisplay(shieldAfter);

        _sequence = DOTween.Sequence();//Start sequence
        _sequence.AppendInterval(0.5f);
        _sequence.OnComplete(CCommand.CommandExecutionComplete);
    }

    public void PlayHeal(int value, int valueAfter)
    {
        ValuePopup valuePopup = Instantiate(_valuePopupPref, transform.position + _valuePopupOffset_Damage, Quaternion.identity);
        valuePopup.popupType = ValuePopupType.Heal;
        valuePopup.displayValue = value;

        _unit.UpdateHealthDisplay(valueAfter);
        _sequence = DOTween.Sequence();//Start sequence
        _sequence.AppendInterval(0.5f);
        _sequence.OnComplete(CCommand.CommandExecutionComplete);
    }

    public void PlayHurt()
    {
        ResetAllTweens();
        _sequence = DOTween.Sequence();//Start sequence
        _sequence.AppendInterval(0.075f);
        _sequence.Append(_unitSprite.DOPunchScale(new Vector3(-0.3f, 0.4f, 0), _attackDuration, _attackVibrato, _attackElast));
        _sequence.OnComplete(CCommand.CommandExecutionComplete);
    }

    public void PlayAlign(Vector3 newPosition, float duration)
    {
        //print(_unit.unitData.unitName + " "+ _unit.orderInBox + " play align");
        _unitObject.DOLocalMove(newPosition, duration);//.SetEase(Ease.InOutSine);
    }

    public void PlayDeath()
    {
        // TODO play deathAnimation
        ResetAllTweens();
        _sequence = DOTween.Sequence();//Start sequence
        _sequence.Append(_unitSprite.DOScaleY(0.2f,0.5f));
        _sequence.AppendInterval(0f);
        _sequence.OnComplete(_unit.DestroySelf);
        ///We called CCommand.CommandExecutionComplete in _unit.DestroySelf

        //_unitSprite.DOKill();
        //_sequence.Kill();// Note : you cannot use DoKill to kill sequence, you need the direct reference
    }

    public void PlayExhaust()
    {
        _unitBG.DOColor(exhaustedColor, 0.2f);
    }

    public void PlayUnexhaust()
    {
        _unitBG.DOColor(defaultColor, 0.5f);
    }

    public void PlayAttackChange(int changeValue, int valueAfter)
    {
        if (changeValue > 0)
        {
            _unit.attackDisplay.transform.DOPunchPosition(new Vector3(0, 0.3f, 0), 0.2f, 10, 0.5f).OnComplete(() =>
                CCommand.CommandExecutionComplete());
        }
        else
        {
            _unit.attackDisplay.transform.DOPunchPosition(new Vector3(0, -0.3f, 0), 0.2f, 10, 0.5f).OnComplete(() =>
                CCommand.CommandExecutionComplete());
        }

        _unit.UpdateAttackDisplay(valueAfter);
    }

    public void PlayHealthChange(int changeValue, int valueAfter)
    {
        if (changeValue > 0)
        {
            _unit.healthDisplay.transform.DOPunchPosition(new Vector3(0, 0.3f, 0), 0.2f, 10, 0.5f).OnComplete(() =>
                CCommand.CommandExecutionComplete());
        }
        else
        {
            _unit.healthDisplay.transform.DOPunchPosition(new Vector3(0, -0.3f, 0), 0.2f, 10, 0.5f).OnComplete(() =>
                CCommand.CommandExecutionComplete());
        }

        _unit.UpdateHealthDisplay(valueAfter);
    }

    public void PlayStartAbility()
    {
        float originalY = _startLocalPos.y;
        _unitSprite.DOLocalMoveY(originalY + 0.2f, 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
            CCommand.CommandExecutionComplete());
    }

    public void PlayEndAbility()
    {
        float originalY = _startLocalPos.y;
        _unitSprite.DOLocalMoveY(originalY, 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
            CCommand.CommandExecutionComplete());
    }
}
