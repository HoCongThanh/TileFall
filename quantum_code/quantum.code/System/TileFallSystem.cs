using Quantum.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe class TileFallSystem : SystemSignalsOnly,
        ISignalOnCollisionEnter3D,
        ISignalInitGameData
    {
        public void InitGameData(Frame f)
        {
            var filter = f.Filter<TileFallAreaDataComp>();

            while (filter.NextUnsafe(out var tfAreaDataEntity, out var tfAreaDataComp))
            {
                tfAreaDataComp->Init(f);
            }
        }

        public void OnCollisionEnter3D(Frame f, CollisionInfo3D info)
        {
            if (f.Has<PlayerType>(info.Other) && f.Has<TileType>(info.Entity))
            {
                if (f.Unsafe.TryGetPointer<TileFallComp>(info.Entity, out var tileFallComp))
                {
                    tileFallComp->OnPlayerStepOnTile(f, info.Other, info.Entity);
                }
            }
        }

        public override void OnInit(Frame f)
        {
            base.OnInit(f);
        }
    }
}
