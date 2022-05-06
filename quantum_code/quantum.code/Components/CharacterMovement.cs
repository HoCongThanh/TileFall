using Photon.Deterministic;

namespace Quantum
{
    public unsafe partial struct CharacterMovement
    {
        public void Init(Frame f, PlayerRef playerRef)
        {
            this.Player = playerRef;

            var groundDetector = f.Create();
            f.Add<OverlapSphereQueryComp>(groundDetector, groundDetectorQueryComp);

            this.groundDetector = groundDetector;
        }

        public void Update(Frame f, EntityRef characterEntity)
        {
            var input = f.GetPlayerInput(Player);
            
            if(f.Unsafe.TryGetPointer<PhysicsBody3D>(characterEntity, out var physicBody))
            {
                UpdateDirectionMovement(f, input, physicBody);

                UpdateJump(f, input, physicBody, characterEntity);
            }
        }

        private unsafe void UpdateDirectionMovement(Frame f, Input* input, PhysicsBody3D* physicBody)
        {
            if (moveState == MoveState.Lock || jumpState == JumpState.SecondJump)
            {
                return;
            }

            var direction = input->Direction.XOY;

            if (direction == FPVector3.Zero)
            {
                if (jumpState == JumpState.OnTheGround)
                {
                    moveState = MoveState.Idle;
                    currentVelocityMagnitude = 0;

                    var curYVelocity = physicBody->Velocity.Y;

                    physicBody->Velocity = FPVector3.Zero + new FPVector3(0, curYVelocity);
                }
            }
            else
            {
                currentDirection = direction;

                moveState = MoveState.Moving;

                var curYVelocity = physicBody->Velocity.Y;

                currentVelocityMagnitude += Accelaration * f.DeltaTime;

                currentVelocityMagnitude = FPMath.Clamp(currentVelocityMagnitude, 0, MaxVelocityMagnitude);

                physicBody->Velocity = direction * currentVelocityMagnitude + new FPVector3(0, curYVelocity);
            }
        }

        private void UpdateJump(Frame f, Input* input, PhysicsBody3D* physicBody, EntityRef characterEntity)
        {
            if (f.Unsafe.TryGetPointer<OverlapSphereQueryComp>(groundDetector, out var groundDetectorComp))
            {
                var groundHits = f.Physics3D.GetQueryHits(groundDetectorComp->QueryIndex);

                //Log.Info("OverlapSphereQueryComp " + groundHits.Count + "_" + groundDetectorComp->QueryIndex);

                bool IsTouchingTheGround = groundHits.Count > 0;

                if (jumpState != JumpState.OnTheGround && physicBody->Velocity.Y < 0 && IsTouchingTheGround)
                {
                    Log.Info("Change to !OnTheGround to OnTheGround");
                    jumpState = JumpState.OnTheGround;
                }
                else if (jumpState == JumpState.OnTheGround && !IsTouchingTheGround)
                {
                    Log.Info("Change to OnTheGround to Falling");
                    jumpState = JumpState.Falling;
                }

                if (jumpState == JumpState.OnTheGround)
                {
                    physicBody->GravityScale = 0;
                }
                else
                {
                    physicBody->GravityScale = NormalGravityScale;
                }

                if (f.TryGet<Transform3D>(characterEntity, out var characterPosition))
                {
                    groundDetectorComp->Position = characterPosition.Position;
                }
            }

            if (moveState == MoveState.Lock)
            {
                return;
            }

            if (input->Jump.WasPressed && jumpState != JumpState.Falling)
            {
                if (jumpState == JumpState.OnTheGround)
                {
                    Log.Info("Change to OnTheGround to FirstJump");
                    jumpState = JumpState.FirstJump;
                    physicBody->AddLinearImpulse(FPVector3.Up * JumpImpulse);
                }
                else if (jumpState == JumpState.FirstJump)
                {
                    Log.Info("Change to FirstJump to SecondJump");
                    jumpState = JumpState.SecondJump;
                    physicBody->AddLinearImpulse(FPVector3.Up * DoubleJumpImpulse);
                    physicBody->AddLinearImpulse(currentDirection * DoubleJumpPushForce);
                }
            }
        }

        public void LockMovement()
        {
            moveState = MoveState.Lock;
        }

        public void UnlockMovement()
        {
            moveState = MoveState.Idle;
        }

        public void ResetJumpState()
        {
            jumpState = JumpState.OnTheGround;
        }

        public void ResetMovement(Frame f, EntityRef playerEntity)
        {
            if (f.Unsafe.TryGetPointer<PhysicsBody3D>(playerEntity, out var physicBody))
            {
                physicBody->Velocity = FPVector3.Zero;
                physicBody->ClearForce();
                physicBody->ClearTorque();
                UnlockMovement();
                ResetJumpState();

            }
        }
    }

    public enum MoveState : byte
    {
        Idle,
        Moving,
        Lock,
    }

    public enum JumpState : byte
    {
        OnTheGround,
        FirstJump,
        SecondJump,
        Falling,
    }
}
