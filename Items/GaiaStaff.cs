using System.Collections.Generic;
using Terraria;
using Terraria.World.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BotanyPlus.Items
{
	public class GaiaStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Creates grass and moss on dirt and stone\n" +
				"Greatly increases alchemy plant collection when used to gather\n" +
				"Automatically replants seeds in clay pots and planter boxes\n" +
				"Use on planted saplings to instantly grow them into trees");
		}

		public override void SetDefaults()
		{
			item.damage = 14;
			item.melee = true;
			item.width = 24;
			item.height = 28;
			item.createTile = TileID.Grass;
			item.useTime = 1;
			item.useAnimation = 25;
			item.knockBack = 3f;
			item.value = 200000;
			item.useTurn = true;
			item.tileBoost = 2;
			item.rare = ItemRarityID.Lime;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StaffofRegrowth);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
			recipe.AddIngredient(ItemID.Vine, 5);
			recipe.AddIngredient(ItemID.LifeFruit, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override bool UseItem(Player player)
        {
			Tile targetTile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
			bool isBloomingPlant = targetTile.type == TileID.BloomingHerbs;
			int targetStyle = targetTile.frameX / 18;
			if (targetTile.type == TileID.MatureHerbs)
			{
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
			}
			if (isBloomingPlant &&
				player.position.X / 16f - (float)Player.tileRangeX - (float)player.inventory[player.selectedItem].tileBoost - (float)player.blockRange <= (float)Player.tileTargetX && 
				(player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)player.inventory[player.selectedItem].tileBoost - 1f + (float)player.blockRange >= (float)Player.tileTargetX &&
				player.position.Y / 16f - (float)Player.tileRangeY - (float)player.inventory[player.selectedItem].tileBoost - (float)player.blockRange <= (float)Player.tileTargetY && 
				(player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)player.inventory[player.selectedItem].tileBoost - 2f + (float)player.blockRange >= (float)Player.tileTargetY)
			{
				Tile baseTile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY + 1);
				WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY);
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					int plantDrop;
					int seedDrop;
					if (targetStyle != 6)
					{
						plantDrop = 313 + targetStyle;
						seedDrop = 307 + targetStyle;
					}
					else
					{
						plantDrop = 2358;
						seedDrop = 2357;
					}
					int item = Item.NewItem(Player.tileTargetX * 16, Player.tileTargetY * 16, 16, 16, plantDrop, WorldGen.genRand.Next(1, 6));
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
					item = Item.NewItem(Player.tileTargetX * 16, Player.tileTargetY * 16, 16, 16, seedDrop, WorldGen.genRand.Next(1, 6));
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
				}
				if (baseTile.type == TileID.ClayPot || baseTile.type == TileID.PlanterBox)
                {
					WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, 82, false, false, -1, targetStyle);
					WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY);
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);
				}
			}
			if (targetTile.type == TileID.Saplings)
			{
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					ModPacket growCoord = mod.GetPacket();
					growCoord.Write(Player.tileTargetX);
					growCoord.Write(Player.tileTargetY);
					growCoord.Send();
				}
				else if ((Main.tile[Player.tileTargetX, Player.tileTargetY].frameX < 324 || Main.tile[Player.tileTargetX, Player.tileTargetY].frameX >= 540)
					? WorldGen.GrowTree(Player.tileTargetX, Player.tileTargetY) : WorldGen.GrowPalmTree(Player.tileTargetX, Player.tileTargetY))
					WorldGen.TreeGrowFXCheck(Player.tileTargetX, Player.tileTargetY);

			}
			if (targetTile.type == TileID.Stone)
            {
				int num50 = 0;
				int num51 = 0;
				Point point = player.Center.ToTileCoordinates();
				Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
				WorldUtils.Gen(new Point(point.X - 25, point.Y - 25), new Shapes.Rectangle(50, 50), new Actions.TileScanner(182, 180, 179, 183, 181, 381).Output(dictionary));
				foreach (KeyValuePair<ushort, int> item in dictionary)
				{
					if (item.Value > num51)
					{
						num51 = item.Value;
						num50 = item.Key;
					}
				}
				if (num51 == 0)
				{
					num50 = Utils.SelectRandom<int>(Main.rand, 182, 180, 179, 183, 181);
				}
				if (num50 != 0)
				{
					Main.tile[Player.tileTargetX, Player.tileTargetY].type = (ushort)num50;
					WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY);
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);
				}
			}
			return true;
        }
    }

	public class GaiaStaffGlobal : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModContent.ItemType<GaiaStaff>())
            {
				tooltips.RemoveAt(tooltips.FindIndex(x => x.Name.Contains("Placeable")));
			}
        }
    }
}
