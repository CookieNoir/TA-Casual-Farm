using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class MeshAgentController : MonoBehaviour
{
    NavMeshAgent _meshAgent;

    private void Awake()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                _meshAgent.destination = hit.point;
            }
        }
    }
}
