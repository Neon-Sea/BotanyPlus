using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BotanyPlus.Items
{
	public class SpiritLantern : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Greatly enhances growth rate of nearby herbs \nMature herbs in range will always bloom");
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
			item.value = 70000;
			item.rare = ItemRarityID.Yellow;
			item.createTile = TileType<Tiles.SpiritLantern>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GrowthLantern>(), 1);
			recipe.AddIngredient(ItemID.Ectoplasm, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}