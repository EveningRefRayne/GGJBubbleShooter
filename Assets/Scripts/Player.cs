using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CollisionMasks
{
    DEFAULT = 0x0001,
    TRANSPARENT_FX = 0x0002,
    IGNORE_RAYCAST = 0x0004,
    THREE = 0x0008,
    WATER = 0x0010,
    UI = 0x0020,
    SIX = 0x0040,
    SEVEN = 0x0080,
    TERRAIN = 0x0100,
    PROJECTILE = 0x0200,
    PLAYER = 0x0400,
    GUN = 0x0800,
    DEATH = 0x1000,
}

public enum GameStates
{
    TITLE, PLAY
}

public class Player : MonoBehaviour
{
    // "I'm allowed just one singleton, okay?" -Peng, 2020
    public static Player Me
    {
        get; private set;
    } = null;

    const bool JUMP_INFINITE = false;

    public float movementSpeed = 5f;
    public float rotationSpeed = 1f;
    public float pitchSpeed = 1f;
    public float jumpSpeed = 2f;
    public float runModifier = 2f;
    public int maxHealth = 3;
    public float reJumpTime = 1f;
    private float jumpTime;

    //public AudioClip victoryMusic;
    //public AudioSource victorySource;
    //public GameObject winScreen;

    public int Health
    {
        get; private set;
    }

    public GameStates Mode
    {
        get; private set;
    } = GameStates.TITLE;

    private bool lockCursor = true;
    private bool mlMode = true;
    private int maxJumpCount = 2;
    public int jumpsRemaining;
    private float maxIFrames = 1.5f;

    private Camera mainCamera;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Quaternion rotation;

    public float IFrames
    {
        get; private set;
    } = 0;

    void Awake()
    {
        if (Me)
        {
            Destroy(gameObject);
            return;
        }

        Me = this;
        Health = maxHealth;

        mainCamera = GetComponentInChildren<Camera>();
        rotation = transform.rotation;
        jumpsRemaining = maxJumpCount;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        EnterPlayMode(); //for basic testing
    }

    void FixedUpdate()
    {
        switch (Mode)
        {
            case GameStates.TITLE:
                break;
            case GameStates.PLAY:
                FixedUpdateGameplay();
                break;
        }
    }

    void Update()
    {
        switch (Mode)
        {
            case GameStates.TITLE:
                break;
            case GameStates.PLAY:
                UpdateGameplay();
                break;
        }
    }

    public void EnterWinScreen()
    {
        Mode = GameStates.TITLE;
        SetCursorLock(false);

        AudioSource[] musics = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource ms in musics)
        {
            ms.gameObject.SetActive(false);
        }

        /*
        victorySource.gameObject.SetActive(true);
        victorySource.Stop();
        victorySource.clip = victoryMusic;
        victorySource.Play();
        victorySource.volume = 0.65f;
        */
    }

    public void EnterPlayMode()
    {
        Mode = GameStates.PLAY;
        SetCursorLock(true);
    }

    private void FixedUpdateGameplay()
    {
        // Gather input
        float horizontal = Input.GetAxis("HorizontalMove");
        float vertical = Input.GetAxis("VerticalMove");
        float run = Input.GetAxis("Run");

        horizontal = ((horizontal > 0) ? 1 : ((horizontal < 0) ? -1 : 0));
        vertical = ((vertical > 0) ? 1 : ((vertical < 0) ? -1 : 0));
        run = (run > 0) ? runModifier : 1f;

        PlayerMovement(horizontal, vertical, run);
    }

    private void UpdateGameplay()
    {
        float aimHor = Input.GetAxis("HorizontalAim");
        float aimVer = Input.GetAxis("VerticalAim");
        bool jump = Input.GetButtonDown("Jump");

        if (mlMode)
        {
            if (lockCursor) CameraLook(aimHor, aimVer);
            UpdateCursorLock();
        }

        if (jump && (JumpAvailable() || JUMP_INFINITE || jumpsRemaining > 0))
        {
            Jump();
        }

        IFrames = Mathf.Max(0f, IFrames - Time.deltaTime);

        transform.position = GetComponentInChildren<Rigidbody>().transform.position;
    }

    private void PlayerMovement(float horizontal, float vertical, float speedModifier)
    {
        // Movement
        if (Mathf.Sqrt(horizontal * horizontal + vertical * vertical) > 0)
        {
            // trigonometry functions return radians but transform stuff uses degrees
            float movementAngle = Mathf.Atan2(-vertical, horizontal) * Mathf.Rad2Deg + rotation.eulerAngles.y;
            Vector3 position = transform.position;
            position = position + Quaternion.Euler(0f, movementAngle, 0f) * Vector3.right * movementSpeed * speedModifier * Time.deltaTime;
            transform.position = position;
        }
    }

    public void Jump()
    {
        //Debug.Log("Hit Jump");
        if (Time.time < jumpTime + reJumpTime) return;
        else
        {
            Rigidbody rb = GetComponentInChildren<Rigidbody>();
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
            rb.AddForce(Vector3.up * jumpSpeed);
            jumpTime = Time.time;
            jumpsRemaining--;
        }
    }

    public bool JumpAvailable()
    {
        if (Time.time < jumpTime + reJumpTime) return false;
        else
        {
            CapsuleCollider collider = GetComponentInChildren<CapsuleCollider>();
            //Debug.Log("Checking jump - height: " + collider.height + " and radius: " + collider.radius);
            Vector3 halfHeight = Vector3.up * ((collider.height / 2) - collider.radius);
            //Debug.Log("Half height: " + halfHeight);
            float checkDistance = collider.radius;
            float checkRadius = 0.9f;
            bool floored = Physics.CapsuleCast(transform.position, transform.position - halfHeight, collider.radius * checkRadius, Vector3.down, checkDistance, ((int)CollisionMasks.TERRAIN));
            if (floored)
            {
                //Debug.Log("On Ground");
                jumpsRemaining = maxJumpCount;
            }
            return floored;
        }
    }

    #region Rayne's camera code
    private void CameraLook(float horizontal, float vertical)
    {
        // Look side to side
        rotation = rotation * Quaternion.Euler(0f, horizontal * rotationSpeed, 0f);
        transform.rotation = rotation;

        // Look up and down
        mainCamera.GetComponent<Transform>().localRotation *= Quaternion.Euler(-vertical * pitchSpeed, 0f, 0f);
    }

    private void SetCursorLock(bool value)
    {
        //this is called internally to do the locking and unlocking when you want to lock or unlock the cursor.
        lockCursor = value;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SetMLMode(bool value)
    {
        //This is an external function to be called by other scripts to change into and out of mouselook mode entirely.
        mlMode = value;
        SetCursorLock(mlMode);
    }

    private void UpdateCursorLock()
    {
        //this checks every update if the player hit escape to unlock the mouse, or clicked back in.
        if (Input.GetButtonUp("Pause"))
        {
            SetCursorLock(false);
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            SetCursorLock(true);
        }
    }
    #endregion

    public void Damage(int amount = 1, float iFrames = -1f)
    {
        if (IFrames > 0)
        {
            return;
        }

        if (iFrames > 0)
        {
            IFrames = iFrames;
        }
        else
        {
            IFrames = maxIFrames;
        }

        Health = Mathf.Min(Health - amount, maxHealth);

        if (Health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Methods required by IMortal
    /// </summary>
    public void Die()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        Health = maxHealth;
    }
}