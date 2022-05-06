using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    [Serializable]
    public unsafe partial class VictoryAreaCollisionBehaviour : OnCollisionEnterBehaviour
    {
        public override void ProcessOnTriggerEnter(Frame f, CollisionInfo3D collisionInfo)
        {
            if(f.Has<PlayerType>(collisionInfo.Other))
            {
                f.Signals.OnPlayerTouchVictoryGround(collisionInfo.Other);
            }
        }
    }
}
