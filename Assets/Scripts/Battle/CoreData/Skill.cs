using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Battle.CoreData
{
    public class ActiveSkill
    {
        public int Cost { get; set; }
    }

    public class PassiveSkill
    {

    }

    public class Attack : ActiveSkill
    {
        public float Power { get; set; }
        public float Accuracy { get; set; }
        public string Type { get; set; }
        public string Condition { get; set; }
        public float Drain { get; set; }
        public int Delay { get; set; }
    }

    public class Defense : ActiveSkill
    {
        public float Ratio { get; set; }
        public float Amount { get; set; }
    }

    public class Recovery : ActiveSkill
    {
        public float Amount { get; set; }
        public float Duration { get; set; }
        public string Type { get; set; }
    }

    public class Buff : ActiveSkill
    {
        public string Type { get; set; }
    }

    public class Critical : PassiveSkill
    {
        public float Probability { get; set; }
        public float Multiplier { get; set; }

        public static float Apply(IEnumerable<Card> deck, float beforeDamage)
        {
            var skills = from card in deck
                select card.passiveSkill;

            var criticals = from skill in skills
                where skill is Critical
                select skill as Critical;

            var successCritical = from critical in criticals
                let randomValue = Random.Range(0.0f, 1.0f)
                where randomValue < critical.Probability
                select critical;
            
            var isCriticalSuccess = successCritical.Any();
            var criticalMultipliers = successCritical.Sum(critical => critical.Multiplier);
            if (isCriticalSuccess)
            {
                var afterDamage = beforeDamage * criticalMultipliers;
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
