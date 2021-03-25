using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace BotanyPlus.Tiles
{
	public class GrowthLantern : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileWaterDeath[Type] = true;
			drop = mod.ItemType("GrowthLantern");
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Growth Lantern");
			AddMapEntry(new Color(31, 31, 191), name);
			disableSmartCursor = true;
		}

		private readonly int animationFrameWidth = 18;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			r = 0.1f;
			g = 0.3f;
			b = 0.8f;
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
    }
}