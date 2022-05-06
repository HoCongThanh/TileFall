using UnityEngine;
using Quantum;

public class TileObserver : MonoBehaviour
{
    [SerializeField]
    private EntityView entityView;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material matCorrectTile;

    [SerializeField]
    private Material matWrongTile;

    private void Awake()
    {
        QuantumEvent.Subscribe<EventOnTileFall>(this, HandleOnTileFall);
        QuantumEvent.Subscribe<EventOnCorrectTileFound>(this, HandleOnCorrectTileFound);
    }

    private void OnDestroy()
    {
        QuantumEvent.UnsubscribeListener<EventOnTileFall>(this);
        QuantumEvent.UnsubscribeListener<EventOnCorrectTileFound>(this);
    }

    private void HandleOnTileFall(EventOnTileFall eventData)
    {
        if (entityView.EntityRef != eventData.tile)
            return;

        meshRenderer.material = matWrongTile;
    }

    private void HandleOnCorrectTileFound(EventOnCorrectTileFound eventData)
    {
        if (entityView.EntityRef != eventData.tile)
            return;

        meshRenderer.material = matCorrectTile;
    }
}
