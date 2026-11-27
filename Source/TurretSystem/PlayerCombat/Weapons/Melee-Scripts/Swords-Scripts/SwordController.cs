using UnityEngine;
using UnityEngine.InputSystem;

public class SwordController : MonoBehaviour, IWeapon, IMeleeWeapon, IComboWeapon
{
    [SerializeField] private SwordStatusSO swordStatus;

    private Animator anim;

    private SwordAttack swordAttack;
    private SwordAbility swordAbility;
    private SwordSFX swordSFX;
    private SwordDash swordDash;
    private PlayerHighlight playerHighlight;

    private PlayerControls inputActions;
    private PlayerMovement playerMove;

    private bool abilityReady = false;
    private bool comboQueued = false;
    private bool windowCombo = false;
    private bool isHoldingAttack = false;
    private bool isSprinting = false;
    public bool abilityInUse => abilityReady;

    public event System.Action<bool> OnAbilityStateChanged;

    private void SetAbilityState(bool value)
    {
        if (abilityReady == value) return;

        abilityReady = value;
        OnAbilityStateChanged?.Invoke(abilityInUse);
    }

    #region Animator Hashes
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int AbilityHash = Animator.StringToHash("MeleeAbility");
    private static readonly int ActivateSlashHash = Animator.StringToHash("MeleeAbilityAction");
    #endregion

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        swordAttack = GetComponent<SwordAttack>();
        swordAbility = GetComponent<SwordAbility>();
        swordSFX = GetComponent<SwordSFX>();
        swordDash = GetComponent<SwordDash>();
        playerHighlight = GetComponentInParent<PlayerHighlight>();
        playerMove = PlayerMovement.PlayerMove;

        swordAttack.InitializeAttack(swordStatus);
        swordAbility.InitializeAbility(swordStatus, PlayerStatus.Player);
        swordDash.InitializeDashRange(swordStatus.abilityRange);
        playerHighlight.SetMeleeRange(swordStatus.basicRange, swordStatus.abilityRange, this);

        inputActions = InputManager.Instance.Controls;
    }

    private void OnEnable()
    {
        playerMove.OnSprint += HandleSprint;
        inputActions.Player.Attack.performed += OnAttackPerformed;
        inputActions.Player.Attack.canceled += OnAttackCanceled;
        inputActions.Player.MeleeSkill.performed += OnAbility;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttackPerformed;
        inputActions.Player.Attack.canceled -= OnAttackCanceled;
        inputActions.Player.MeleeSkill.performed -= OnAbility;
        playerMove.OnSprint -= HandleSprint;
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (!abilityReady)
        {
            isHoldingAttack = true;

            if (windowCombo)
            {
                comboQueued = true;
            }
            else
            {
                anim.SetBool(AttackHash, true);
            }

            return;
        }

        if (!swordDash.TryStartDash())
            return;

        swordSFX.StopAbilityIdle();
        anim.SetTrigger(ActivateSlashHash);
    }

    private void OnAttackCanceled(InputAction.CallbackContext ctx)
    {
        isHoldingAttack = false;
    }

    public void DealDamage() => swordAttack.DealDamage();
    public void AttackStart() => swordAttack.AttackStart();
    public void ActivateAbility()
    {
        swordAbility.ActivateAbility();

        swordSFX.PlaySlash();

        if (swordAbility.AbilityFinished())
            AbilitiFinished();
    }

    private void OnAbility(InputAction.CallbackContext ctx)
    {
        if (isSprinting) return;

        if (!abilityReady)
        {
            if (!swordAbility.CanUseAbility()) return;

            SetAbilityState(true);
            swordSFX.PlayAbilityIdle();
            anim.SetBool(AbilityHash, true);
        }
        else
        {
            AbilitiFinished();
            swordSFX.StopAbilityIdle();
        }
    }

    public void OpenComboWindow()
    {
        windowCombo = true;
        comboQueued = isHoldingAttack;
    }

    public void CloseComboWindow()
    {
        if (!comboQueued)
            anim.SetBool(AttackHash, false);

        windowCombo = false;
    }

    private void AbilitiFinished()
    {
        SetAbilityState(false);
        anim.SetBool(AttackHash, false);
        anim.SetBool(AbilityHash, false);

        if (swordAbility.AbilityInAction)
            swordAbility.AbilityCost();

        swordAbility.ResetTargets();
    }

    private void HandleSprint(bool isRunning)
    {
        if (isRunning)
        {
            AbilitiFinished();
            swordSFX.StopAbilityIdle();
        }

        isSprinting = isRunning;
    }
}