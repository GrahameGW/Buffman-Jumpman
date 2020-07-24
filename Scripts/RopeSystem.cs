using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class RopeSystem : MonoBehaviour
{
    //general
    public bool attached; // currently attached to rope
    public bool placed; // is the rope placed
    [SerializeField] float ColliderRadius = 0.5f;
    private List<BoxCollider2D> colliders;

    // player
    [SerializeField] Player player;
    private Transform playerTrans;
    private Vector2 playerPos;

    // anchor
    private Vector2 anchorPos;
    private Rigidbody2D anchorRb;
    private SpriteRenderer anchorSprite;

    // rope
    [SerializeField] GameObject Rope;
    [SerializeField] float segmentLen = 0.5f;
    [SerializeField] float ropeWidth = 1.2f;
    [SerializeField] float ropeLength = 20f;
    [SerializeField] int contraintReps = 10;
    private LineRenderer lineRenderer;
    private List<Segment> segments = new List<Segment>();
    private int numSegments;


    public struct Segment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public Segment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }

    void Awake()
    {

        // Anchor refs
        anchorPos = GetComponent<Transform>().position;
        anchorSprite = GetComponent<SpriteRenderer>();

        // rope refs
        lineRenderer = Rope.GetComponent<LineRenderer>();


        // start vals
        attached = true;
        placed = true;
        numSegments = (int)Mathf.Floor(ropeLength / segmentLen);
        colliders = new List<BoxCollider2D>();
    }

    void Start()
    {
        // player refs
        playerTrans = player.GetComponent<Transform>();
        playerPos = playerTrans.position;


        for (int i = 0; i < numSegments; i++)
        {
            segments.Add(new Segment(anchorPos));
        }
        CreateColliders();
    }

    void Update()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        playerPos = playerTrans.position;
        animateRope();
        UpdateColliders();
    }

    public void plantAnchor()
    {
        anchorPos = new Vector2(playerPos.x, playerPos.y);
        Rope.SetActive(true);
        placed = true;
        Debug.Log("Anchor Planted");
    }

    public void release()
    {
        attached = false;
    }

    public void reset()
    {
        anchorPos = new Vector2(playerPos.x, playerPos.y);
        transform.parent = player.transform;
        segments = new List<Segment>();
        colliders = new List<BoxCollider2D>();
        Rope.SetActive(false);
        gameObject.SetActive(false);
        placed = false;
    }


    private void DrawRope()
    {
        float lineWidth = this.ropeWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] positions = new Vector3[numSegments];
        for (int i = 0; i < numSegments; i++)
        {
            positions[i] = segments[i].posNow;
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    private void animateRope()
    {

        Vector2 forceGravity = Physics.gravity;
        RaycastHit2D hit;


        for (int i = 1; i < numSegments; i++)
        {
            Segment seg = segments[i];
            Vector2 velocity = seg.posNow - seg.posOld;
            seg.posOld = seg.posNow;
            seg.posNow += velocity;
            seg.posNow += forceGravity * Time.fixedDeltaTime;

            hit = Physics2D.Raycast(seg.posNow, -Vector2.up, Mathf.Infinity, 11);
            if (hit)
            {
                //Debug.Log("collision");
                seg.posNow = seg.posOld;
            }

            segments[i] = seg;
        }
        float currLen = 0;
        for (int j = 0; j < contraintReps; j++)
        {
            currLen = constrain(currLen);
        }
    }

    private float constrain(float currLen)
    {
        RaycastHit2D hit;

        //Constrant to anchor 
        Segment firstSegment = segments[0];
        firstSegment.posNow = anchorPos;
        segments[0] = firstSegment;


        //Constrant to player
        if (attached)
        {
            if (currLen > ropeLength)
            {
                Vector2 dist = segments[numSegments - 2].posNow - playerPos;
                playerTrans.Translate(dist);
            }
            Segment endSegment = segments[segments.Count - 1];
            endSegment.posNow = playerPos;
            segments[segments.Count - 1] = endSegment;
        }


        for (int i = 0; i < this.numSegments - 1; i++)
        {
            Segment firstSeg = this.segments[i];
            Segment secondSeg = this.segments[i + 1];


            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;

            float error = Mathf.Abs(dist - segmentLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > segmentLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            
            Vector2 changeAmount = changeDir * error * 0.5f;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount;
                
                segments[i] = firstSeg;

                secondSeg.posNow += changeAmount;
                hit = Physics2D.Raycast(firstSeg.posNow, -Vector2.up, Mathf.Infinity, 11);
                if (hit)
                {
                    secondSeg.posNow -= changeAmount;
                }

                segments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                secondSeg.posNow += changeAmount;
                segments[i + 1] = secondSeg;
            }
        }
        return currLen;
    }

    public float getCurrLen()
    {
        float len = 0;
        for (int i = 0; i < this.numSegments - 1; i++)
        {
            len += (segments[i].posNow - segments[i + 1].posNow).magnitude;
        }
        return len;
    }

    private void enforceMaxLen()
    {
        if (getCurrLen() > ropeLength)
        {
            Vector2 dist = segments[numSegments - 1].posNow - playerPos;
            playerTrans.Translate(dist);
        }
    }


    private void CreateColliders()
    {
        Vector2 boxSize = new Vector2((float)ColliderRadius * segmentLen, (float)ColliderRadius * segmentLen);
        for (int i = 0; i < numSegments; i++)
        {
            BoxCollider2D boxCollider = Rope.AddComponent<BoxCollider2D>();
            boxCollider.size = boxSize;
            boxCollider.isTrigger = true;
            boxCollider.usedByComposite = true;
            boxCollider.offset = transform.InverseTransformPoint(segments[i].posNow);
            colliders.Add(boxCollider);
        }
    }

    private void UpdateColliders()
    {
        for (int i = 0; i < numSegments; i++)
        {
            colliders[i].offset = Rope.transform.InverseTransformPoint(segments[i].posNow);
        }
    }
}