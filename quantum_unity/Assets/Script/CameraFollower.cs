using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : QuantumCallbacks
{
    [SerializeField]
    private Vector3 offset;

    public override void OnUpdateView(QuantumGame game)
    {
        base.OnUpdateView(game);

        var target = PlayerEntityView.LocalPlayerEntityView;

        if(target == null)
        {
            return;
        }

        transform.position = target.transform.position + offset;
    }
}
