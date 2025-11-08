using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PartyBunch
{
    [FormerlySerializedAs("singlePArty")] public List<EnemyContoller.EnemyType> singleParty;
    public int repeatAmountForParty;
    public float spawnRateForIndividuals;
}
