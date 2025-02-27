using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool inRange = false;
    [SerializeField] int range = 5;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        ray = new Ray(transform.position, (transform.position - other.transform.position).normalized);


        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.gameObject.tag != "Object")
            {
                if ((transform.position - other.transform.position).magnitude < range)
                {
                    inRange = true;
                    agent.isStopped = true;
                }
                else
                {
                    inRange = false;
                    agent.isStopped = false;
                }

                agent.SetDestination(other.gameObject.transform.position);
            }
        }
    }
}
