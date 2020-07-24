
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DoorLever : MonoBehaviour
{
    [SerializeField] Sprite triggerOn = default;
    [SerializeField] Sprite triggerOff = default;

    public Door Door;
    public bool On {
        get => _on;
        private set {
            _on = value;
            Door.CheckIfOpened();
        }
    }
    private bool _on = false;

    private bool locked = false;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    AudioSource audioSource;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = triggerOff;
    }

    public void LockTrigger()
    {
        locked = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!locked && collision.gameObject.layer == 8) { // Players
            On = true;
            spriteRenderer.sprite = triggerOn;
            audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!locked && collision.gameObject.layer == 8) { // Players
            On = false;
            spriteRenderer.sprite = triggerOff;
            audioSource.Play();
        }
    }


}
