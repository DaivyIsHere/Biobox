using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitAnimation : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Transform animateTarget;

    [Header("Values")]
    [SerializeField] private Vector3 _attackVector = new Vector3(0.5f, 0, 0);
    [SerializeField] private float _attackDuration = 0.5f;
    [SerializeField] private int _attackVibrato = 6;
    [SerializeField] private float _attackElast = 0.2f;

    public void PlayAttack(Unit unit)
    {
        Vector3 punchVector = _attackVector;
        if (unit.boxSide == BoxSide.RightSide)
            punchVector *= -1;

        animateTarget.DOComplete();
        animateTarget.DOPunchPosition(punchVector, _attackDuration, _attackVibrato, _attackElast);
        animateTarget.DOPunchScale(new Vector3(0.3f, -0.2f, 0), _attackDuration, _attackVibrato, _attackElast);
    }

    public void PlayHit(Unit unit)
    {
        animateTarget.DOComplete();
        animateTarget.DOPunchScale(new Vector3(-0.3f, 0.4f, 0), _attackDuration, _attackVibrato, _attackElast);
    }
}
