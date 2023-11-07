using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Bloons;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Unity;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSteamNative;
using Il2CppSystem.Security.Cryptography.X509Certificates;
using MelonLoader;

[assembly: MelonInfo(typeof(BtD6RandomizedRounds.Main), "Randomized Rounds", "1.0.0", "SpoXe")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BtD6RandomizedRounds
{
    public class Main : BloonsTD6Mod
    {
        public class RandomizedRounds : ModRoundSet
        {
            public override string BaseRoundSet => RoundSetType.Default;
            public override int DefinedRounds => BaseRounds.Count;
            public override string DisplayName => "RandomizedRounds";
            public override string Icon => VanillaSprites.RainbowBloonsIcon;

            public Dictionary<string, int> BloonsDictionary =
                new Dictionary<string, int>()
                {
                    {"Red", 1},
                    {"Blue",2},
                    {"Green",3},
                    {"Yellow",4},
                    {"Pink",5},
                    {"Black",11},
                    {"Purple",11},
                    {"White",11},
                    {"Lead",23},
                    {"Zebra",23},
                    {"Rainbow",47},
                    {"Ceramic",104},
                    {"Moab",596},
                    {"Bfb",3014},
                    {"Zomg",15656},
                    {"DdtCamo",848},
                    {"Bad",59056}
                };

            public override void ModifyRoundModels(RoundModel roundModel, int round)
            {
                roundModel.ClearBloonGroups();
                int maxDamage = (int)Math.Pow(Math.E,3+0.08*round);
                int currentDamage = maxDamage;
                string x;
                Random rand = new Random();
                int count;
                while (currentDamage > 0)
                {
                    x = BloonsDictionary.ElementAt(rand.Next(0, BloonsDictionary.Count)).Key;
                    count = rand.Next(1, 25);
                    if (currentDamage - BloonsDictionary[x] * count > maxDamage*-0.5)
                    {
                        int endtime = rand.Next(0,30*round);
                        roundModel.AddBloonGroup(x, count, 0, endtime);
                        currentDamage -= BloonsDictionary[x] * count;
                    }
                }

                foreach (var group in roundModel.groups)
                {
                    double randDouble = rand.NextDouble();
                    var bloon = Game.instance.model.GetBloon(group.bloon);
                    if (bloon.FindChangedBloonId(bloonModel => bloonModel.isCamo = true, out var camoBloon) && randDouble < 0.1)
                    {
                        group.bloon = camoBloon;
                    }
                }
            }
        }

        /*public override void OnMainMenu()
        {
            RandomizedRounds rr = new RandomizedRounds();
            RoundModel test = new RoundModel("random_rounds", new Il2CppReferenceArray<BloonGroupModel>(new BloonGroupModel[] {}));
            for (int i = 0; i < 100; i++)
            {
                rr.ModifyRoundModels(test,i);
            }
        }*/
    }
}
