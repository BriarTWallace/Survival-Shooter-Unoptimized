using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Scriptable Objects/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("Stats")]
    public int startingHealth = 100;
    public int scoreValue = 10;
    public int attackDamage = 10;
    public float attackInterval = 0.5f;

    [Header("Movement")]
    public float navSpeed = 3.5f;
    public float navAcceleration = 8f;
    public float navAngularSpeed = 120f;

    [Header("Audio / FX")]
    public AudioClip deathClip;
}
