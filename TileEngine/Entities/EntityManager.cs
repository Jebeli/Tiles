/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
*/

namespace TileEngine.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;
    using TileEngine.Graphics;
    using TileEngine.Logging;

    public class EntityManager
    {
        private Engine engine;
        private List<Entity> entities;
        private Entity selectedEnemy;

        public EntityManager(Engine engine)
        {
            this.engine = engine;
            entities = new List<Entity>();
        }

        public Entity SelectedEnemy
        {
            get { return selectedEnemy; }
            set
            {
                if (selectedEnemy != value)
                {
                    if (selectedEnemy != null)
                    {

                    }
                    selectedEnemy = value;
                    if (selectedEnemy != null)
                    {
                        Logger.Info("Entity", $"{selectedEnemy.Name} selected");
                    }
                }
            }
        }

        public void AddEntity(Entity entity)
        {
            entity.CreateVisual();
            engine.Collision.Block(entity.MapPosX, entity.MapPosY, false);
            entities.Add(entity);
            Logger.Info("Entity", $"{entity} added");
        }

        public void Clear()
        {
            entities.Clear();
        }

        public void Update(TimeInfo time)
        {
            foreach (var e in GetVisibleEntities())
            {
                e.Update(time);
            }
            HandleHoverSelection();
        }

        private void HandleHoverSelection()
        {
            SelectedEnemy = GetEnemyAt(engine.Input.ScaledMouseX, engine.Input.ScaledMouseY);
        }

        public void AddRenderables(IList<RenderTextureRegion> list, IList<RenderTextureRegion> listDead)
        {
            foreach (var e in GetVisibleEntities())
            {
                e.AddRenderables(e.Dead ? listDead : list);
            }
        }

        private IEnumerable<Entity> GetVisibleEntities(bool forcePlayer = true)
        {
            List<Entity> list = new List<Entity>();
            foreach (var e in entities)
            {
                if (e.Visible)
                {
                    list.Add(e);
                }
            }
            return list;
        }

        public Entity GetEnemyAt(float mx, float my)
        {
            return GetEntityAt(mx, my, GetVisibleLivingEnemies());
        }

        private IEnumerable<Entity> GetVisibleLivingEnemies()
        {
            List<Entity> list = new List<Entity>();
            foreach (var e in entities)
            {
                if (e.Visible && !e.Dead && e.Type == EntityType.Enemy)
                {
                    list.Add(e);
                }
            }
            return list;
        }

        private Entity GetEntityAt(float mx, float my, IEnumerable<Entity> list)
        {
            foreach (var e in list)
            {
                engine.Camera.MapToScreen(e.MapPosX, e.MapPosY, out int sX, out int sY);
                Rect sR = e.GetFrameRect(sX, sY);
                if (mx >= sR.X && my >= sR.Y && mx < sR.X + sR.Width && my < sR.Y + sR.Height)
                {
                    return e;
                }
            }
            return null;
        }
    }
}
