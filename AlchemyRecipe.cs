using Terraria;

namespace BotanyPlus
{
    public class AlchemyRecipes
    {
        public static int Consume(int numRequired)
        {
            if (Main.player[Main.myPlayer].GetModPlayer<BotanyPlayer>().tmTable)
            {
                int saved = 0;
                for (int n = 0; n < numRequired; n++)
                {
                    if (Main.rand.Next(3) <= 1)
                    {
                        saved++;
                    }
                }
                numRequired -= saved;
            }
            return numRequired;
        }
    }
}