using Pathfinding;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform goal;
    AIPath ai;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ai = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void Update()
    {
        ai.destination = goal.position;
    }
}
