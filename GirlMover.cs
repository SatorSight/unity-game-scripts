using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GirlMover : MonoBehaviour
{
    private float maxHealth = 5f;

    public float speed = 6;
    public float jumpAmount = 5;
    private Animator anim;
    private Vector3 direction;
    //public FollowTarget lookPos;
    private Camera camera;
    public GameObject firstCamera;
    public GameObject aimCamera;
    public GameObject pistol;
    public GameObject katana;
    private GameObject healthBar;
    public float health = 5f;

    public TextMeshProUGUI keysFoundText;
    public TextMeshProUGUI bulletsCounter;

    // from 0 to 2, because there are 3 key items in the game
    private int inventory = 0;
    private float jumpStarts = 0f;
    private float groundingStarts = 0f;
    private Rigidbody body;
    private PistolMover pistolMover;

    public int ammoTotal = 0;
    public int ammo = 8;

    //private GameObject escMenu;
    //private GameObject escMenuCameraObject;
    //private Camera escMenuCamera;

    private RaycastHit hitInfo = new RaycastHit();
    public LayerMask IgnoreMe;
    // TODO: remove duplicate by using gameObject on this one
    public Cinemachine.CinemachineVirtualCameraBase aimCam;
    private Cinemachine.CinemachineImpulseSource impulseSource;

    public GameObject muzzleFire;

    public bool hasKatana = false;
    public bool hasPistol = false;
    
    
    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    private float _verticalVelocity;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;


    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    
    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;
    
    private float _terminalVelocity = 53.0f;

    
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;
    
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 4.0f;
    
    [Header("Player")]
    [Tooltip("Move speed of the character when moving backwards in m/s")]
    public float BackMoveSpeed = 4.0f;


    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 7f;
    
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;


    private float _speed;
    private float _animationBlend;

    
    private AudioSource audioSource;
    public AudioClip footstepSound;
    public AudioClip footstepRunningSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip shotSound;
    public AudioClip reloadSound;
    public AudioClip katanaSwingSound;

    private CharacterController controller;
    
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float inputMagnitude;
    private float speedOffset;
    private static readonly int Attacking = Animator.StringToHash("Attacking");


    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        camera = Camera.main;
        healthBar = GameObject.Find("PlayerHealthBar");
        //escMenuCamera = escMenuCameraObject.GetComponent<Camera>();        

        katana.SetActive(false);
        pistol.SetActive(false);

        aimCamera.SetActive(false);

        pistolMover = (PistolMover)pistol.GetComponent(typeof(PistolMover));
        muzzleFire.SetActive(false);
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        bulletsCounter.gameObject.SetActive(false);
        controller = GetComponent<CharacterController>();
        
        audioSource = GetComponent<AudioSource>();

        // since I am using old input its always one, would be different for controller etc.
        inputMagnitude = 1f;

        // not sure what it does, something related to lerping speed calculation
        speedOffset = 0.1f;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        handleSwitchingWeaponInput();

        if (Input.GetKeyDown(KeyCode.R) && pistol.active) {
            reload();
        }

        if (pistol.active)
        {
            updateBulletsCounter();
            aimingModeAndShooting();
        }

        if (anim.GetBool("Aiming") == false && katana.active)
        {
            attackIfNeeded();
        }

        if (isDead())
        {
            return;
        }

        if (!anim.GetBool("PickingItem") && !anim.GetBool("Reloading"))
        {
            movement();
        }

        // inside we detect if jump key is pressed and if character can jump
        // jumpWhenNeeded();
        handleHealthBar();
    }

    private void updateBulletsCounter()
    {
        bulletsCounter.text = ammo.ToString() + " / " + ammoTotal.ToString();
    }
    
    // TODO: move this to separate script
    // called by animator event, can access through animator clicking on animation
    void OnFootstep(AnimationEvent animationEvent) 
    {
        audioSource.PlayOneShot(footstepSound);
    }
    
    void OnFootstepRunning(AnimationEvent animationEvent) 
    {
        // audioSource.PlayOneShot(footstepRunningSound);
    }
    
    void OnLand(AnimationEvent animationEvent) 
    {
        audioSource.PlayOneShot(landSound);
    }

    void ShotFired(AnimationEvent animationEvent)
    {
        audioSource.PlayOneShot(shotSound);
    }
    
    void OnReload(AnimationEvent animationEvent)
    {
        audioSource.PlayOneShot(reloadSound);
    }
    
    void OnKatanaHit(AnimationEvent animationEvent)
    {
        audioSource.PlayOneShot(katanaSwingSound);
    }

    public bool canOpenGate()
    {
        return inventory == 3;
    }

    public void aimingModeAndShooting()
    {

        // right click
        if (Input.GetMouseButton(1))
        {
            firstCamera.SetActive(false);
            aimCamera.SetActive(true);
            anim.SetBool("Aiming", true);
            // TODO: remove
            pistolMover.aiming = true;

            if (Input.GetMouseButtonDown(0))
            {
                handleShotFired();
                anim.SetBool("Shot", true);
                StartCoroutine(Shot());
            }
        }
        else
        {
            // if (anim.GetBool("Aiming"))
            // {
            //     // TODO: Make offset so the girl is facing slightly to the right
            //     transform.rotation = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
            // }

            firstCamera.SetActive(true);
            aimCamera.SetActive(false);
            anim.SetBool("Aiming", false);

            pistolMover.aiming = false;
        }

    }

    private void handleShotFired()
    {
        if (ammo == 0)
        {
            if (ammoTotal != 0)
            {
                reload();
            }
            else
            {
                // TODO: play click sound here
            }
        }
        else
        {
            if (anim.GetBool("Shot") == false)
            {
                Vector3 rayFromMe = aimCam.State.FinalPosition;
                Vector3 aimDir = aimCam.State.FinalOrientation * Vector3.forward;

                ammo--;

                impulseSource.GenerateImpulse(camera.transform.up * 0.1f);
                bool hit = Physics.Raycast(rayFromMe, aimDir, out hitInfo, 1000f, ~IgnoreMe);
                muzzleFire.active = true;
                StartCoroutine(ShotParticles());

                if (hit)
                {
                    Collider col = hitInfo.collider;

                    if (col && col.tag == "Destructible")
                    {
                        CrashCrate crate = (CrashCrate)col.GetComponent(typeof(CrashCrate));
                        crate.destroyed();
                    }

                    if (col && col.tag == "Enemy")
                    {
                        EnemyController other = (EnemyController)col.GetComponent(typeof(EnemyController));
                        // TODO: rename wtf
                        other.damagedWithKatana();
                    }
                }
            }

        }
    }

    private void handleSwitchingWeaponInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (hasKatana)
            {
                katana.active = true;
                anim.SetBool("ChangeGear", true);
                StartCoroutine(ChangeGear());
            }
            pistol.active = false;
            bulletsCounter.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            katana.active = false;
            if (hasPistol)
            {
                pistol.active = true;
                bulletsCounter.gameObject.SetActive(true);
                anim.SetBool("ChangeGear", true);
                StartCoroutine(ChangeGear());
            }
        }
    }

    private void movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // basic movement vector, need to be separate without speed so we can normalize it
        Vector3 moveVec = new Vector3(horizontal, 0, vertical);
        moveVec.Normalize();
        var move2d = new Vector2(horizontal, vertical);


        // need this for animation blending to better distinction when walking back
        var onlyNegativeVertical = vertical;
        if (onlyNegativeVertical > 0)
        {
            onlyNegativeVertical = 0;
        }

        anim.SetFloat("ForwardDirection", onlyNegativeVertical * 10);

        _speed = calcSpeed(move2d);
        calcAnimationBlend(move2d);

        if(anim.GetBool("Aiming") == false)
        {
            if (move2d != Vector2.zero)
            {
                rotateCharacter(moveVec, vertical);
            }
        } else
        {
            rotateCharacterWithCamera(moveVec);
        }

        
        // _targetRotation set in rotateCharacter
        Vector3 targetDirection2 = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        controller.Move(targetDirection2.normalized * (_speed * Time.deltaTime) +
                        new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        
        anim.SetFloat("Speed", _animationBlend);
        anim.SetFloat("MotionSpeed", inputMagnitude);
    }

    // rotate player and set _targetRotation
    private void rotateCharacter(Vector3 moveVec, float vertical)
    {
        _targetRotation = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
            RotationSmoothTime);
        // rotate to face input direction relative to camera position
            
        if (vertical > -0.01f)
        {
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        else
        {
            Quaternion toRotation = Quaternion.LookRotation(-targetDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
        }
    }

    private void calcAnimationBlend(Vector2 move2d)
    {
        float targetSpeed = calcTargetSpeed(move2d);
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;
    }

    private float calcSpeed(Vector2 move2d)
    {
        float resSpeed;
        float targetSpeed = calcTargetSpeed(move2d);
        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            resSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);
        
            // round speed to 3 decimal places
            resSpeed = Mathf.Round(resSpeed * 1000f) / 1000f;
            
        }
        else
        {
            resSpeed = targetSpeed;
        }
        return resSpeed;
    }
    
    private float calcTargetSpeed(Vector2 move2d)
    {
        bool isSprint = Input.GetKey(KeyCode.LeftShift);
        bool isMovingBackwards = move2d.y < -0.01f;

        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = MoveSpeed;
        if (isMovingBackwards)
        {
            targetSpeed = BackMoveSpeed;
        }
        else
        {
            if (isSprint)
            {
                targetSpeed = SprintSpeed;
            }
        }
        
        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (move2d == Vector2.zero) targetSpeed = 0.0f;

        return targetSpeed;
    }

    private void rotateCharacterWithCamera(Vector3 moveVec)
    {
        _targetRotation = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
        
        
        var rotationAmount = 10f;
        
        Quaternion cameraRotation = aimCamera.transform.rotation;

        // Create a rotation representing rotation to the right
        Quaternion rightRotation = Quaternion.AngleAxis(rotationAmount, Vector3.up);

        // Combine the camera's rotation with the right rotation
        Quaternion newRotation = cameraRotation * rightRotation;

        // Apply the new rotation to the character
        transform.rotation = newRotation;
        
        
        // transform.rotation = aimCamera.transform.rotation;
    }

    public void addToInventory()
    {
        inventory += 1;
        keysFoundText.text = "Keys " + inventory.ToString() + "/3";
    }

    public void addAmmo()
    {
        ammoTotal += 8;
    }

    private void reload()
    {
        if (ammo == 8 || ammoTotal == 0)
        {
            return;
        }

        anim.SetBool("Reloading", true);
        StartCoroutine(Reloading());

        int bulletsNeeded = 8 - ammo;

        if (ammoTotal <= bulletsNeeded)
        {
            ammo += ammoTotal;
            ammoTotal = 0;
        } else
        {
            ammoTotal -= bulletsNeeded;
            ammo += bulletsNeeded;
        }
    }

    private void handleHealthBar()
    {
        GameObject innerBar = healthBar.transform.Find("HealthBar/HealthBarInner").gameObject;
        RectTransform rect = (RectTransform)innerBar.GetComponent(typeof(RectTransform));
        rect.offsetMin = new Vector2(maxHealth - health, rect.offsetMin.y);
    }

    private void attackIfNeeded()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (anim.GetBool(Attacking) == false)
            {
                anim.SetBool(Attacking, true);
                StartCoroutine(Attack());
            }
        }
    }

    public void damagedByEnemy()
    {
        if (health < 0)
        {
            die();
            return;
        }
        anim.SetBool("Damaged", true);
        health -= 2f;
        Debug.Log("damaged by zombie");
        
        StartCoroutine(Damaged());
    }

    private void die()
    {
        anim.SetBool("Dead", true);
    }

    private bool isDead()
    {
        return anim.GetBool("Dead");
    }
    //
    // private bool someWeaponEquipped()
    // {
    //     return katana.active || pistol.active;
    // }
    //
    // private void jumpWhenNeeded()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         if (CanJump())
    //         {
    //             anim.SetBool("Jump", true);
    //             jumpStarts = Time.time;
    //             StartCoroutine(Jump());
    //         }
    //     }
    //
    //     if (anim.GetBool("Jump"))
    //     {
    //         // because character is grounded by default, we can only detect if jump is over after detecting second grounding
    //         // so we check for grounding only while characher is midair
    //         if (Time.time > jumpStarts + 0.5f)
    //         {
    //             if (isGrounded())
    //             {
    //                 groundingStarts = Time.time;
    //                 anim.SetBool("Jump", false);
    //             }
    //         }
    //     }
    // }

    // private bool CanJump()
    // {
    //     if (anim.GetBool("Jump") == true)
    //     {
    //         return false;
    //     }
    //
    //     if (Time.time < groundingStarts + 0.2f)
    //     {
    //         return false;
    //     }
    //
    //     return true;
    // }

    // detect if character hits the ground, though being triggered two times: when jump starts and when it ends
    // private bool isGrounded()
    // {
    //     RaycastHit hit;
    //
    //     if (Physics.Raycast(transform.position, -transform.up, out hit, 1f))
    //     {
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    // }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        anim.SetBool("Grounded", Grounded);
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // update animator if using character
            anim.SetBool("Jump", false);
            anim.SetBool("FreeFall", false);

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                // update animator if using character
                anim.SetBool("Jump", true);
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                anim.SetBool("FreeFall", true);
            }

            // if we are not grounded, do not jump
            // _input.jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    public void handlePickingAnimation()
    {
        anim.SetBool("PickingItem", true);
        StartCoroutine(PickingItem());
    }

    // needed for coroutine cause we need to delay the jump so animation matches the jump
    // IEnumerator Jump()
    // {
    //     yield return new WaitForSeconds(0.15f);
    //     body.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
    // }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("Slashing", true);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Slashing", false);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool(Attacking, false);
    }

    IEnumerator Damaged()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Damaged", false);
    }

    IEnumerator Shot()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Shot", false);
    }

    IEnumerator ShotParticles()
    {
        //muzzleFire.active = true;
        yield return new WaitForSeconds(0.2f);
        muzzleFire.active = false;
    }

    IEnumerator PickingItem()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("PickingItem", false);
    }

    IEnumerator ChangeGear()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("ChangeGear", false);
    }

    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Reloading", false);
    }
}
