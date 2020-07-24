using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffMan : Player
{
    [SerializeField] float throwStrength = 5f;
    [SerializeField] float punchRange = 0.5f;
    [SerializeField] float grabRange = 0.25f;
    [SerializeField] ContactFilter2D punchFilter = new ContactFilter2D();
    [SerializeField] ContactFilter2D grabFilter = new ContactFilter2D();


    private ICanThrow throwable;

    public override void MainAction()
    {
        if (throwable != null) {
            Debug.Log("Throw");
            Throw();
        }
        else {
            Debug.Log("Punch!");
            Punch();
        }


    }

    public override void SecondAction()
    {
        Debug.Log("Grab!");
        if (throwable != null) {
            Drop();
        }
        else {
            Grab();
        }
    }

    private void Punch()
    {
        Vector2 direction = Facing == Facing.right ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Vector2 origin2 = new Vector2(transform.position.x, transform.position.y + 0.5f);
        Debug.DrawRay(origin, direction * punchRange, Color.green, 3f);
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        List<RaycastHit2D> hits2 = new List<RaycastHit2D>();


        Physics2D.Raycast(origin, direction, punchFilter, hits, punchRange);
        Physics2D.Raycast(origin2, direction, punchFilter, hits2, punchRange);

        hits.AddRange(hits2);

        if (hits.Count > 0) {
            List<GameObject> punches = hits.Select(h => h.collider.gameObject)
                                        .Where(s => s.GetComponent<ICanPunch>() != null)
                                        .ToList();

            for (int i = 0; i < punches.Count; i++) {
                punches[i].GetComponent<ICanPunch>().Punch();
            }
        }

        animator.ResetTrigger("Punch");
        animator.SetTrigger("Punch");
    }

    private void Throw()
    {
        if (throwable.Transform.IsChildOf(transform))
            throwable.Transform.SetParent(transform.parent);

        int xDir = Facing == Facing.right ? 1 : -1;
        Vector2 force = new Vector2(xDir * 0.5f * throwStrength, throwStrength);

        throwable.RigidBody.isKinematic = false;
        throwable.RigidBody.AddForce(force, ForceMode2D.Impulse);
        throwable = null;
    }

    private void Grab()
    {
        Vector2 direction = Facing == Facing.right ? Vector2.right : Vector2.left;

        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.5f);

        Debug.DrawRay(origin, direction * grabRange, Color.red, 3f);

        Physics2D.Raycast(origin, direction, grabFilter, hits, grabRange);

        if (hits.Count > 0) {
            ICanThrow grabbed = hits.Select(h => h.collider.gameObject.GetComponent<ICanThrow>())
                                    .Where(g => g != null)
                                    .ToArray()
                                    .FirstOrDefault();

            if (grabbed != null) {
                grabbed.Transform.SetParent(transform);
                grabbed.Transform.localPosition = Vector2.zero;
                grabbed.RigidBody.isKinematic = true;
                throwable = grabbed;
            }
        }
    }

    private void Drop()
    {
        if (throwable != null) {
            if (throwable.Transform.IsChildOf(transform))
                throwable.Transform.SetParent(transform.parent);

            throwable.RigidBody.gravityScale = 1;
            var backwards = Facing == Facing.right ? Vector2.left : Vector2.right;

            throwable.RigidBody.AddForce(backwards, ForceMode2D.Impulse);
            throwable = null;
        }
    }
}
