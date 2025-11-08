using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[Serializable]
public class Wave
{
    public List<PartyBunch> parties;
    public float spawnRatesBetweenParty;
    public int repeatThroughList;
}
