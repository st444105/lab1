using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace lab1.Models
{
    public class CEnemyTemplateList
    {
        private List<CEnemyTemplate> enemies;

        public CEnemyTemplateList()
        {
            enemies = new List<CEnemyTemplate>();
        }

        public void AddEnemy(
            string name,
            string iconName,
            int baseLife,
            double lifeModifier,
            int baseGold,
            double goldModifier,
            double spawnChance)
        {
            CEnemyTemplate enemy = new CEnemyTemplate(
                name,
                iconName,
                baseLife,
                lifeModifier,
                baseGold,
                goldModifier,
                spawnChance);

            enemies.Add(enemy);
        }

        public CEnemyTemplate GetEnemyByName(string name)
        {
            foreach (CEnemyTemplate enemy in enemies)
            {
                if (enemy.Name == name)
                {
                    return enemy;
                }
            }

            return null;
        }

        public CEnemyTemplate GetEnemyByIndex(int id)
        {
            return enemies[id];
        }

        public void DeleteEnemyByName(string name)
        {
            CEnemyTemplate enemy = GetEnemyByName(name);

            if (enemy != null)
            {
                enemies.Remove(enemy);
            }
        }

        public void DeleteEnemyByIndex(int id)
        {
            if (id >= 0 && id < enemies.Count)
            {
                enemies.RemoveAt(id);
            }
        }

        public List<string> GetListOfEnemyNames()
        {
            List<string> names = new List<string>();

            foreach (CEnemyTemplate enemy in enemies)
            {
                names.Add(enemy.Name);
            }

            return names;
        }

        public void SaveToJson(string path)
        {
            JsonSerializerOptions options =
                new JsonSerializerOptions();

            options.WriteIndented = true;

            string json = JsonSerializer.Serialize(
                enemies,
                options);

            File.WriteAllText(path, json);
        }

        public void LoadFromJson(string path)
        {
            string json = File.ReadAllText(path);

            JsonDocument doc = JsonDocument.Parse(json);

            enemies.Clear();

            foreach (JsonElement element in doc.RootElement.EnumerateArray())
            {
                string name =
                    element.GetProperty("Name").GetString() ?? "";

                string iconName =
                    element.GetProperty("IconName").GetString() ?? "";

                int baseLife =
                    element.GetProperty("BaseLife").GetInt32();

                double lifeModifier =
                    element.GetProperty("LifeModifier").GetDouble();

                int baseGold =
                    element.GetProperty("BaseGold").GetInt32();

                double goldModifier =
                    element.GetProperty("GoldModifier").GetDouble();

                double spawnChance =
                    element.GetProperty("SpawnChance").GetDouble();

                AddEnemy(
                    name,
                    iconName,
                    baseLife,
                    lifeModifier,
                    baseGold,
                    goldModifier,
                    spawnChance);
            }
        }

        public void EditEnemyByIndex(
            int id,
            string name,
            string iconName,
            int baseLife,
            double lifeModifier,
            int baseGold,
            double goldModifier,
            double spawnChance)
        {
            if (id < 0 || id >= enemies.Count)
            {
                return;
            }

            CEnemyTemplate enemy = new CEnemyTemplate(
                name,
                iconName,
                baseLife,
                lifeModifier,
                baseGold,
                goldModifier,
                spawnChance);

            enemies[id] = enemy;
        }
    }
}
