using System.IO;
using Terraria.ModLoader;
using Terraria;

namespace BotanyPlus
{
	public class BotanyPlus : Mod
	{
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            int growX = reader.ReadInt32();
            int growY = reader.ReadInt32();
            if ((Main.tile[growX, growY].frameX < 324 || Main.tile[growX, growY].frameX >= 540)
                ? WorldGen.GrowTree(growX, growY) : WorldGen.GrowPalmTree(growX, growY))
                WorldGen.TreeGrowFXCheck(growX, growY);
        }
    }
}