using System.Collections.Generic;
using Terraria;
using Terraria.World.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System.IO;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;

namespace BotanyPlus.Items
{
    public class GaiaStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates grass and moss on dirt and stone\n" +
                "Greatly increases alchemy plant collection when used to gather\n" +
                "Automatically replants seeds in clay pots and planter boxes\n" +
                "Use on planted saplings to instantly grow them into trees");
        }

        public override void SetDefaults()
        {
            item.damage = 14;
            item.melee = true;
            item.width = 24;
            item.height = 28;
            item.createTile = TileID.Grass;
            item.useTime = 1;
            item.useAnimation = 25;
            item.knockBack = 3f;
            item.value = 200000;
            item.useTurn = true;
            item.tileBoost = 2;
            item.rare = ItemRarityID.Lime;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StaffofRegrowth);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
            recipe.AddIngredient(ItemID.Vine, 5);
            recipe.AddIngredient(ItemID.LifeFruit, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            Tile targetTile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
            //check if target is a sapling, if multiplayer send coordinates to server, if singleplayer grow tree
            if (targetTile.type == TileID.Saplings)
            {
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    ModPacket growCoord = mod.GetPacket();
                    growCoord.Write((byte)0);
                    growCoord.Write(Player.tileTargetX);
                    growCoord.Write(Player.tileTargetY);
                    growCoord.Send();
                }
                else if ((Main.tile[Player.tileTargetX, Player.tileTargetY].frameX < 324 || Main.tile[Player.tileTargetX, Player.tileTargetY].frameX >= 540)
                    ? WorldGen.GrowTree(Player.tileTargetX, Player.tileTargetY) : WorldGen.GrowPalmTree(Player.tileTargetX, Player.tileTargetY))
                    WorldGen.TreeGrowFXCheck(Player.tileTargetX, Player.tileTargetY);
            }
            //adapted from vanilla code, if target is stone, counts nearby moss tiles and creates the most prominent type or chooses randomly if none are found
            if (targetTile.type == TileID.Stone)
            {
                int mossID = 0;
                int mossCount = 0;
                Point point = player.Center.ToTileCoordinates();
                Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
                WorldUtils.Gen(new Point(point.X - 25, point.Y - 25), new Shapes.Rectangle(50, 50), new Actions.TileScanner(182, 180, 179, 183, 181, 381).Output(dictionary));
                foreach (KeyValuePair<ushort, int> item in dictionary)
                {
                    if (item.Value > mossCount)
                    {
                        mossCount = item.Value;
                        mossID = item.Key;
                    }
                }
                if (mossCount == 0)
                {
                    mossID = Utils.SelectRandom<int>(Main.rand, 182, 180, 179, 183, 181);
                }
                if (mossID != 0)
                {
                    Main.tile[Player.tileTargetX, Player.tileTargetY].type = (ushort)mossID;
                    WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY);
                    if (Main.netMode != NetmodeID.SinglePlayer)
                        NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);
                }
            }
            return true;
        }

        public override bool Autoload(ref string name)
        {
            IL.Terraria.Player.PlaceThing += HookPlaceThing;
            return base.Autoload(ref name);
        }

        private void HookPlaceThing(ILContext il)
        {
            //staff of regrowth's ability to break plants is hardcoded, this IL patch makes it also check for the gaia staff in Player.PlaceThing()
            var c = new ILCursor(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(213)))
            {
                return;
            }
            ILLabel labelOr = c.DefineLabel();
            ILLabel labelFalse = c.DefineLabel();
            c.Index++;
            c.GotoNext(i => i.MatchLdcI4(213));
            c.GotoNext(i => i.MatchBneUn(out labelFalse));
            c.Remove();
            c.Emit(OpCodes.Beq, labelOr);
            c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate<Func<Player, bool>>(player => player.HeldItem.type == ModContent.ItemType<GaiaStaff>());
            c.Emit(OpCodes.Brfalse, labelFalse);
            c.MarkLabel(labelOr);
            return;
        }
    }

    public class GaiaStaffGlobal : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModContent.ItemType<GaiaStaff>())
            {
                tooltips.RemoveAt(tooltips.FindIndex(x => x.Name.Contains("Placeable")));
            }
        }
    }
}
