using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Events;
using TileEngine.Logging;
using TileEngine.Maps;

namespace TileEngine.Entities
{
    public class EnemyManager
    {
        private Engine engine;
        private List<EnemyGroup> enemyGroups;
        private List<Entity> enemyTemplates;

        public EnemyManager(Engine engine)
        {
            this.engine = engine;
            enemyGroups = new List<EnemyGroup>();
            enemyTemplates = new List<Entity>();
        }

        public bool Initialized
        {
            get { return enemyTemplates.Count > 0; }
        }

        public void AddEnemyGroup(EnemyGroup enemyGroup)
        {
            enemyGroups.Add(enemyGroup);
        }

        public void Clear()
        {
            enemyGroups.Clear();
        }

        public void ClearAll()
        {
            enemyGroups.Clear();
            enemyTemplates.Clear();
        }

        public void SpwanEnemies()
        {
            foreach (var eg in enemyGroups)
            {
                SpwanEnemies(eg);
            }
        }

        public void SpawnMapSpawn(MapSpawn spawn)
        {
            EnemyGroup eg = new EnemyGroup(spawn.Type);
            eg.MapSpawn = true;
            eg.Category = spawn.Type;
            eg.PosX = spawn.MapX;
            eg.PosY = spawn.MapY;
            eg.Width = 1;
            eg.Height = 1;
            eg.MinNumber = 1;
            eg.MaxNumber = 1;
            eg.MinLevel = 0;
            eg.MaxLevel = 0;
            //eg.Direction = spawn.Direction;
            //eg.Summoner = spawn.Summoner;
            SpwanEnemies(eg);
        }

        public void AddEnemyTemplates(IList<string> enemies)
        {
            foreach(var s in enemies)
            {
                Entity ent = engine.LoadEnemy(s);
                if (ent != null)
                {
                    AddEnemyTemplate(ent);
                }
            }
        }

        public void AddEnemyTemplate(Entity enemy)
        {
            Logger.Info("Enemy", $"Adding Enemy Template {enemy}");
            enemyTemplates.Add(enemy);
        }

        private void SpwanEnemies(EnemyGroup eg)
        {
            int num = Utils.RandBetween(eg.MinNumber, eg.MaxNumber);
            string eName = eg.Name + ".txt";
            for (int i = 0; i < num; i++)
            {
                Entity e = FindEnemyTemplate(eg);
                if (e != null)
                {
                    e = new Entity(e);
                }
                else
                {
                    e = engine.LoadEnemy(eName);
                }
                if (e != null)
                {
                    //e.MapSpawn = eg.MapSpawn;
                    //e.Summoner = eg.Summoner;
                    SetEnemyPosition(e, eg);
                    //e.WayPoints = eg.WayPoints;
                    if (eg.WanderRadius > 0)
                    {
                        //e.Wander = true;
                        //e.SetWanderArea(eg.WanderRadius);
                    }
                    engine.EntityManager.AddEntity(e);
                    //AnimationStance stance = e.SpawnStance;
                    int dir = eg.Direction;
                    if (dir < 0) dir = Utils.Rand() % 7;
                    if (e.InitAnimation(EntityStance.Spawning, dir))
                    {
                        //e.Behavior = defaultEnemyBehavior;
                        //enemies.Add(e);
                        Logger.Info("Enemy", $"{e.Name} (Level {e.Level}) spawned at {new FPoint(e.MapPosX, e.MapPosY)}");
                    }
                    else
                    {
                        Logger.Warn("Enemy", $"Could not load enemy animations {eg.Name}");
                    }
                }
                else
                {
                    Logger.Warn("Enemy", $"Could not load enemy {eg.Name}");
                }

            }
        }

        private Entity FindEnemyTemplate(EnemyGroup eg)
        {
            List<Entity> enemyCandidates = new List<Entity>();
            var catMatch = enemyTemplates.Where(e => e.Categories.Contains(eg.Category));
            foreach (var e in catMatch)
            {
                if ((e.Level >= eg.MinLevel && e.Level <= eg.MaxLevel) || (eg.MinLevel == 0 && eg.MaxLevel == 0))
                {
                    int addTimes = 0;
                    switch (e.Rarity)
                    {
                        case "common":
                            addTimes = 6;
                            break;
                        case "uncommon":
                            addTimes = 3;
                            break;
                        case "rare":
                            addTimes = 1;
                            break;
                    }
                    for (int j = 0; j < addTimes; j++)
                    {
                        enemyCandidates.Add(e);
                    }
                }
            }
            if (enemyCandidates.Count > 0)
            {
                return enemyCandidates[Utils.Rand() % enemyCandidates.Count];
            }
            return null;
        }

        private void SetEnemyPosition(Entity ent, EnemyGroup eg)
        {
            int x = -1;
            int y = -1;
            int count = 0;
            while (!engine.Collision.IsValidTile(x, y, MovementType.Normal, false) && count < 10)
            {
                if (count == 0)
                {
                    x = eg.PosX + Utils.RandBetween(0, eg.Width);
                    y = eg.PosY + Utils.RandBetween(0, eg.Height);
                }
                else
                {
                    x = eg.PosX + Utils.RandBetween(-1, eg.Width + 1);
                    y = eg.PosY + Utils.RandBetween(-1, eg.Height + 1);
                }
                count++;
            }
            ent.MapPosX = x + 0.5f;
            ent.MapPosY = y + 0.5f;
        }
    }
}
