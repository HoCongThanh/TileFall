using UnityEngine;
using Quantum;

public unsafe class QuantumGameUpdater : QuantumCallbacks
{
    internal static Frame verifyFrame;


    public override void OnUpdateView(QuantumGame game)
    {
        base.OnUpdateView(game);
        var frames = game.Frames;

        verifyFrame = frames.Verified;
    }
}
