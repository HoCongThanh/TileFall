using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    unsafe class CollisionDetectSystem : SystemSignalsOnly,
        ISignalOnCollisionEnter3D
    {
        public void OnCollisionEnter3D(Frame f, CollisionInfo3D info)
        {
            if(f.Unsafe.TryGetPointer<OnCollisionEnterComp>(info.Entity, out var behaviourComp))
            {
                if(f.TryFindAsset<OnCollisionEnterBehaviour>(behaviourComp->behaviour.Id, out var behaviourAsset))
                {
                    behaviourAsset.ProcessOnTriggerEnter(f, info);
                }
            }
        }
    }
}
