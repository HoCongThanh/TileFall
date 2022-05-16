using Quantum;
using Quantum.Demo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerEntityView : MonoBehaviour
{
    internal static PlayerEntityView LocalPlayerEntityView;

    [SerializeField]
    private EntityView entityView;

    [SerializeField]
    private TextMesh txtMesh;

    bool Init = false;

    private unsafe void Awake()
    {
        StartCoroutine(SetUpPlayerEntity());
    }

    private IEnumerator SetUpPlayerEntity()
    {
        yield return new WaitUntil(() => QuantumRunner.Default.Session != null && QuantumRunner.Default.Session.FrameVerified != null);
        var f = QuantumRunner.Default.Game.Frames.Verified;
        yield return new WaitUntil(() => f.Exists(entityView.EntityRef));

        Debug.Log("SetUpPlayerEntity");
        InitEntityView(f);
    }

    private unsafe void InitEntityView(Frame f)
    {
        if(f.Unsafe.TryGetPointer<PlayerComp>(entityView.EntityRef, out var playerComp))
        {
            Debug.Log("PlayerEntityView Start " + playerComp->playerRef._index + "_" + QuantumRunner.Default.Session.IsLocalPlayer(playerComp->playerRef));

            if (QuantumRunner.Default.Session.IsLocalPlayer(playerComp->playerRef))
            {
                LocalPlayerEntityView = this;
            }

            txtMesh.text = f.GetPlayerData(playerComp->playerRef).PlayerName;
        }
    }
}
