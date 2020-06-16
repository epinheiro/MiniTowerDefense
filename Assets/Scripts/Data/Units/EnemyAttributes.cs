using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Units/EnemyAttributes", order = 4)]
public class EnemyAttributes : ScriptableObject{
    public int life;
    public float speed;

    public Color color;
}
