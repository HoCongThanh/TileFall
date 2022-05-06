using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;

public class LocalInput : MonoBehaviour
{

    private void OnEnable()
    {
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    public void PollInput(CallbackPollInput callback)
    {
        // chỉ update input cho player của mình
        if (!QuantumRunner.Default.Game.Session.IsLocalPlayer(callback.Player))
        {
            return;
        }

        Quantum.Input i = new Quantum.Input();

        i.Jump = UnityEngine.Input.GetKey(KeyCode.Space);

        var x = UnityEngine.Input.GetAxis("Horizontal");
        var y = UnityEngine.Input.GetAxis("Vertical");

        i.Direction = new FPVector2(x.ToFP(), y.ToFP());

        callback.SetInput(i, DeterministicInputFlags.Repeatable);
    }
}
