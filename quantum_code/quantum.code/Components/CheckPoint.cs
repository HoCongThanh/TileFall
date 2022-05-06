using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe partial struct CheckPoint
    {
        public void RespawnPlayer(Frame f, EntityRef playerEntity)
        {
            if(f.Unsafe.TryGetPointer<Transform3D>(playerEntity, out var playerTransform) &&
               f.TryResolveList(listRespawnPoint, out var respawnPoints))
            {
                // Todo: write logic in here to ensure player not spawn overlap each other
                var respawrnPositionIdx = f.RNG->Next(0, respawnPoints.Count);

                Log.Info("RespawnPlayer " + respawrnPositionIdx);

                playerTransform->Position = respawnPoints[respawrnPositionIdx];
            }
        }
    }
}
