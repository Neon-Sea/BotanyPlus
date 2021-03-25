﻿using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace BotanyPlus
{
    public class BotanyPlayer : ModPlayer
    {
        public bool tmTable;

		public override void PostUpdate()
		{
			tmTable = false;
			int num = 4;
			int num2 = 3;
			int num3 = (int)((player.position.X + (float)(player.width / 2)) / 16f);
			int num4 = (int)((player.position.Y + (float)player.height) / 16f);
			for (int j = num3 - num; j <= num3 + num; j++)
			{
				for (int k = num4 - num2; k < num4 + num2; k++)
				{
					if (Framing.GetTileSafely(j, k).type == ModContent.TileType<Tiles.TransmutationTable>())
                    {
						tmTable = true;
						player.alchemyTable = false;
                    }
				}
			}
		}
        public override bool PreItemCheck()
        {
			if (player.HeldItem.type == ItemID.StaffofRegrowth || player.HeldItem.type == ModContent.ItemType<Items.GaiaStaff>())
				try
                {
					Cursor();
				}
				catch
                {
					Main.SmartCursorEnabled = false;
                }
					
			return true;
        }

		public void Cursor()
		{
			if (player.whoAmI != Main.myPlayer) return;
			Main.SmartCursorShowing = false;
			if (!Main.SmartCursorEnabled) return;
			Item item = player.inventory[player.selectedItem];
			Vector2 mouse = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			if (player.gravDir == -1f)
			{
				mouse.Y = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
			}
			int targetX = (int)MathHelper.Clamp(Player.tileTargetX, 10, Main.maxTilesX - 10);
			int targetY = (int)MathHelper.Clamp(Player.tileTargetY, 10, Main.maxTilesY - 10);
			bool disableCursor = false;
			if (Main.tile[targetX, targetY] == null) return;
			if (Main.tile[targetX, targetY].active())
			{
				switch (Main.tile[targetX, targetY].type)
				{
					case 4:
					case 10:
					case 11:
					case 13:
					case 21:
					case 29:
					case 33:
					case 49:
					case 50:
					case 55:
					case 79:
					case 85:
					case 88:
					case 97:
					case 104:
					case 125:
					case 132:
					case 136:
					case 139:
					case 144:
					case 174:
					case 207:
					case 209:
					case 212:
					case 216:
					case 219:
					case 237:
					case 287:
					case 334:
					case 335:
					case 338:
					case 354:
					case 386:
					case 387:
					case 388:
					case 389:
					case 411:
					case 425:
					case 441:
					case 463:
					case 467:
					case 468:
						disableCursor = true;
						break;
					case 314:
						if (player.gravDir == 1f) disableCursor = true;
						break;
				}
			}
			TileLoader.DisableSmartCursor(Main.tile[targetX, targetY], ref disableCursor);
			int tileBoost = item.tileBoost;
			int maxLeft    = (int)(player.position.X / 16f) - Player.tileRangeX - tileBoost + 1;
			int maxRight   = (int)((player.position.X + player.width) / 16f) + Player.tileRangeX + tileBoost - 1;
			int maxUp      = (int)(player.position.Y / 16f) - Player.tileRangeY - tileBoost + 1;
			int maxDown    = (int)((player.position.Y + player.height) / 16f) + Player.tileRangeY + tileBoost - 2;
			maxLeft  = Utils.Clamp(maxLeft, 10, Main.maxTilesX - 10);
			maxRight = Utils.Clamp(maxRight, 10, Main.maxTilesX - 10);
			maxDown  = Utils.Clamp(maxDown, 10, Main.maxTilesY - 10);
			maxUp    = Utils.Clamp(maxUp, 10, Main.maxTilesY - 10);
			if (disableCursor && targetX >= maxLeft && targetX <= maxRight && targetY <= maxDown && targetY >= maxUp) return;
			List<Tuple<int, int>> listTargets = new List<Tuple<int, int>>();
			for (int xCheck = maxLeft; xCheck <= maxRight; xCheck++)
			{
				for (int yCheck = maxUp; yCheck <= maxDown; yCheck++)
				{
					Tile checkTile = Main.tile[xCheck, yCheck];
					if (checkTile.type == 84)
					{
						listTargets.Add(new Tuple<int, int>(xCheck, yCheck));
					}
				}
			}
			if (listTargets.Count > 0)
			{
				float num130 = -1f;
				Tuple<int, int> tupleCoords = listTargets[0];
				for (int num131 = 0; num131 < listTargets.Count; num131++)
				{
					float num132 = Vector2.Distance(new Vector2(listTargets[num131].Item1, listTargets[num131].Item2) * 16f + Vector2.One * 8f, mouse);
					if (num130 == -1f || num132 < num130)
					{
						num130 = num132;
						tupleCoords = listTargets[num131];
					}
				}
				if (Collision.InTileBounds(tupleCoords.Item1, tupleCoords.Item2, maxLeft, maxUp, maxRight, maxDown))
				{
					Main.SmartCursorX = (Player.tileTargetX = tupleCoords.Item1);
					Main.SmartCursorY = (Player.tileTargetY = tupleCoords.Item2);
					Main.SmartCursorShowing = true;
				}
				listTargets.Clear();
			}
		}
    }
}