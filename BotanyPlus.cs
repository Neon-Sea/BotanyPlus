using System.IO;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace BotanyPlus
{
	public class BotanyPlus : Mod
	{
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            switch (reader.ReadByte())
            {
                case 0:
                    StaffGrow(reader.ReadInt32(), reader.ReadInt32());
                    break;
                default:
                    break;
            }
        }

        public void StaffGrow(int x, int y)
        {
            if ((Main.tile[x, y].frameX < 324 || Main.tile[x, y].frameX >= 540)
                ? WorldGen.GrowTree(x, y) : WorldGen.GrowPalmTree(x, y))
                WorldGen.TreeGrowFXCheck(x, y);
        }
    }
}