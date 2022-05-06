using Photon.Deterministic;

namespace Quantum
{
    public unsafe struct MovementFilter
    {
        public EntityRef entity;
        public Transform3D* transform;
        public PhysicsBody3D* physicBody;
        public CharacterMovement* characterMovement;
    }

    public unsafe class MovementSystem : SystemMainThreadFilter<MovementFilter>,
        ISignalLockCharacterMovement,
        ISignalOnPlayerRespawn,
        ISignalOnPlayerWin,
        ISignalGameOver
    {
        public void GameOver(Frame f)
        {
            var filter = f.Filter<CharacterMovement>();

            while(filter.NextUnsafe(out var characterEntity, out var characterMovementComp))
            {
                Log.Info("Lock movement when game over");
                ClearAndLockMovementEntity(f, characterEntity, characterMovementComp);
            }
        }

        public void LockCharacterMovement(Frame f, EntityRef charaterEntity)
        {
            if(f.Unsafe.TryGetPointer<CharacterMovement>(charaterEntity, out var charMovementComp))
            {
                charMovementComp->LockMovement();
            }
        }

        public void OnPlayerRespawn(Frame f, EntityRef playerEntity)
        {
            if (f.Unsafe.TryGetPointer<CharacterMovement>(playerEntity, out var charMovementComp))
            {
                charMovementComp->ResetMovement(f, playerEntity);
            }
        }

        public void OnPlayerWin(Frame f, EntityRef playerEntity)
        {
            if (f.Unsafe.TryGetPointer<CharacterMovement>(playerEntity, out var charMovementComp))
            {
                Log.Info("Lock movement when player win");
                ClearAndLockMovementEntity(f, playerEntity, charMovementComp);
            }
        }

        private void ClearAndLockMovementEntity(Frame f, EntityRef playerEntity, CharacterMovement* characterMovementComp)
        {
            characterMovementComp->ResetMovement(f, playerEntity);
            characterMovementComp->LockMovement();
        }

        public override void Update(Frame f, ref MovementFilter filter)
        {
            filter.characterMovement->Update(f, filter.entity);
        }
    }
}
