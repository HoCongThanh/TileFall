using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    unsafe class CheckPointSystem : SystemSignalsOnly,
        ISignalOnCollisionEnter3D
    {
        public void OnCollisionEnter3D(Frame f, CollisionInfo3D info)
        {
            if(f.Unsafe.TryGetPointer<CheckPoint>(info.Entity, out var checkPointComp) && 
               f.Unsafe.TryGetPointer<RespawnComp>(info.Other, out var respawnComp))
            {
                respawnComp->AddCheckPoint(f, info.Entity);
            }
        }
    }
}
