using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;

namespace TileEngine.Entities
{
    public class EnemyGroup : NamedObject
    {
        private string category;
        private int posX;
        private int posY;
        private int width;
        private int height;
        private int minLevel;
        private int maxLevel;
        private int minNumber;
        private int maxNumber;
        private int direction;
        private bool mapSpawn;
        private Entity summoner;
        private int chance;
        private int wanderRadius;
        private List<FPoint> wayPoints;

        public EnemyGroup()
            : base("")
        {
            width = 1;
            height = 1;
            minLevel = 0;
            maxLevel = 0;
            minNumber = 1;
            maxNumber = 1;
            direction = -1;
            chance = 100;
            wayPoints = new List<FPoint>();
        }

        public EnemyGroup(string name)
            : base(name)
        {
            width = 1;
            height = 1;
            minLevel = 0;
            maxLevel = 0;
            minNumber = 1;
            maxNumber = 1;
            direction = -1;
            chance = 100;
            wayPoints = new List<FPoint>();
        }

        public bool MapSpawn
        {
            get { return mapSpawn; }
            set { mapSpawn = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public int PosX
        {
            get { return posX; }
            set { posX = value; }
        }

        public int PosY
        {
            get { return posY; }
            set { posY = value; }
        }

        public float CenterX
        {
            get { return posX + width / 2.0f; }
        }

        public float CenterY
        {
            get { return posY + height / 2.0f; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int MinLevel
        {
            get { return minLevel; }
            set { minLevel = value; }
        }

        public int MaxLevel
        {
            get { return maxLevel; }
            set { maxLevel = value; }
        }

        public int MinNumber
        {
            get { return minNumber; }
            set { minNumber = value; }
        }

        public int MaxNumber
        {
            get { return maxNumber; }
            set { maxNumber = value; }
        }

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Entity Summoner
        {
            get { return summoner; }
            set { summoner = value; }
        }

        public int Chance
        {
            get { return chance; }
            set { chance = value; }
        }

        public int WanderRadius
        {
            get { return wanderRadius; }
            set { wanderRadius = value; }
        }

        public IList<FPoint> WayPoints
        {
            get { return wayPoints; }
            set
            {
                wayPoints.Clear();
                if (value != null)
                {
                    wayPoints.AddRange(value);
                }
            }
        }
    }
}
