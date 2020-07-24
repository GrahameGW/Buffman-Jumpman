using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IEntity
{
    public Facing Facing { get; protected set; } = Facing.right;

    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] int health = 1;

    protected Collider2D enemyCollider;

    private SpriteRenderer sprite;
    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] AudioClip hitClip = default;


    private void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if (GameManager.Instance.Playing && health > 0) Move(0);
        else animator.SetBool("Run", false);
    }

    public virtual void MainAction()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Move(float input)
    {
        sprite.flipX = Facing == Facing.right;
        animator.SetBool("Run", true);
    }

    public void Damage()
    {
        health--;
        // Automatically kills them and plays damage animation
        animator.ResetTrigger("Hit");
        animator.SetTrigger("Hit");
        audioSource.PlayOneShot(hitClip);
    }
}
