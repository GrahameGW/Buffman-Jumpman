using System;
using UnityEngine;

public class SpikerEnemy : Enemy, ICanStomp
{
    [SerializeField] Vector2[] waypoints = new Vector2[2];
    private Vector2 currentWaypoint;

    private void Start()
    {
        currentWaypoint = waypoints[0];
    }

    private void OnValidate()
    {
        if (waypoints == null || waypoints.Length < 2) waypoints = new Vector2[2];
    }

    public override void Move(float input)
    {
        if (Vector2.Distance(transform.position, currentWaypoint) >= 0.05f) {
            var direction = currentWaypoint.x - transform.position.x;
            Facing = direction > 0 ? Facing.right : Facing.left;

            transform.Translate(new Vector2(direction, 0f).normalized * moveSpeed * Time.deltaTime);
        }
        else {
            if (currentWaypoint == waypoints[waypoints.Length - 1]) currentWaypoint = waypoints[0];
            else currentWaypoint = waypoints[Array.IndexOf(waypoints, currentWaypoint) + 1];
        }

        base.Move(input);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) { // players
            var player = collision.gameObject;
            var playerX = collision.bounds.center.x;
            var playerY = collision.bounds.center.y - collision.bounds.extents.y;
            var crawlerY = enemyCollider.bounds.center.y;
            var crawlerMinX = enemyCollider.bounds.center.x - enemyCollider.bounds.extents.x;
            var crawlerMaxX = enemyCollider.bounds.center.x + enemyCollider.bounds.extents.x;

            if (player.GetComponent<JumpMan>() != null && 
                playerY > crawlerY && playerX >= crawlerMinX && playerX <= crawlerMaxX) {
                Damage();
                player.GetComponent<JumpMan>().Bounce();
            }
            
            else {
                player.GetComponent<Player>().Damage();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        
        for (int i = 0; i < waypoints.Length; i++) {
            if (waypoints[i] != null) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(waypoints[i], 0.5f);
            }
        }
    }
}
