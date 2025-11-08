using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PartyBunch
{
    public int repeatAmountForParty = 1;
    public float spawnRateForIndividuals;
    public List<EnemyContoller.EnemyType> singleParty;
}
