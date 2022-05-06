using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe partial struct GameLogicComp
    {
        public void OnPlayerWin(Frame f, EntityRef playerEntity)
        {
            if(f.Unsafe.TryGetPointer<PlayerComp>(playerEntity, out var playerComp))
            {
                Log.Info("OnPlayerWin");
                f.Events.OnPlayerWin(playerComp->playerRef);

                if (!f.Global->refListWinPlayer.TryResloveOrCreateQList(f, out var listWinPlayer))
                {
                    Log.Info("OnPlayerWin create list");
                    f.Global->refListWinPlayer = listWinPlayer;
                }

                listWinPlayer.Add(playerComp->playerRef);

                var currentWinPlayerCount = listWinPlayer.Count;
                Log.Info("OnPlayerWin " + currentWinPlayerCount + "_" + totalWinPlayerToGameOver);
                if (currentWinPlayerCount >= totalWinPlayerToGameOver)
                {
                    f.Signals.GameOver();
                }
            }
        }
    }
}
