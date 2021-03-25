using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BotanyPlus.Items
{
	public class GrowthLantern : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Moderately enhances growth rate of nearby herbs");
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
			item.value = 7000;
			item.rare = ItemRarityID.Blue;
			item.createTile = TileType<Tiles.GrowthLantern>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 1);
			recipe.AddIngredient(ItemID.WaterCandle, 1);
			recipe.AddIngredient(ItemID.GrassSeeds, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}