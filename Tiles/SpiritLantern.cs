using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System.Collections.Generic;
using System;

namespace BotanyPlus.Tiles
{
	public class SpiritLantern : ModTile
	{

		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileWaterDeath[Type] = true;
			drop = mod.ItemType("SpiritLantern");
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Spirit Lantern");
			AddMapEntry(new Color(192, 31, 63), name);
			disableSmartCursor = true;
		}

		private readonly int animationFrameWidth = 18;

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.6f;
			g = 0.1f;
			b = 0.2f;
		}

		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			int uniqueAnimationFrame = Main.tileFrame[Type] + i;
			if (i % 2 == 0)
			{
				uniqueAnimationFrame += 3;
			}
			if (i % 3 == 0)
			{
				uniqueAnimationFrame += 3;
			}
			if (i % 4 == 0)
			{
				uniqueAnimationFrame += 3;
			}
			uniqueAnimationFrame %= 4;

			frameXOffset = uniqueAnimationFrame * animationFrameWidth;
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if (++frameCounter >= 9)
			{
				frameCounter = 0;
				frame = ++frame % 4;
			}
		}

        public override void NearbyEffects(int i, int j, bool closer)
		{
			int growthRange = 12; //range at which Growth Lanterns take effect
			for (int xTile = i - growthRange; xTile <= i + growthRange; xTile++)
			{
				for (int yTile = j - growthRange; yTile <= j + growthRange; yTile++)
				{
					if (Framing.GetTileSafely(xTile, yTile).type == TileID.MatureHerbs)
					{
						Main.tile[xTile, yTile].type = TileID.BloomingHerbs;
						WorldGen.SquareTileFrame(xTile, yTile);
						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendTileSquare(-1, xTile, yTile, 1);
					}
				}
			}
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!fail)
			{
				int growthRange = 12; //range at which Growth Lanterns take effect
				for (int xTile = i - growthRange; xTile <= i + growthRange; xTile++)
				{
					for (int yTile = j - growthRange; yTile <= j + growthRange; yTile++)
					{
						Tile checkTile = Framing.GetTileSafely(xTile, yTile);
						if (checkTile.type == TileID.BloomingHerbs)
						{
							switch (checkTile.frameX / 18)
							{
								/*case 0: //daybloom
									if (!Main.dayTime)
									{
										Main.tile[xTile, yTile].type = TileID.MatureHerbs;
									}
									break;*/
								/*case 1: //moonglow
									if (Main.dayTime)
									{
										Main.tile[xTile, yTile].type = TileID.MatureHerbs;
									}
									break;*/
								case 2: //blinkroot - random chance
									if (WorldGen.genRand.Next(2) == 0)
									{
										Main.tile[xTile, yTile].type = TileID.MatureHerbs;
									}
									break;
								/*case 3: //deathweed
									if (Main.dayTime || (!Main.bloodMoon && Main.moonPhase != 0))
									{
										Main.tile[xTile, yTile].type = TileID.MatureHerbs;
									}
									break;*/
								/*case 4: //waterleaf
									if (!Main.raining && Main.cloudAlpha <= 0f)
									{
										Main.tile[xTile, yTile].type = TileID.MatureHerbs;
									}
									break;*/
								/*case 5: //fireblossom
									if (Main.raining || !Main.dayTime || Main.time <= 40500.0)
									{
										Main.tile[xTile, yTile].type = TileID.MatureHerbs;
									}
									break;*/
								case 6: //shiverthorn
									break;
								default:
									Main.tile[xTile, yTile].type = TileID.MatureHerbs;
									break;
							}
							WorldGen.TileFrame(xTile, yTile);
							if (Main.netMode != NetmodeID.SinglePlayer)
                                NetMessage.SendTileSquare(-1, xTile, yTile, 1);
						}
					}
				}
			}
		}
	}
}