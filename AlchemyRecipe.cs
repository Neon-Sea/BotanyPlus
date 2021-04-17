using Terraria;

namespace BotanyPlus
{
    public class AlchemyRecipes
    {
        public static int Consume(int numRequired)
        {
            //this is called by the IL patch in Recipe.Create() (see TransmutationTable.cs), saves ingredients with 2/3 chance
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