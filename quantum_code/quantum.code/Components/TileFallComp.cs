using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe partial struct TileFallComp
    {
        public void OnPlayerStepOnTile(Frame f, EntityRef playerEntity, EntityRef tileEntity)
        {
            if(f.Unsafe.TryGetPointer<TileFallComp>(tileEntity, out var tileComp))
            {
                if(tileComp->IsFallable)
                {
                    if (f.Unsafe.TryGetPointer<PhysicsBody3D>(tileEntity, out var tilePhysicBodyComp))
                    {
                        tilePhysicBodyComp->IsKinematic = false;
                    }
                    f.Events.OnTileFall(tileEntity);
                    f.Signals.LockCharacterMovement(playerEntity);
                }
                else
                {
                    f.Events.OnCorrectTileFound(tileEntity);
                }
            }
        }
    }
}
