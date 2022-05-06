using UnityEngine;
using Quantum;
using UnityEngine.UI;

public class GameStateObserver : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Text txtWin;

    [SerializeField]
    private Text txtGameOver;


    private void Awake()
    {
        QuantumEvent.Subscribe<EventOnPlayerWin>(this, HandleEventPlayerWin);
        QuantumEvent.Subscribe<EventGameOver>(this, HandleEventGameOver);

    }

    private void OnDestroy()
    {
        QuantumEvent.UnsubscribeListener<EventOnPlayerWin>(this);
        QuantumEvent.UnsubscribeListener<EventGameOver>(this);
    }

    private unsafe void HandleEventGameOver(EventGameOver eventGameOver)
    {
        var verifiedFrame = QuantumGameUpdater.verifyFrame;

        var refListWinPlayer = verifiedFrame.Global->refListWinPlayer;

        if(verifiedFrame.TryResolveList(refListWinPlayer, out var listWinPlayer))
        {
            if (listWinPlayer.IsContain(playerRef =>
            {
                return QuantumRunner.Default.Game.Session.IsLocalPlayer(playerRef);
            }))
            {
                txtWin.enabled = true;
            }
            else
            {
                txtGameOver.enabled = true;
            }
        }
    }

    private void HandleEventPlayerWin(EventOnPlayerWin eventOnPlayerWin)
    {
        if(!QuantumRunner.Default.Game.Session.IsLocalPlayer(eventOnPlayerWin.playerRef))
        {
            return;
        }

        txtWin.enabled = true;
    }
}
