using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;
using System;
using Rocket.Unturned.Player;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.Unturned;


namespace BoneHurtingBullets
{
    public class BoneHurtingBullets : RocketPlugin<BoneHurtingBulletsConfiguration>
    {
        private static readonly System.Random rand = new System.Random();

        protected override void Load()
        {
            Rocket.Core.Logging.Logger.Log("BoneBreakingBullets Loaded, Chances:\n" + String.Join("\n", Configuration.Instance.boneBreakingChances.Select(x => $"{x.Limb}: {x.BreakChance}%").ToArray()));
            // DamageTool.playerDamaged += OnPlayerDamage;
            // damagetool doesn't work iv'e tried in diff proj
            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
        }

        protected override void Unload()
        {
            
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
        }
        /* Using new event */
        private void OnPlayerConnected(UnturnedPlayer player)
        {
            
            player.Player.life.onHurt += OnHurt;

        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            player.Player.life.onHurt -= OnHurt;

        }
        private void OnHurt(Player player, byte damage, Vector3 force, EDeathCause cause, ELimb limb, CSteamID killer)
        {
            if (cause == EDeathCause.GUN || cause == EDeathCause.PUNCH || cause == EDeathCause.MELEE)
            {

                var limbName = limb.ToString();

                var chance = Configuration.Instance.boneBreakingChances.FirstOrDefault(x => x.Limb == limbName);

                if (chance != null && rand.Next(1, 101) <= chance.BreakChance)
                {
                    player.life.breakLegs();
                }
                

            }
            else
            {
                //L
                return;
            }
            
            
            

        }
    }
}
