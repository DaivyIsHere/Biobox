using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDManager : Singleton<IDManager>
{
    private int _unitCIDCount;
    //private int _decoCIDCount;
    private Dictionary<int, Unit> _unitCIDHolders = new Dictionary<int, Unit>();

    public void RegisterNewUnitCID(Unit unit)
    {
        _unitCIDCount++;
        unit.unitCID.id = _unitCIDCount;
        _unitCIDHolders.Add(_unitCIDCount, unit);
    }

    public Unit GetUnitWithID(UnitCID unitCID)
    {
        Unit result = null;
        _unitCIDHolders.TryGetValue(unitCID.id, out result);
        return result;
    }

    public void RemoveUnitCID(UnitCID unitCID)
    {
        _unitCIDHolders.Remove(unitCID.id);
    }

    public void ResetIDs()
    {
        _unitCIDCount = 0;
    }
}
