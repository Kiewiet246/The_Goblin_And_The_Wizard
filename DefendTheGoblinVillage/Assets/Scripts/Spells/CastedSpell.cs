using System;
using System.Collections.Generic;
using UnityEngine;

public class CastedSpell : MonoBehaviour
{
    [SerializeField]
    private SpellManager.SpellType spellType;
    public float stackDamage;
    public EnemyManager enemyManager;
    public SpellManager spellManager;
    public TileInfo spellTile;
    private bool CastOnce = false;
    [SerializeField] private ShowDamage showDamage;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
           SpellIsTriggered();
        }
    }

    private void SpellIsTriggered()
    {
        if (!CastOnce)
        {
            CastOnce = true;
            List<LeaderScript> leaderScripts = enemyManager.enemiesInField;

            foreach (LeaderScript leader in leaderScripts)
            {
                switch (spellType)
                {
                    case SpellManager.SpellType.Normal:
                        break;
                    case SpellManager.SpellType.Fire:
                        spellManager.LeadersGotBurnt(leader, stackDamage);
                        break;
                    case SpellManager.SpellType.Ice:
                        spellManager.LeadersGotIced(leader, stackDamage);
                        break;
                    case SpellManager.SpellType.Poison:
                        spellManager.LeadersGotPoisoned(leader, stackDamage);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            spellTile.spellOnTile = SpellManager.SpellType.Normal;
            Destroy(gameObject);
        }
    }
}
