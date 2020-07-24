using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour
{
    [SerializeField] DoorLever[] triggers = default;
    [SerializeField] Sprite openDoorSprite = default;
    [SerializeField] Sprite closedDoorSprite = default;
    [SerializeField] GameObject doorFramePrefab = default;

    private Collider2D doorCollider;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private bool open = false;

    private void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = closedDoorSprite;
        doorFramePrefab.SetActive(false);
    }

    private void Start()
    {
        for (int i = 0; i < triggers.Length; i++) {
            triggers[i].Door = this;
        }
    }

    public void CheckIfOpened()
    {
        if (open) return;

        for (int i = 0; i < triggers.Length; i++) {
            if (!triggers[i].On) return;
        }

        OpenDoor();
    }

    private void OpenDoor()
    {
        for (int i = 0; i < triggers.Length; i++) {
            triggers[i].LockTrigger();
        }

        doorCollider.enabled = false;
        spriteRenderer.sprite = openDoorSprite;
        doorFramePrefab.SetActive(true);
        audioSource.PlayOneShot(audioSource.clip);
        Debug.Log("Door Opened!");
        open = true;
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < triggers.Length; i++) {
            if (triggers[i] != null) {
                var collider = triggers[i].GetComponent<Collider2D>();
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(triggers[i].transform.position, collider.bounds.size);
            }
        }
    }
}
