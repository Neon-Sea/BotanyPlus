using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using System.Reflection;

namespace BotanyPlus.Tiles
{
	public class TransmutationTable : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Transmutation Table");
			AddMapEntry(new Color(200, 200, 200), name);
			animationFrameHeight = 54;
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.AlchemyTable, TileID.Sinks, TileID.Bottles };
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frame = Main.tileFrame[TileID.AlchemyTable];
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 48, 48, ItemType<Items.TransmutationTable>());
		}
        public override bool Autoload(ref string name, ref string texture)
        {
			IL.Terraria.Recipe.Create += HookCreate;
			return base.Autoload(ref name, ref texture);
        }

		private void HookCreate(ILContext il)
        {
			//because there is no GlobalRecipe.ConsumeItem() this IL patch adds my own for any bottle ("alchemy") recipes
			int itemIndex = 0;
			int stackIndex = 0;
			var c = new ILCursor(il);
			if (!c.TryGotoNext(i => i.MatchLdelemRef() && i.Next.MatchStloc(out itemIndex)))
            {
				return;
            }

			ILLabel aftercheck = c.DefineLabel();
			
			c.GotoNext(i => i.MatchLdfld(typeof(Item).GetField("stack")) && i.Next.MatchStloc(out stackIndex));
			c.Index += 2;
			c.Emit(OpCodes.Ldarg_0);
			c.Emit(OpCodes.Ldfld, typeof(Recipe).GetField("alchemy"));
			c.Emit(OpCodes.Brfalse_S, aftercheck);
			c.Emit(OpCodes.Ldloc, itemIndex);
			c.Emit(OpCodes.Ldfld, typeof(Item).GetField("stack"));
			c.Emit(OpCodes.Call, typeof(AlchemyRecipes).GetMethod("Consume"));
			c.Emit(OpCodes.Stloc, stackIndex);
			c.MarkLabel(aftercheck);
			c.GotoNext(i => i.MatchLdloc(itemIndex) && i.Next.MatchLdfld(typeof(Item).GetField("stack")));
			c.Remove();
			c.Remove();
			c.Emit(OpCodes.Ldloc, stackIndex);
			return;
		}
    }
}