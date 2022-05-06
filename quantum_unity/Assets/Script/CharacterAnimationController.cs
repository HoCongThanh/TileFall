using Quantum;
using UnityEngine;

public class CharacterAnimationController : QuantumCallbacks
{
    [SerializeField]
    private EntityView entityView;

    [SerializeField]
    private Animator animator;

    private const string TRIGGER_IDLE_ANIMATOR = "Idle";
    private const string TRIGGER_JUMP_ANIMATOR = "Jump";
    private const string TRIGGER_JUMP_END_ANIMATOR = "Jump End";
    private const string BOOLEAN_MOVING_ANIMATOR = "Moving";
    private const string TRIGGER_FALL_ANIMATOR = "Fall";

    private EntityRef entityRef;

    private MoveState lastMoveState = MoveState.Idle;
    private JumpState lastJumpState = JumpState.OnTheGround;

    private void Start()
    {
        entityRef = entityView.EntityRef;
    }

    public unsafe override void OnUpdateView(QuantumGame game)
    {
        base.OnUpdateView(game);

        var f = game.Frames.Verified;
        if(f.TryGet<CharacterMovement>(entityRef, out var characterMovement))
        {
            var curMoveState = characterMovement.moveState;
            if (lastMoveState != curMoveState)
            {
                animator.SetBool(BOOLEAN_MOVING_ANIMATOR, curMoveState == MoveState.Moving);
            }
            lastMoveState = curMoveState;


            var curJumpState = characterMovement.jumpState;
            if (lastJumpState == JumpState.OnTheGround && curJumpState == JumpState.FirstJump)
            {
                animator.SetTrigger(TRIGGER_JUMP_ANIMATOR);
            }
            else if(lastJumpState == JumpState.FirstJump && curJumpState == JumpState.SecondJump)
            {
                animator.SetTrigger(TRIGGER_JUMP_ANIMATOR);
            }
            else if((lastJumpState == JumpState.FirstJump || lastJumpState == JumpState.SecondJump) &&
                    curJumpState == JumpState.OnTheGround)
            {
                animator.SetTrigger(TRIGGER_JUMP_END_ANIMATOR);
            }
            else if(lastJumpState == JumpState.Falling && curJumpState == JumpState.OnTheGround)
            {
                animator.SetTrigger(TRIGGER_JUMP_END_ANIMATOR);
            }
            else if(lastJumpState != JumpState.Falling && curJumpState == JumpState.Falling)
            {
                animator.SetTrigger(TRIGGER_FALL_ANIMATOR);
            }
            lastJumpState = curJumpState;

        }
    }
}
