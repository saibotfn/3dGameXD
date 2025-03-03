using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool inRange = false;
    [SerializeField] int range = 10;
    public GameObject pewPew;

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
        ray = new Ray(transform.position, other.transform.position - transform.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.gameObject.tag == "Player")
            {
                if ((transform.position - other.transform.position).magnitude < range)
                {
                    inRange = true;
                    agent.isStopped = true;
                    GetComponent<PewPew>().Shoot();
                    Debug.Log("enemy go pewpew");
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
