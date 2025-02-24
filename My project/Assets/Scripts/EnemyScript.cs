using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool inRange = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if ((transform.position - other.transform.position).magnitude < 5)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
        
        if (inRange)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
        
        agent.SetDestination(other.gameObject.transform.position);
    }
}
