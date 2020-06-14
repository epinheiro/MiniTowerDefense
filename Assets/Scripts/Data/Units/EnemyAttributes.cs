using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/EnemyAttributes", order = 4)]
public class EnemyAttributes : ScriptableObject{
    public int life;
    public float speed;
}
