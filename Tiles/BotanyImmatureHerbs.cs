using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;

namespace BotanyPlus
{
    public class BotanyImmatureHerbs : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileAlch[Type] = true;
            Main.tileNoFail[Type] = true;
            //mushroom
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
            TileObjectData.newTile.AnchorValidTiles = new int[2]
            {
                TileID.Grass,
                TileID.HallowedGrass
            };
            TileObjectData.newTile.AnchorAlternateTiles = new int[1]
            {
                TileID.ClayPot
            };
            //vile mushroom
            TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleAlch);
            TileObjectData.newSubTile.AnchorValidTiles = new int[1]
            {
                TileID.CorruptGrass
            };
            TileObjectData.newSubTile.AnchorAlternateTiles = new int[1]
            {
                TileID.ClayPot
            };
            TileObjectData.addSubTile(1);
            //glowing mushroom
            TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleAlch);
            TileObjectData.newSubTile.AnchorValidTiles = new int[1]
            {
                TileID.MushroomGrass
            };
            TileObjectData.newSubTile.AnchorAlternateTiles = new int[1]
            {
                TileID.ClayPot
            };
            TileObjectData.addSubTile(2);
            //vicious mushroom
            TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleAlch);
            TileObjectData.newSubTile.AnchorValidTiles = new int[1]
            {
                TileID.FleshGrass
            };
            TileObjectData.newSubTile.AnchorAlternateTiles = new int[1]
            {
                TileID.ClayPot
            };
            TileObjectData.addSubTile(3);
            TileObjectData.addTile(Type);
        }
    }
}