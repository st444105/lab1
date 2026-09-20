using lab1.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace lab1
{
    public partial class MainWindow : Window
    {
        private CEnemyTemplateList enemyList;
        private List<EnemyIcon> enemyIcons;

        private int selectedEnemyIndex = -1;

        public MainWindow()
        {
            InitializeComponent();

            enemyList = new CEnemyTemplateList();
            enemyIcons = new List<EnemyIcon>();
        }

        private void LoadIconsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog =
                new System.Windows.Forms.FolderBrowserDialog();

            System.Windows.Forms.DialogResult result =
                dialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            LoadIconsFromFolder(dialog.SelectedPath);
        }


        private void LoadIconsFromFolder(string path)
        {
            enemyIcons.Clear();
            IconsListBox.Items.Clear();

            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                if (Path.GetExtension(file).Equals(
                    ".png",
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    continue;
                }

                EnemyIcon icon = new EnemyIcon(
                    Path.GetFileName(file),
                    file);

                enemyIcons.Add(icon);

                System.Windows.Controls.Image image =
                    new System.Windows.Controls.Image();

                BitmapImage bitmap = new BitmapImage();

                bitmap.BeginInit();

                bitmap.UriSource = new Uri(file);

                bitmap.DecodePixelWidth = 64;

                bitmap.CacheOption = BitmapCacheOption.OnLoad;

                bitmap.EndInit();

                image.Source = bitmap;

                image.Width = 64;
                image.Height = 64;
                image.Margin = new Thickness(5);

                IconsListBox.Items.Add(image);
            }
        }

        private void IconsListBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ListBox iconHolder =
                sender as System.Windows.Controls.ListBox;

            if (iconHolder == null)
            {
                return;
            }

            if (iconHolder.SelectedItem
                is System.Windows.Controls.Image selectedImage)
            {
                if (selectedImage.Source == null)
                {
                    return;
                }

                string iconName =
                    Path.GetFileName(
                        selectedImage.Source.ToString());

                IconNameTextBox.Text = iconName;

                MainEnemyIcon.Source =
                    selectedImage.Source;
            }
        }

        private void AddEnemyButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!ReadEnemyData(
                out string name,
                out string iconName,
                out int baseLife,
                out double lifeModifier,
                out int baseGold,
                out double goldModifier,
                out double spawnChance))
            {
                return;
            }

            if (name == "")
            {
                System.Windows.MessageBox.Show(
                    "Введите имя противника.");

                return;
            }

            CEnemyTemplate existing =
                enemyList.GetEnemyByName(name);

            if (existing != null)
            {
                System.Windows.MessageBox.Show(
                    "Противник с таким именем уже существует.");

                return;
            }

            enemyList.AddEnemy(
                name,
                iconName,
                baseLife,
                lifeModifier,
                baseGold,
                goldModifier,
                spawnChance);

            RefreshEnemyList();

            EnemyListBox.SelectedIndex =
                EnemyListBox.Items.Count - 1;
        }

        private void EditEnemyButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (selectedEnemyIndex < 0)
            {
                System.Windows.MessageBox.Show(
                    "Сначала выберите противника.");

                return;
            }

            if (!ReadEnemyData(
                out string name,
                out string iconName,
                out int baseLife,
                out double lifeModifier,
                out int baseGold,
                out double goldModifier,
                out double spawnChance))
            {
                return;
            }

            if (name == "")
            {
                System.Windows.MessageBox.Show(
                    "Введите имя противника.");

                return;
            }

            enemyList.EditEnemyByIndex(
                selectedEnemyIndex,
                name,
                iconName,
                baseLife,
                lifeModifier,
                baseGold,
                goldModifier,
                spawnChance);

            RefreshEnemyList();

            EnemyListBox.SelectedIndex = selectedEnemyIndex;

            ShowEnemyIcon(iconName);
        }

        private void DeleteEnemyButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (selectedEnemyIndex < 0)
            {
                System.Windows.MessageBox.Show(
                    "Сначала выберите противника.");

                return;
            }

            enemyList.DeleteEnemyByIndex(
                selectedEnemyIndex);

            RefreshEnemyList();

            selectedEnemyIndex = -1;

            ClearEnemyFields();
        }

        private void EnemyListBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            selectedEnemyIndex =
                EnemyListBox.SelectedIndex;

            if (selectedEnemyIndex < 0)
            {
                return;
            }

            CEnemyTemplate enemy =
                enemyList.GetEnemyByIndex(
                    selectedEnemyIndex);

            if (enemy == null)
            {
                return;
            }

            NameTextBox.Text = enemy.Name;

            IconNameTextBox.Text =
                enemy.IconName;

            BaseLifeTextBox.Text =
                enemy.BaseLife.ToString();

            LifeModifierTextBox.Text =
                enemy.LifeModifier.ToString();

            BaseGoldTextBox.Text =
                enemy.BaseGold.ToString();

            GoldModifierTextBox.Text =
                enemy.GoldModifier.ToString();

            SpawnChanceTextBox.Text =
                enemy.SpawnChance.ToString();

            ShowEnemyIcon(enemy.IconName);
        }

        private void ShowEnemyIcon(string iconName)
        {
            MainEnemyIcon.Source = null;

            foreach (EnemyIcon icon in enemyIcons)
            {
                if (icon.Name == iconName)
                {
                    MainEnemyIcon.Source =
                        new BitmapImage(
                            new Uri(icon.ImagePath));

                    return;
                }
            }
        }

        private void RefreshEnemyList()
        {
            EnemyListBox.Items.Clear();

            List<string> names =
                enemyList.GetListOfEnemyNames();

            foreach (string name in names)
            {
                EnemyListBox.Items.Add(name);
            }
        }

        private bool ReadEnemyData(
            out string name,
            out string iconName,
            out int baseLife,
            out double lifeModifier,
            out int baseGold,
            out double goldModifier,
            out double spawnChance)
        {
            name = NameTextBox.Text;
            iconName = IconNameTextBox.Text;

            baseLife = 0;
            lifeModifier = 0;
            baseGold = 0;
            goldModifier = 0;
            spawnChance = 0;

            if (!int.TryParse(
                BaseLifeTextBox.Text,
                out baseLife))
            {
                System.Windows.MessageBox.Show(
                    "Base Life должно быть целым числом.");

                return false;
            }

            if (!double.TryParse(
                LifeModifierTextBox.Text,
                out lifeModifier))
            {
                System.Windows.MessageBox.Show(
                    "Life Modifier должно быть числом.");

                return false;
            }

            if (!int.TryParse(
                BaseGoldTextBox.Text,
                out baseGold))
            {
                System.Windows.MessageBox.Show(
                    "Base Gold должно быть целым числом.");

                return false;
            }

            if (!double.TryParse(
                GoldModifierTextBox.Text,
                out goldModifier))
            {
                System.Windows.MessageBox.Show(
                    "Gold Modifier должно быть числом.");

                return false;
            }

            if (!double.TryParse(
                SpawnChanceTextBox.Text,
                out spawnChance))
            {
                System.Windows.MessageBox.Show(
                    "Spawn Chance должно быть числом.");

                return false;
            }

            return true;
        }

        private void ClearEnemyFields()
        {
            NameTextBox.Text = "";
            IconNameTextBox.Text = "";

            BaseLifeTextBox.Text = "100";
            LifeModifierTextBox.Text = "1";

            BaseGoldTextBox.Text = "10";
            GoldModifierTextBox.Text = "1";

            SpawnChanceTextBox.Text = "1";

            MainEnemyIcon.Source = null;
        }

        private void SaveJsonButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog =
                new Microsoft.Win32.SaveFileDialog();

            dialog.FileName = "enemies";
            dialog.DefaultExt = ".json";
            dialog.Filter =
                "JSON files (*.json)|*.json";

            bool? result =
                dialog.ShowDialog();

            if (result == true)
            {
                enemyList.SaveToJson(
                    dialog.FileName);

                System.Windows.MessageBox.Show(
                    "Список противников сохранён.");
            }
        }

        private void LoadJsonButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog =
                new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".json";
            dialog.Filter =
                "JSON files (*.json)|*.json";

            bool? result =
                dialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    enemyList.LoadFromJson(
                        dialog.FileName);

                    RefreshEnemyList();

                    ClearEnemyFields();

                    selectedEnemyIndex = -1;

                    System.Windows.MessageBox.Show(
                        "Список противников загружен.");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(
                        "Ошибка загрузки JSON:\n" +
                        ex.Message);
                }
            }
        }
    }
}