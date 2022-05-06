using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe partial struct SpawnArea
    {
        public void SpawnPlayer(Frame f, PlayerRef player)
        {
            if(f.TryResolveList(spawnPositions, out var listSpawnPositions))
            {
                if (player._index > listSpawnPositions.Count)
                {
                    return;
                }

                if(f.TryFindAsset<SpawnData>(spawnData.Id, out var spawnDataAsset))
                {
                    var playerEntity = f.Create(spawnDataAsset.playerPrefab);
                    if (f.Unsafe.TryGetPointer<Transform3D>(playerEntity, out var playerTransform))
                    {
                        playerTransform->Position = listSpawnPositions[player._index - 1];
                    }

                    if (f.Unsafe.TryGetPointer<CharacterMovement>(playerEntity, out var characterMovement))
                    {
                        characterMovement->Init(f, player);
                    }

                    if (f.Unsafe.TryGetPointer<PlayerComp>(playerEntity, out var playerComp))
                    {
                        playerComp->Init(f, player);
                    }
                }
                
            }
        }
    }
}
