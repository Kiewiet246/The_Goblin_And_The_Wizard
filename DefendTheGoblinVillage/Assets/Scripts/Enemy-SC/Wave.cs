using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[Serializable]
public class Wave
{
    public float spawnRatesBetweenParty;
    public int repeatThroughList = 1;
    public List<PartyBunch> parties;
}
