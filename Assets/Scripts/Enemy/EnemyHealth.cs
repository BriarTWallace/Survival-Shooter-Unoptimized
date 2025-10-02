using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyHealth : PooledObject
{
    public EnemyConfig config;

    public float sinkSpeed = 2.5f;


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    public bool isDead;
    bool isSinking;
    int currentHealth;

    private static readonly int hashDead = Animator.StringToHash("Dead");

    ScoreManager scoreManager;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
        scoreManager = FindObjectOfType<ScoreManager>();
        currentHealth = config.startingHealth;
    }


    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
            
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger(hashDead);

        enemyAudio.clip = config.deathClip;
        enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        scoreManager?.AddScore(config.scoreValue);
        StartCoroutine(SinkAndDespawn());
    }

    private IEnumerator SinkAndDespawn()
    {
        yield return new WaitForSeconds(2f);
        PoolManager.Instance.Despawn(gameObject);
    }

    public override void OnSpawned()
    {
        isDead = false;
        isSinking = false;
        currentHealth = config.startingHealth;
        if (capsuleCollider != null)
            capsuleCollider.isTrigger = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        var nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (nav != null)
        {
            nav.enabled = true;
            nav.isStopped = false;
            nav.ResetPath();
        }

        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f);
        }
    }

}
