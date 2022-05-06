using Quantum;
using Quantum.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityView : MonoBehaviour
{
    internal static PlayerEntityView LocalPlayerEntityView;

    [SerializeField]
    private EntityView entityView;

    [SerializeField]
    private TextMesh txtMesh;

    private void Start()
    {
        InitEntityView(QuantumRunner.Default.Game.Frames.Verified);
    }

    private unsafe void InitEntityView(Frame f)
    {
        if(f.Unsafe.TryGetPointer<PlayerComp>(entityView.EntityRef, out var playerComp))
        {
            if (QuantumRunner.Default.Session.IsLocalPlayer(playerComp->playerRef))
            {
                LocalPlayerEntityView = this;
            }

            txtMesh.text = f.GetPlayerData(playerComp->playerRef).PlayerName;
        }
    }
}
