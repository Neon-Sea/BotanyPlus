using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria.Localization;

namespace BotanyPlus
{
    public class BotanyWorld : ModWorld
    {

        public override void PostDrawTiles()
        {
            //draw range preview square if holding a lantern in placement range
            int itemType = Main.player[Main.myPlayer].HeldItem.type;
            if (Main.placementPreview && Main.player[Main.myPlayer].showItemIcon
                && (itemType == ModContent.ItemType<Items.GrowthLantern>() || itemType == ModContent.ItemType<Items.SpiritLantern>()))
            {
                Vector2 coords = new Vector2((int)Main.MouseWorld.X - (int)Main.MouseWorld.X % 16 - 192, (int)Main.MouseWorld.Y - (int)Main.MouseWorld.Y % 16 - 192);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                Main.spriteBatch.Draw(Main.blackTileTexture, coords - Main.screenPosition, null, new Color(255, 255, 255, 63) * (Main.mouseTextColor / 255f), 0f, Vector2.Zero, 25f, SpriteEffects.None, 0f);
                Main.spriteBatch.End();
            }
        }
    }
}