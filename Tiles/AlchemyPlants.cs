using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BotanyPlus
{
    public class AlchemyPlants : GlobalTile
    {
        public override void RandomUpdate(int i, int j, int type)
        {
            if (type == TileID.ImmatureHerbs || type == TileID.MatureHerbs)
            {
                int growthRange = 12; //range at which Growth Stations take effect
                int growthRate = 10 * ((Main.tile[i, j].frameX / 18 == 6) ? 2 : 1); //growth rate multiplier - doubled for shiverthorn, may adjust
                bool station1flag = false;
                for (int xTile = i - growthRange; xTile <= i + growthRange; xTile++)
                {
                    for (int yTile = j - growthRange; yTile <= j + growthRange; yTile++)
                    {
                        station1flag = Framing.GetTileSafely(xTile, yTile).type == ModContent.TileType<Tiles.GrowthLantern>();
                        if (Framing.GetTileSafely(xTile, yTile).type == ModContent.TileType<Tiles.SpiritLantern>())
                        {
                            for (int k = 1; k < growthRate * 3; k++)
                            {
                                WorldGen.GrowAlch(i, j);
                                if (Main.tile[i, j].type != type)
                                    break;
                            }
                            if (Main.netMode != NetmodeID.SinglePlayer && Main.tile[i, j].type != type)
                                NetMessage.SendTileSquare(-1, i, j, 1);
                            return;
                        }
                    }
                }
                if (station1flag)
                {
                    for (int k = 1; k < growthRate; k++)
                    {
                        WorldGen.GrowAlch(i, j);
                        if (Main.tile[i, j].type != type)
                            break;
                    }
                    if (Main.netMode != NetmodeID.SinglePlayer && Main.tile[i, j].type != type)
                        NetMessage.SendTileSquare(-1, i, j, 1);
                }
            }
        }

        public override bool AutoSelect(int i, int j, int type, Item item)
        {
            Player player = Main.player[Main.myPlayer];

            if (type == 83 || type == 84)
            {
                int targetStyle = Framing.GetTileSafely(i, j).frameX / 18;
                bool isBloomingPlant = type == 84;
                switch (targetStyle)
                {
                    case 0:
                        {
                            if (Main.dayTime)
                                isBloomingPlant = true;
                            break;
                        }
                    case 1:
                        {
                            if (!Main.dayTime)
                                isBloomingPlant = true;
                            break;
                        }
                    case 3:
                        {
                            if (!Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0))
                                isBloomingPlant = true;
                            break;
                        }
                    case 4:
                        {
                            if (Main.raining || Main.cloudAlpha > 0f)
                                isBloomingPlant = true;
                            break;
                        }
                    case 5:
                        {
                            if (!Main.raining && Main.dayTime && Main.time > 40500.00)
                                isBloomingPlant = true;
                            break;
                        }
                    default:
                        break;
                }
                if (isBloomingPlant &&
                player.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX &&
                (player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX &&
                player.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY &&
                (player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY)
                {
                    if (item.type == ItemID.StaffofRegrowth || item.type == ModContent.ItemType<Items.GaiaStaff>())
                    {
                        return true;
                    }
                    else if (!player.HasItem(ItemID.StaffofRegrowth) && !player.HasItem(ModContent.ItemType<Items.GaiaStaff>()) && item.pick > 0) // prioritize staff over pick for plants
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public override bool Drop(int i, int j, int type) //only works in single player due to auto planting - see GaiaStaff for item drop code in multiplayer
        {
            if (type == 83 || type == 84)
            {
                int targetStyle = Framing.GetTileSafely(i, j).frameX / 18;
                bool isBloomingPlant = type == 84;
                switch (targetStyle)
                {
                    case 0:
                        {
                            if (Main.dayTime)
                                isBloomingPlant = true;
                            break;
                        }
                    case 1:
                        {
                            if (!Main.dayTime)
                                isBloomingPlant = true;
                            break;
                        }
                    case 3:
                        {
                            if (!Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0))
                                isBloomingPlant = true;
                            break;
                        }
                    case 4:
                        {
                            if (Main.raining || Main.cloudAlpha > 0f)
                                isBloomingPlant = true;
                            break;
                        }
                    case 5:
                        {
                            if (!Main.raining && Main.dayTime && Main.time > 40500.00)
                                isBloomingPlant = true;
                            break;
                        }
                    default:
                        break;
                }
                if (isBloomingPlant && Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].HeldItem.type == ModContent.ItemType<Items.GaiaStaff>())
                {
                    int plantDrop;
                    if (targetStyle != 6)
                    {
                        plantDrop = 313 + targetStyle;
                    }
                    else
                    {
                        plantDrop = 2358;
                    }
                    Item.NewItem(i * 16, j * 16, 16, 16, plantDrop, WorldGen.genRand.Next(1, 6));
                    return false;
                }
                else return true;
            }
            else return true;
        }
    }
}