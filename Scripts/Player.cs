using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour, IEntity
{
    public Facing Facing { get; private set; } = Facing.right;

    [SerializeField] int health = 2;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float climbSpeed = 2f;

    protected Rigidbody2D playerRb;
    protected Collider2D playerCollider;
    protected Animator animator;
    protected AudioSource audioSource;

    protected bool canClimb = false;
    protected bool onGround = false;
    protected bool onRamp = false;

    private SpriteRenderer sprite;
    [SerializeField] Material selectedMaterial = default;
    [SerializeField] Material inactiveMaterial = default;
    [SerializeField] AudioClip landingClip = default;
    [SerializeField] AudioClip runLoopClip = default;


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = runLoopClip;
    }

    public void Move(float input)
    {
        if (input != 0) {
            Facing = input > 0 ? Facing.right : Facing.left;
            animator.SetBool("Run", true);
            if (audioSource.clip != null && !audioSource.isPlaying) audioSource.UnPause();
        }
        else {
            animator.SetBool("Run", false);
            StartCoroutine(LerpDownSound());
        }

        if (onRamp)
        {
            transform.Translate(0f, Mathf.Abs(input) * Time.deltaTime * moveSpeed, 0f);
        }
        transform.Translate(input * Time.deltaTime * moveSpeed, 0f, 0f);

        sprite.flipX = Facing == Facing.left;
    }

    public virtual void Climb(float input)
    {
        if (input == 0 || !canClimb) return;

        transform.Translate(0f, input * Time.deltaTime * climbSpeed, 0f);
        playerRb.gravityScale = 0; // is climbing so no gravity
    }

    public virtual void MainAction()
    {
        throw new NotImplementedException("Entity does not have a Main Action. Give it one or use a different type");
    }

    public virtual void SecondAction()
    {
        throw new NotImplementedException("Entity does not have a Second Action. Give it one or use a different type");
    }

    /*
    public void InteractAction()
    {
        if (canInteractWith != null) {

        }
    }
    */

    public virtual void Damage()
    {
        Debug.Log(name + " was hurt!");
        health--;
        animator.ResetTrigger("Hit");
        animator.SetTrigger("Hit");

        if (health == 0) {
            Debug.Log(gameObject.name + " died!");
            // Animator will destroy game object
            GameManager.Instance.Lose();
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // using stay instead of enter in case of chaining climbs
    {
        if (collision.gameObject.tag == "Climbable") canClimb = true;
        if (collision.gameObject.tag == "Ramp") onRamp = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Climbable") {
            canClimb = false;
            playerRb.gravityScale = 1; // no longer climbing, has gravity
        }
        if (collision.gameObject.tag == "Ramp") onRamp = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11) { // ground
            onGround = true;
            animator.SetBool("Fall", false);
            //audioSource.PlayOneShot(landingClip);
        }
        /*
        if (collision.gameObject.GetComponent<DoorLever>() != null) {
            canInteractWith = collision.gameObject;
        }
        */
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11) { // ground
            onGround = false;
            animator.SetBool("Fall", true);
            //audioSource.Pause(); // no running noises on the ground
         }
        /*
        if (collision.gameObject.GetComponent<DoorLever>() != null) {
            canInteractWith = null;
        }
        */
    }

    public void Activate()
    {
        sprite.material = selectedMaterial;
    }

    public void Deactivate()
    {
        sprite.material = inactiveMaterial;
    }

    private IEnumerator LerpDownSound()
    {
        float vol;
        float defaultVol = vol = audioSource.volume;
        while (vol > 0) {
            vol -= 0.1f;
            yield return null;
        }
        audioSource.Pause();
        audioSource.volume = defaultVol;
    }
}

public enum Facing { left, right };
