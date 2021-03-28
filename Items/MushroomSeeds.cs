using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace BotanyPlus.Items
{
    public class MushroomSeeds : ModItem
    {
        public override void SetDefaults()
        {
			item.width = 12;
			item.height = 14;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 80;
			item.createTile = TileType<Tiles.BotanyImmatureHerbs>();
			item.placeStyle = 1;
		}
    }
}