using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Wave/EnemyWave", order = 3)]
public class EnemyWave : ScriptableObject{
    public enum Type {Fast, Strong};

    public float timeUntilNextSubWave;

    public Type enemyType;

    public int quantity;

    public SpawnSystem.SpawnTypes formation;
}
