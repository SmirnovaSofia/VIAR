using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Transform[] patrolPoints;

    public float detectDistance = 5f;
    public float loseDistance = 7f;

    public float waitAtPoint = 1f;

    private int currentPoint = 0;
    private bool chasing = false;

    private NavMeshAgent agent;
    private float waitTimer = 0f;

    private GameManager gm;

    // 🎵 ЗВУК
    public AudioSource audioSource;
    public AudioClip detectSound;

    private bool hasPlayedDetectSound = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        gm = Object.FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (player == null || agent == null) return;
        if (!agent.isOnNavMesh) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // 👁 логика обнаружения
        if (dist < detectDistance)
        {
            chasing = true;

            PlayDetectSoundOnce();
        }
        else if (dist > loseDistance)
        {
            chasing = false;
            hasPlayedDetectSound = false; // сброс звука
        }

        // 🤖 поведение
        if (chasing)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        if (agent.pathPending) return;

        Transform target = patrolPoints[currentPoint];

        agent.SetDestination(target.position);

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance + 0.2f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitAtPoint)
            {
                currentPoint = (currentPoint + 1) % patrolPoints.Length;
                waitTimer = 0f;
            }
        }
    }

    // 💀 игрок пойман
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gm.EnemyCaughtPlayer();
        }
    }

    // 🎵 звук обнаружения (1 раз)
    void PlayDetectSoundOnce()
    {
        if (!hasPlayedDetectSound)
        {
            if (audioSource != null && detectSound != null)
            {
                audioSource.PlayOneShot(detectSound);
            }

            hasPlayedDetectSound = true;
        }
    }
}