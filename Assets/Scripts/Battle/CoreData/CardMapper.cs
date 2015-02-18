using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battle.CoreData;
using LitJson;
using LitJson.Extensions;
using UnityEngine;


namespace Battle.CoreData
{
  
	#if UNITY_EDITOR
	[UnityEditor.InitializeOnLoad]
	#endif
    static class CardMapper
    {
        const IDictionary<string, Func<JsonData, ActiveSkill>> ActiveCategoryTable
            = new Dictionary<string, Func<JsonData, ActiveSkill>>
            {
                {"Attack", AttackImporter},
                {"Defense", DefenseImporter},
                {"Recover", RecoverImporter},
                {"Buff", BuffImporter},
            };
        const IDictionary<string, Func<JsonData, PassiveSkill>> PassiveCategoryTable
            = new Dictionary<string, Func<JsonData, PassiveSkill>>
            {
                {"Critical", CriticalImporter},
            };

        static CardMapper()
        {
            Register();
        }

        public static void Register()
        {
            JsonMapper.RegisterImporter<JsonData, Card>(CardImporter);
        }

        public static Card CardImporter(JsonData data)
        {
            Card card = new Card();
            card.name = (string)data["name"];
            card.activeSkill = ActiveSkillImporter(data["active_skill"]);
            card.passiveSkill = PassiveSkillImporter(data["passive_skill"]);
            return card;
        }

        public static ActiveSkill ActiveSkillImporter(JsonData data)
        {
            var category = (string)data["category"];
            var importer = ActiveCategoryTable[category];
            return importer(data);
        }

        public static PassiveSkill PassiveSkillImporter(JsonData data)
        {
            var category = (string)data["category"];
            var importer = PassiveCategoryTable[category];
            return importer(data);
        }

        public static Attack AttackImporter(JsonData data)
        {
            Attack attack = new Attack();
            attack.Cost = (int)data["cost"];
            attack.Power = (float)data["power"];
            attack.Accuracy = (float)data["accuracy"];
            attack.Type = (string)data["type"];
            attack.Condition = (string)data["condition"];
            attack.Drain = (float)data["drain"];
            attack.Delay = (int)data["delay"];
            return attack;
        }

        public static Defense DefenseImporter(JsonData data)
        {
            Defense defense = new Defense();
            defense.Cost = (int)data["cost"];
            defense.Ratio = (float)data["ratio"];
            defense.Amount = (float)data["amount"];
            return defense;
        }

        public static Recover RecoverImporter(JsonData data)
        {
            Recover recover = new Recover();
            recover.Cost = (int)data["cost"];
            recover.Amount = (float)data["amount"];
            recover.Duration = (float)data["duration"];
            recover.Type = (string)data["type"];
            return recover;
        }

        public static Buff BuffImporter(JsonData data)
        {
            Buff buff = new Buff();
            buff.Cost = (int)data["cost"];
            buff.Type = (string)data["type"];
            return buff;
        }

        public static Critical CriticalImporter(JsonData data)
        {
            Critical critical = new Critical();
            critical.Multiplier = (float)data["multiplier"];
            critical.Probability = (float)data["probability"];
            return critical;
        }
    }
}
