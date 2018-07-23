using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Maps;

namespace TileEngine.Savers
{
    public class IniSaver : AbstractSaver
    {
        public IniSaver(Engine engine)
            : base(engine, ".txt")
        {

        }

        public override void Save(Map map, string fileId)
        {
            using (var stream = GetOutputStream(fileId))
            {
                using (TextWriter tw = new StreamWriter(stream))
                {
                    tw.WriteLine("[header]");
                    tw.WriteLine($"width={map.Width}");
                    tw.WriteLine($"height={map.Height}");
                    tw.WriteLine($"tilewidth={map.TileWidth}");
                    tw.WriteLine($"tileheight={map.TileHeight}");
                    tw.WriteLine($"orientation={map.Orientation}");
                    tw.WriteLine($"background_color={map.BackgroundColor.ToIniString()}");
                    tw.WriteLine($"hero_pos={map.StartX},{map.StartY}");
                    tw.WriteLine($"music={map.MusicName}");
                    tw.WriteLine($"tileset={map.TileSetName}");
                    if (map.Parallax != null)
                        tw.WriteLine($"parallax_layers={map.Parallax.Name}");
                    tw.WriteLine($"title={map.Name}");
                    tw.WriteLine();
                    foreach (var layer in map.Layers)
                    {
                        tw.WriteLine("[layer]");
                        tw.WriteLine($"type={layer.Name}");
                        tw.WriteLine("data=");
                        tw.WriteLine(layer.GetCSV());
                        tw.WriteLine();
                    }
                    foreach (var npc in map.LoadNPCs)
                    {
                        tw.WriteLine("[npc]");
                        tw.WriteLine("type=npc");
                        tw.WriteLine($"location={npc.PosX},{npc.PosY},1,1");
                        tw.WriteLine($"filename={npc.EntityId}");
                        tw.WriteLine();
                    }
                    foreach (var evt in map.LoadEvents)
                    {
                        tw.WriteLine("[event]");
                        tw.WriteLine("type=event");
                        tw.WriteLine($"location={evt.PosX},{evt.PosY},{evt.Width},{evt.Height}");
                        tw.WriteLine($"activate={evt.Type.ToIniString()}");
                        if (evt.HotSpot)
                        {
                            if (evt.HotSpotIsLocation)
                            {
                                tw.WriteLine("hotspot=location");
                            }
                            else
                            {
                                tw.WriteLine($"hotspot={evt.HotPosX},{evt.HotPosY},{evt.HotWidth},{evt.HotHeight}");
                            }
                        }
                        if (evt.Cooldown > 0)
                        {
                            tw.WriteLine($"cooldown={evt.Cooldown.ToDuration()}");
                        }
                        if (evt.Delay > 0)
                        {
                            tw.WriteLine($"delay={evt.Delay.ToDuration()}");
                        }
                        foreach (var ec in evt.Components)
                        {
                            tw.Write(ec.GetIniTypeString());
                            tw.Write("=");
                            tw.Write(ec.ToIniString());
                            tw.WriteLine();
                        }
                        tw.WriteLine();
                    }
                    foreach (var enemy in map.LoadEnemyGroups)
                    {
                        tw.WriteLine("[enemy]");
                        tw.WriteLine("type=enemy");
                        tw.WriteLine($"location={enemy.PosX},{enemy.PosY},{enemy.Width},{enemy.Height}");
                        tw.WriteLine($"category={enemy.Category}");
                        if (enemy.Chance < 100)
                            tw.WriteLine($"chance={enemy.Chance}");
                        tw.WriteLine($"level={enemy.MinLevel},{enemy.MaxLevel}");
                        tw.WriteLine($"number={enemy.MinNumber},{enemy.MaxNumber}");
                        if (enemy.Direction >= 0)
                            tw.WriteLine($"direction={enemy.Direction}");
                        if (enemy.WayPoints.Count > 0)
                        {
                            tw.Write("waypoints=");
                            for (int i = 0; i < enemy.WayPoints.Count; i++)
                            {
                                var wp = enemy.WayPoints[i];
                                tw.Write((int)wp.X);
                                tw.Write(",");
                                tw.Write((int)wp.Y);
                                if (i < enemy.WayPoints.Count - 1)
                                    tw.Write(",");
                            }
                            tw.WriteLine();
                        }
                        if (enemy.WanderRadius > 0)
                        {
                            tw.WriteLine($"wander_radius={enemy.WanderRadius}");
                        }
                        tw.WriteLine();
                    }
                }
            }
        }

        public override void Save(TileSet tileSet, string fileId)
        {
            using (var stream = GetOutputStream(fileId))
            {
                using (TextWriter tw = new StreamWriter(stream))
                {
                    string lastTexture = "";
                    tw.WriteLine();
                    foreach (var i in tileSet.Tiles)
                    {
                        var tile = tileSet.GetTile(i);
                        if (!lastTexture.Equals(tile.Texture.Name))
                        {
                            lastTexture = tile.Texture.Name;
                            tw.WriteLine($"img={lastTexture}");
                            tw.WriteLine();
                        }
                        tw.WriteLine($"tile={i},{tile.X},{tile.Y},{tile.Width},{tile.Height},{tile.OffsetX},{tile.OffsetY}");
                        if (tileSet.IsAnimTile(i))
                        {
                            var anim = tileSet.GetTileAnim(i);
                            tw.Write($"animation={i};");
                            for (int j = 0; j < anim.FrameCount; j++)
                            {
                                tw.Write(anim.Textures[j].X);
                                tw.Write(",");
                                tw.Write(anim.Textures[j].Y);
                                tw.Write(",");
                                tw.Write(anim.FrameDurations[j].ToDuration("ms", engine.MaxFramesPerSecond));
                                tw.Write(";");
                            }
                            tw.WriteLine();
                        }
                    }
                }
            }
        }

        public override void Save(MapParallax parallax, string fileId)
        {
            using (var stream = GetOutputStream(fileId))
            {
                using (TextWriter tw = new StreamWriter(stream))
                {
                    foreach (var layer in parallax.Layers)
                    {
                        tw.WriteLine("[layer]");
                        tw.WriteLine($"image={layer.Texture.Name}");
                        tw.WriteLine($"speed={layer.Speed}");
                        if (layer.FixedSpeedX != 0 || layer.FixedSpeedY != 0)
                            tw.WriteLine($"fixed_speed={layer.FixedSpeedX},{layer.FixedSpeedY}");
                        if (!string.IsNullOrEmpty(layer.MapLayer))
                            tw.WriteLine($"map_layer={layer.MapLayer}");
                        tw.WriteLine();
                    }
                }
            }
        }

        protected override string AdjustName(string fileId)
        {
            fileId = Path.GetFileName(fileId);
            return fileId.Replace(".xml", ".txt");

        }
    }
}
