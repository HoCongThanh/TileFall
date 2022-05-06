using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    [Serializable]
    public partial class DeathAreaCollisionBehaviour : OnCollisionEnterBehaviour
    {
        public override void ProcessOnTriggerEnter(Frame f, CollisionInfo3D collisionInfo)
        {
            Log.Info("ProcessOnTriggerEnter DeathAreaCollisionBehaviour");
            f.Signals.OnObjectEnterDeathArea(collisionInfo.Other);
        }
    }
}
