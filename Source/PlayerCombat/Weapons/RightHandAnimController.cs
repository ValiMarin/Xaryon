using UnityEngine;

public class RightHandAnimController : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement playerMove;
    private PlayerAbilities playerAbilities;

    private static readonly int MoveHash = Animator.StringToHash("Move");
    private static readonly int RunHash = Animator.StringToHash("Run");
    private static readonly int DeadframeHash = Animator.StringToHash("Deadframe");
    private static readonly int DeadframeActionHash = Animator.StringToHash("DeadframeAction");

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMove = PlayerMovement.PlayerMove;
        playerAbilities = GetComponentInParent<PlayerAbilities>();

        playerMove.OnMove += HandleMove; 
        playerMove.OnSprint += HandleSprint;
        playerAbilities.OnDeadframe += OnDeadframe;
        playerAbilities.AbilityTrigger += DeadframeHashAction;
    }

    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnPauseStarted += OnPause;
            GameStateManager.Instance.OnPauseEnded += OnResume;
        }
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnPauseStarted -= OnPause;
            GameStateManager.Instance.OnPauseEnded -= OnResume;
        }

        playerMove.OnMove -= HandleMove;
        playerMove.OnSprint -= HandleSprint;
        playerAbilities.OnDeadframe -= OnDeadframe;
        playerAbilities.AbilityTrigger -= DeadframeHashAction;
    }

    private void HandleMove(bool value)
    {
        anim.SetBool(MoveHash, value);
    }

    private void HandleSprint(bool value)
    {
        anim.SetBool(RunHash, value);
    }

    private void OnDeadframe(bool value)
    {
        anim.SetBool(DeadframeHash, value);
    }

    private void DeadframeHashAction()
    {
        anim.SetTrigger(DeadframeActionHash);
        anim.SetBool(DeadframeHash, false);
    }

    private void OnPause()
    {
        anim.speed = 0f;
    }

    private void OnResume()
    {
        anim.speed = 1f;
    }
}
