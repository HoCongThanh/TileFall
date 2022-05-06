
using Photon.Deterministic;

namespace Quantum
{
    class OverlapSphereQuerySystem : SystemMainThread
    {
        public unsafe override void Update(Frame f)
        {
            foreach (var overlapSphereQueryComp in f.Unsafe.GetComponentBlockIterator<OverlapSphereQueryComp>())
            {
                
                var comp = overlapSphereQueryComp.Component;
                if (comp->Options == 0)
                    comp->QueryIndex = f.Physics3D.AddOverlapShapeQuery(comp->Position + comp->Offset, FPQuaternion.Identity, Shape3D.CreateSphere(comp->SphereRadius), comp->Layer);
                else
                    comp->QueryIndex = f.Physics3D.AddOverlapShapeQuery(comp->Position + comp->Offset, FPQuaternion.Identity, Shape3D.CreateSphere(comp->SphereRadius), comp->Layer, (QueryOptions)comp->Options);
             
            }
        }
    }
}
