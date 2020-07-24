using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Breakable : MonoBehaviour, ICanPunch
{
    private int health = 3;
    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] GameObject[] brokenPiecePrefabs = default;
    [SerializeField] float explodeForce = 1.5f;
    [SerializeField] float averageShardDecayTime = 5f;
    [Range(0, 1)]
    [SerializeField] float shardDecayVariance = 0.3f;
    [SerializeField] AudioClip[] hitClip = default;
    [SerializeField] AudioClip smashClip = default;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        animator.SetInteger("Health", health);
    }

    private void OnValidate()
    {
        for (int i = 0; i < brokenPiecePrefabs.Length; i++) {
            if (brokenPiecePrefabs[i].GetComponent<Rigidbody2D>() == null)
                brokenPiecePrefabs[i].AddComponent<Rigidbody2D>();
        }
    }

    public void Damage()
    {
        health--;
        Debug.Log("Hurt the wall!");

        int i = Random.Range(0, hitClip.Length - 1);
        audioSource.PlayOneShot(hitClip[i]);

        if (health <= 0) {
            ScatterShards();
            audioSource.PlayOneShot(smashClip);
            animator.ResetTrigger("Destroy");
            animator.SetTrigger("Destroy");
        }
        else {
            animator.ResetTrigger("Hit");
            animator.SetTrigger("Hit");
        }


    }

    private void ScatterShards()
    {
        for (int i = 0; i < brokenPiecePrefabs.Length; i++) {
            var piece = Instantiate(brokenPiecePrefabs[i], transform);
            piece.transform.localPosition = Vector2.zero;
            piece.transform.SetParent(transform.parent);
            var pieceRb = piece.GetComponent<Rigidbody2D>();

            Vector2 trajectory = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized;
            pieceRb.AddForce(trajectory * explodeForce, ForceMode2D.Impulse);

            ShardDecay(piece);
        }
    }

    private void ShardDecay(GameObject shard)
    {
        var min = averageShardDecayTime * (1 - shardDecayVariance);
        var max = averageShardDecayTime * (1 + shardDecayVariance);

        Destroy(shard, Random.Range(min, max));
    }

    public void Punch()
    {
        Damage();
    }
}
