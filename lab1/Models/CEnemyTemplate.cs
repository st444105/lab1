using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace lab1.Models
{
    public class CEnemyTemplate
    {
        [JsonInclude]
        public string Name { get; private set; }

        [JsonInclude]
        public string IconName { get; private set; }

        [JsonInclude]
        public int BaseLife { get; private set; }

        [JsonInclude]
        public double LifeModifier { get; private set; }

        [JsonInclude]
        public int BaseGold { get; private set; }

        [JsonInclude]
        public double GoldModifier { get; private set; }

        [JsonInclude]
        public double SpawnChance { get; private set; }

        public CEnemyTemplate(
            string name,
            string iconName,
            int baseLife,
            double lifeModifier,
            int baseGold,
            double goldModifier,
            double spawnChance)
        {
            Name = name;
            IconName = iconName;
            BaseLife = baseLife;
            LifeModifier = lifeModifier;
            BaseGold = baseGold;
            GoldModifier = goldModifier;
            SpawnChance = spawnChance;
        }
    }
}
