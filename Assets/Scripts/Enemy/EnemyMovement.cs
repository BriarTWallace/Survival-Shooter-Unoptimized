using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public EnemyConfig config;

    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();

        if (nav != null && config != null)
        {
            nav.speed = config.navSpeed;
            nav.acceleration = config.navAcceleration;
            nav.angularSpeed = config.navAngularSpeed;
        }
    }

    void Update()
    {
        if (enemyHealth != null && !enemyHealth.isDead && playerHealth != null && playerHealth.currentHealth > 0 && nav != null && nav.enabled && nav.isActiveAndEnabled)
            {
                nav.SetDestination(player.position);
            }
        else if (nav != null && nav.enabled)
            {
                nav.isStopped = true;
                nav.ResetPath();
            }
        }
    }
