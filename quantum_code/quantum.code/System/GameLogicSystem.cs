using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    unsafe class GameLogicSystem : SystemSignalsOnly,
        ISignalOnPlayerTouchVictoryGround
    {
        public void OnPlayerTouchVictoryGround(Frame f, EntityRef playerEntity)
        {
            Log.Info("OnPlayerTouchVictoryGround " + f.IsVerified);
            var filter = f.Filter<GameLogicComp>();

            while(filter.NextUnsafe(out var gameLogicEntity, out var gameLogicComp))
            {
                gameLogicComp->OnPlayerWin(f, playerEntity);
            }
        }
    }
}
