using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Models
{
    public class EnemyIcon
    {
        public string Name { get; set; }

        public string ImagePath { get; set; }

        public EnemyIcon(string name, string imagePath)
        {
            Name = name;
            ImagePath = imagePath;
        }
    }
}
