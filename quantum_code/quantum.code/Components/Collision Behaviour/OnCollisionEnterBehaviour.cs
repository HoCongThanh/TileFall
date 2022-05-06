using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public abstract partial class OnCollisionEnterBehaviour
    {
        public abstract void ProcessOnTriggerEnter(Frame f, CollisionInfo3D collisionInfo);
    }
}
