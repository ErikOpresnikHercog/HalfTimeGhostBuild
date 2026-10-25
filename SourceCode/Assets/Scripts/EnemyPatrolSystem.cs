
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrolSystem : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float pointReachDistance = 0.15f;

    [Header("Chase")]
    public Transform player;
    public PlayerFormSwitcher playerForms;
    public float chaseSpeed = 3.5f;
    public float detectionRange = 5f;

    [Header("Pathfinding")]
    [Tooltip("Layers containing walls and solid obstacles.")]
    public LayerMask obstacleLayer;
    [Min(0.1f)] public float cellSize = 0.5f;
    [Min(0.05f)] public float obstacleCheckRadius = 0.2f;
    [Min(0.1f)] public float repathInterval = 0.35f;
    [Min(1)] public int searchMargin = 8;

    [Header("Animation")]
    public Animator animator;
    public string moveXParameter = "MoveX";
    public string moveYParameter = "MoveY";
    public string speedParameter = "Speed";

    private Rigidbody2D rb;
    private int currentPoint;
    private bool chasing;
    private float repathTimer;

    private List<Vector2> path = new List<Vector2>();
    private int pathIndex;
    private bool hasPath;

    private Vector2 lastMoveDirection = Vector2.down;

    public LevelLoader Transitions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        Transitions = FindObjectOfType<LevelLoader>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");

            if (obj != null)
                player = obj.transform;
        }

        if (player != null && playerForms == null)
            playerForms = player.GetComponent<PlayerFormSwitcher>();

        RecalculatePath();
    }

    private void Update()
    {
        chasing = player != null &&
                  playerForms != null &&
                  playerForms.IsSolid &&
                  Vector2.Distance(transform.position, player.position)
                      <= detectionRange;

        repathTimer -= Time.deltaTime;

        if (repathTimer <= 0f)
        {
            RecalculatePath();
            repathTimer = repathInterval;
        }
    }

    private void FixedUpdate()
    {
        if (chasing)
        {
            FollowPath(chaseSpeed);
            return;
        }

        Patrol();
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            UpdateAnimation(Vector2.zero);
            return;
        }

        Transform target = patrolPoints[currentPoint];

        if (target == null)
        {
            UpdateAnimation(Vector2.zero);
            return;
        }

        if (Vector2.Distance(rb.position, target.position)
            <= pointReachDistance)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            RecalculatePath();
            return;
        }

        FollowPath(patrolSpeed);
    }

    private Transform GetCurrentTarget()
    {
        if (chasing)
            return player;

        if (patrolPoints == null || patrolPoints.Length == 0)
            return null;

        return patrolPoints[currentPoint];
    }

    private void RecalculatePath()
    {
        Transform target = GetCurrentTarget();

        if (target == null)
        {
            hasPath = false;
            return;
        }

        path = FindPath(rb.position, target.position);
        pathIndex = 0;
        hasPath = path != null && path.Count > 0;
    }

    private void FollowPath(float speed)
    {
        if (!hasPath || path == null || pathIndex >= path.Count)
        {
            UpdateAnimation(Vector2.zero);
            return;
        }

        while (pathIndex < path.Count &&
               Vector2.Distance(rb.position, path[pathIndex])
                   <= pointReachDistance)
        {
            pathIndex++;
        }

        if (pathIndex >= path.Count)
        {
            UpdateAnimation(Vector2.zero);
            return;
        }

        Vector2 target = path[pathIndex];
        Vector2 difference = target - rb.position;

        Vector2 direction;

        // Four-way movement: no diagonal animation directions.
        if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
            direction = new Vector2(Mathf.Sign(difference.x), 0f);
        else
            direction = new Vector2(0f, Mathf.Sign(difference.y));

        Vector2 nextPosition = Vector2.MoveTowards(
            rb.position,
            target,
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(nextPosition);
        UpdateAnimation(direction);
    }

    private void UpdateAnimation(Vector2 direction)
    {
        if (animator == null)
            return;

        bool moving = direction.sqrMagnitude > 0.01f;

        if (moving)
            lastMoveDirection = direction;

        animator.SetFloat(moveXParameter, lastMoveDirection.x);
        animator.SetFloat(moveYParameter, lastMoveDirection.y);
        animator.SetFloat(speedParameter, moving ? 1f : 0f);
    }

    // --------------------------------------------------
    // A* PATHFINDING
    // --------------------------------------------------

    private List<Vector2> FindPath(Vector2 start, Vector2 target)
    {
        float size = Mathf.Max(0.1f, cellSize);

        Vector2 min = new Vector2(
            Mathf.Min(start.x, target.x),
            Mathf.Min(start.y, target.y)
        ) - Vector2.one * (searchMargin * size);

        Vector2 max = new Vector2(
            Mathf.Max(start.x, target.x),
            Mathf.Max(start.y, target.y)
        ) + Vector2.one * (searchMargin * size);

        int width = Mathf.Max(
            2, Mathf.CeilToInt((max.x - min.x) / size) + 1
        );

        int height = Mathf.Max(
            2, Mathf.CeilToInt((max.y - min.y) / size) + 1
        );

        bool[,] blocked = new bool[width, height];

        Vector2Int startCell =
            WorldToCell(start, min, size, width, height);

        Vector2Int targetCell =
            WorldToCell(target, min, size, width, height);

        // Detect walls and obstacles.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = CellToWorld(x, y, min, size);

                blocked[x, y] = Physics2D.OverlapCircle(
                    position,
                    obstacleCheckRadius,
                    obstacleLayer
                ) != null;
            }
        }

        // Allow start and destination cells.
        blocked[startCell.x, startCell.y] = false;
        blocked[targetCell.x, targetCell.y] = false;

        List<Vector2Int> open = new List<Vector2Int>();
        bool[,] closed = new bool[width, height];
        Vector2Int[,] parent = new Vector2Int[width, height];
        float[,] gCost = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gCost[x, y] = float.PositiveInfinity;
                parent[x, y] = new Vector2Int(-1, -1);
            }
        }

        gCost[startCell.x, startCell.y] = 0f;
        open.Add(startCell);

        Vector2Int[] directions =
        {
            new Vector2Int( 1,  0),
            new Vector2Int(-1,  0),
            new Vector2Int( 0,  1),
            new Vector2Int( 0, -1),
            new Vector2Int( 1,  1),
            new Vector2Int( 1, -1),
            new Vector2Int(-1,  1),
            new Vector2Int(-1, -1)
        };

        bool found = false;

        while (open.Count > 0)
        {
            int bestIndex = 0;
            float bestScore = float.PositiveInfinity;

            for (int i = 0; i < open.Count; i++)
            {
                Vector2Int cell = open[i];

                float score = gCost[cell.x, cell.y]
                    + Heuristic(cell, targetCell);

                if (score < bestScore)
                {
                    bestScore = score;
                    bestIndex = i;
                }
            }

            Vector2Int current = open[bestIndex];
            open.RemoveAt(bestIndex);

            if (current == targetCell)
            {
                found = true;
                break;
            }

            if (closed[current.x, current.y])
                continue;

            closed[current.x, current.y] = true;

            foreach (Vector2Int direction in directions)
            {
                Vector2Int next = current + direction;

                if (next.x < 0 || next.y < 0 ||
                    next.x >= width || next.y >= height)
                    continue;

                if (blocked[next.x, next.y] ||
                    closed[next.x, next.y])
                    continue;

                // Don't cut diagonally through wall corners.
                if (direction.x != 0 && direction.y != 0)
                {
                    if (blocked[current.x + direction.x, current.y] ||
                        blocked[current.x, current.y + direction.y])
                        continue;
                }

                float moveCost =
                    direction.x != 0 && direction.y != 0
                        ? 1.4142f
                        : 1f;

                float tentativeCost =
                    gCost[current.x, current.y] + moveCost;

                if (tentativeCost < gCost[next.x, next.y])
                {
                    gCost[next.x, next.y] = tentativeCost;
                    parent[next.x, next.y] = current;

                    if (!open.Contains(next))
                        open.Add(next);
                }
            }
        }

        if (!found)
            return new List<Vector2>();

        List<Vector2> result = new List<Vector2>();
        Vector2Int step = targetCell;

        while (step != startCell)
        {
            result.Add(CellToWorld(step.x, step.y, min, size));

            step = parent[step.x, step.y];

            if (step.x < 0 || step.y < 0)
                return new List<Vector2>();
        }

        result.Reverse();
        result.Add(target);

        return result;
    }

    private Vector2Int WorldToCell(
        Vector2 position,
        Vector2 min,
        float size,
        int width,
        int height)
    {
        int x = Mathf.Clamp(
            Mathf.RoundToInt((position.x - min.x) / size),
            0, width - 1
        );

        int y = Mathf.Clamp(
            Mathf.RoundToInt((position.y - min.y) / size),
            0, height - 1
        );

        return new Vector2Int(x, y);
    }

    private Vector2 CellToWorld(
        int x, int y, Vector2 min, float size)
    {
        return min + new Vector2(x * size, y * size);
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        return Mathf.Max(dx, dy)
            + (1.4142f - 1f) * Mathf.Min(dx, dy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (patrolPoints != null)
        {
            Gizmos.color = Color.green;

            foreach (Transform point in patrolPoints)
            {
                if (point != null)
                    Gizmos.DrawSphere(point.position, 0.12f);
            }
        }

        if (path != null)
        {
            Gizmos.color = Color.cyan;

            for (int i = 0; i < path.Count - 1; i++)
                Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Player");
        if (other.CompareTag("Player"))
        {
            Transitions.GetComponent<LevelLoader>().LoadNextLevel(0);
        }
    }
}
