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
    [SerializeField] private Vector3 _valuePopupOffset;

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

    public void PlayTakeDamage(int damageValue, int healthAfter, bool killed)
    {
        //print(_unit.unitData.unitName + " "+ _unit.orderInBox + " play hit");
        ValuePopup valuePopup = Instantiate(_valuePopupPref, transform.position + _valuePopupOffset, Quaternion.identity);
        valuePopup.displayValue = damageValue;

        _unit.OnHealthChange(healthAfter);

        if (!killed)
        {
            ResetAllTweens();
            _sequence = DOTween.Sequence();//Start sequence
            _sequence.AppendInterval(0.075f);
            _sequence.Append(_unitSprite.DOPunchScale(new Vector3(-0.3f, 0.4f, 0), _attackDuration, _attackVibrato, _attackElast));
            _sequence.OnComplete(CCommand.CommandExecutionComplete);
        }
        else
        {
            CCommand.CommandExecutionComplete();
        }
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
        _sequence.AppendInterval(0f);
        _sequence.OnComplete(_unit.DestroySelf);
        ///We called CCommand.CommandExecutionComplete in _unit.DestroySelf

        //_unitSprite.DOKill();
        //_sequence.Kill();// Note : you cannot use DoKill to kill sequence, you need the direct reference
    }

    public void PlayExhaust()
    {
        _unitBG.DOColor(exhaustedColor,0.2f);
    }

    public void PlayUnexhaust()
    {
        _unitBG.DOColor(defaultColor,0.5f);
    }
}
