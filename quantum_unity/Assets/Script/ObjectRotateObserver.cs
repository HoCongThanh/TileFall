using Quantum;
using UnityEngine;
using Photon.Deterministic;

public class ObjectRotateObserver : QuantumCallbacks
{
    [SerializeField]
    private EntityView entityView;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private float rotateSpeed;

    private EntityRef entityRef;

    private Vector3 targetDirection = Vector3.zero;

    private void Start()
    {
        entityRef = entityView.EntityRef;
    }

    private void Update()
    {
        if (targetDirection == Vector3.zero)
        {
            return;
        }

        targetTransform.forward = Vector3.RotateTowards(targetTransform.forward, targetDirection, Mathf.Deg2Rad * rotateSpeed * Time.deltaTime, 0);

    }

    public unsafe override void OnUpdateView(QuantumGame game)
    {
        base.OnUpdateView(game);

        var f = game.Frames.Verified;

        if(f.TryGet<CharacterMovement>(entityRef, out var movement))
        {
            targetDirection = movement.currentDirection.ToUnityVector3();
        }
    }
}
