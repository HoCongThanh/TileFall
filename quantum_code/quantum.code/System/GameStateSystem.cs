using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe class GameStateSystem : SystemMainThread,
        ISignalOnPlayerDataSet,
        ISignalGameOver
    {
        public void GameOver(Frame f)
        {
            f.Events.GameOver();
        }

        public void OnPlayerDataSet(Frame f, PlayerRef player)
        {
            var filter = f.Filter<SpawnArea>();
            while(filter.NextUnsafe(out var entity, out var spawnAreaComp))
                spawnAreaComp->SpawnPlayer(f, player);
            f.Signals.InitGameData();
        }

        public override void OnInit(Frame f)
        {
            base.OnInit(f);
        }

        public override void Update(Frame f)
        {
        }
    }
}
