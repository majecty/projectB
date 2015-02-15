using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Battle.CoreData
{
    public class ActiveSkill
    {
        public int cost;
    }

    public class PassiveSkill
    {

    }
    public class Critical : PassiveSkill
    {
        public static readonly float damageMultiplier = 2.0f;
        public readonly float mProbability = 0.5f;

        public static float Apply(IEnumerable<Card> deck, float beforeDamage)
        {
            var skills = from card in deck
                select card.passiveSkill;

            var criticals = from skill in skills
                where skill is Critical
                select skill as Critical;

            var criticalRandomResults = from critical in criticals
                let randomValue = Random.Range(0.0f, 1.0f)
                where randomValue < critical.mProbability
                select true;
            
            var isCriticalSuccess = criticalRandomResults.Any();

            if (isCriticalSuccess)
            {
                var afterDamage = beforeDamage * damageMultiplier;
                Debug.Log("Critical applied " + beforeDamage + " -> " + afterDamage);
                return afterDamage;
            }
            else
            {
                Debug.Log("Critical is not applied");
                return beforeDamage;
            }
        }
    }
}
