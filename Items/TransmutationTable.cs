using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BotanyPlus.Items
{
	public class TransmutationTable : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Transmutation Table");
			Tooltip.SetDefault("66% chance not to consume potion crafting ingredients\n" +
				"Functions as a water source");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 150000;
			item.rare = ItemRarityID.Pink;
			item.createTile = TileType<Tiles.TransmutationTable>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AlchemyTable);
			recipe.AddIngredient(ItemID.PhilosophersStone);
			recipe.AddIngredient(ItemID.HallowedBar, 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}