using Quantum.Demo;
using UnityEngine;

public class CustomCallbacks : QuantumCallbacks
{

    public override void OnGameStart(Quantum.QuantumGame game)
    {
        // paused on Start means waiting for Snapshot
        if (game.Session.IsPaused) return;

        foreach (var lp in game.GetLocalPlayers())
        {
            Debug.Log("CustomCallbacks - sending player: " + lp);

            var data = new Quantum.RuntimePlayer();

            if (UIMain.Client != null && UIMain.Client.IsConnected)
            {
                data.PlayerName = UIMain.Client.LocalPlayer.NickName;
            }

            game.SendPlayerData(lp, data);
        }
    }

    public override void OnGameResync(Quantum.QuantumGame game)
    {
        Debug.Log("Detected Resync. Verified tick: " + game.Frames.Verified.Number);
    }
}

