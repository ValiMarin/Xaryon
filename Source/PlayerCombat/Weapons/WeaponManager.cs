using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform meleePoint;
    [SerializeField] private Transform pistolPoint;

    [Header("Current Loadout (drag & drop)")]
    [SerializeField] private GameObject meleePrefab;
    [SerializeField] private GameObject pistolPrefab;

    [Header("Refs")]
    [SerializeField] private RightHandAnimEventsHandler rightHand;
    [SerializeField] private LeftHandAnims leftHand;
    [SerializeField] private PlayerControls inputActions;
    [SerializeField] private PlayerSFX playerSFX;
    [SerializeField] private UI ui;

    [Header("Animator")]
    public Animator anim;
    [SerializeField] private AnimatorOverrideController swordOverride;
    [SerializeField] private AnimatorOverrideController pistolOverride;

    private static readonly int ChangeWeaponHash = Animator.StringToHash("ChangeWeapon");
    private static readonly int PistolHash = Animator.StringToHash("Pistol");

    private float switchCooldown = 0;

    private IWeapon meleeWeapon;
    private IWeapon pistolWeapon;
    private IWeapon currentWeapon;

    private GameObject meleeGO;
    private GameObject pistolGO;

    private bool isMelee = true;
    public bool IsMelee => isMelee;

    public event Action<bool> OnSwitchWeapon;

    private void Awake()
    {
        inputActions = InputManager.Instance.Controls;
    }

    private void Start()
    {
        SpawnWeapons();
        rightHand.SetWeapon(meleeWeapon);
        currentWeapon = meleeWeapon;
        ChangeWeapon();
    }

    private void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsPaused)
            return;

        if (switchCooldown > 0)
            switchCooldown -= Time.unscaledDeltaTime;
    }

    private void SpawnWeapons()
    {
        meleeGO = Instantiate(meleePrefab, meleePoint);
        meleeWeapon = meleeGO.GetComponent<IWeapon>();

        if (meleeWeapon is IMeleeWeapon weapon)
            leftHand.SetWeapon(weapon);

        pistolGO = Instantiate(pistolPrefab, pistolPoint);
        pistolWeapon = pistolGO.GetComponent<IWeapon>();

        var pistolController = pistolGO.GetComponent<PistolController>();

        ui.SetWeapon(pistolWeapon);

        pistolController.Initialize();
    }

    #region SWITCH

    private bool CanSwitch()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        return state.IsTag("Idle") || state.IsTag("Move");
    }

    public void SetCurrentWeapon(IWeapon newWeapon)
    {
        if (currentWeapon == newWeapon)
            return;

        if (!CanSwitch())
            return;

        if (switchCooldown > 0) 
            return;

        anim.SetTrigger(ChangeWeaponHash);

        currentWeapon = newWeapon;

        rightHand.SetWeapon(currentWeapon);
        playerSFX.ChangeWeapon();
        switchCooldown = 1f;
    }

    public void ChangeWeapon()
    {
        if (currentWeapon == meleeWeapon)
        {
            pistolGO.SetActive(false);
            meleeGO.SetActive(true);

            anim.runtimeAnimatorController = swordOverride;
            anim.SetBool(PistolHash, false);

            isMelee = true;

            OnSwitchWeapon?.Invoke(true);

            return;
        }

        pistolGO.SetActive(true);
        meleeGO.SetActive(false);
        anim.SetBool(PistolHash, true);

        isMelee = false;

        OnSwitchWeapon?.Invoke(false);

        anim.runtimeAnimatorController = pistolOverride;
    }

    public void DeadframeReady()
    {
        pistolGO.SetActive(false);
        meleeGO.SetActive(false);
    }

    public void DeadframeFinish()
    {
        if(currentWeapon == meleeWeapon)
            meleeGO.SetActive(true);
        else
            pistolGO.SetActive(true);
    }

    #endregion

    #region INPUT

    private void OnEnable()
    {
        inputActions.Player.SwitchWeaponScroll.performed += OnScroll;
        inputActions.Player.SelectMelee.performed += OnSelectMelee;
        inputActions.Player.SelectPistol.performed += OnSelectPistol;
    }

    private void OnDisable()
    {
        inputActions.Player.SwitchWeaponScroll.performed -= OnScroll;
        inputActions.Player.SelectMelee.performed -= OnSelectMelee;
        inputActions.Player.SelectPistol.performed -= OnSelectPistol;
    }

    private void OnSelectMelee(InputAction.CallbackContext ctx)
    {
        SetCurrentWeapon(meleeWeapon);
    }

    private void OnSelectPistol(InputAction.CallbackContext ctx)
    {
        SetCurrentWeapon(pistolWeapon);
    }

    private void OnScroll(InputAction.CallbackContext ctx)
    {
        IWeapon nextWeapon = currentWeapon == meleeWeapon ? pistolWeapon : meleeWeapon;

        SetCurrentWeapon(nextWeapon);
    }

    #endregion
}