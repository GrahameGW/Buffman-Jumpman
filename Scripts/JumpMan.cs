using UnityEngine;

public class JumpMan : Player, ICanThrow
{
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float enemyBounceHeight = 1f;

    public Rigidbody2D RigidBody => playerRb;
    public Transform Transform => transform;

    private GameObject anchor = null;
    private RopeSystem rope = null;

    [SerializeField] AudioClip jumpClip = default;


    public void Update()
    {
        if (playerRb.velocity.y < 0)
            animator.SetBool("Jump", false);
    }

    public override void MainAction()
    {
        Debug.Log("Jump!");
        Jump();
    }

    public override void SecondAction()
    {
        // DoRope()
    }

    private void Jump()
    {
        if (onGround) {
            playerRb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
            audioSource.PlayOneShot(jumpClip);
        }
    }

    public void Bounce()
    {
        Debug.Log("Bounce!");
        playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
        playerRb.AddForce(Vector2.up * enemyBounceHeight, ForceMode2D.Impulse);
        audioSource.PlayOneShot(jumpClip);
    }

    private void DoRope()
    {
        if (!anchor) {
            for (int i = 0; i < transform.childCount; i++) {
                rope = transform.GetChild(i).GetComponent<RopeSystem>();
                if (rope) {
                    anchor = transform.GetChild(i).gameObject;
                    break;
                }
            }

        }
        if (!rope.placed) {
            rope.gameObject.SetActive(true);
            rope.transform.SetParent(null, true);
            rope.plantAnchor();
        }
        else if (rope.attached) {
            rope.release();
        }
        else {
            rope.reset();
        }
    }

}
