using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalBehavior : MonoBehaviour
{
    UnityEvent GoalReached;
    UnityEvent GoalAbandoned;
    [SerializeField] Animator[] animators = default;


    void Start()
    {
        if (GoalReached == null) GoalReached = new UnityEvent();
        if (GoalAbandoned == null) GoalAbandoned = new UnityEvent();

        GoalReached.AddListener(GameManager.Instance.ReachedGoal);
        GoalAbandoned.AddListener(GameManager.Instance.LeftGoal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
            GoalReached.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
            GoalAbandoned.Invoke();
    }

    private void Update()
    {
        if (GameManager.Instance.GameWon) {
            for (int i = 0; i < animators.Length; i++) {
                animators[i].ResetTrigger("Win");
                animators[i].SetTrigger("Win");
            }
        }
    }

}
