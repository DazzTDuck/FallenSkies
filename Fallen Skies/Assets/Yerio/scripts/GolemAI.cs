using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemAI : MonoBehaviour
{
    //FOV detector
    public GameObject player;
    public float maxAngle;
    public float maxRadius;
    private bool isInFov = false;

    //Patrol
    public List<GameObject> patrolSpots = new List<GameObject>();
    int currentSpotIndex;
    Vector3 currentPosToMove;
    float currentMoveSpeed;
    float sprintSpeed;
    bool foundPlayer;

    private void OnDrawGizmosSelected()
    {
        //player detection
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        if (!isInFov)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawRay(transform.position, (player.transform.position - transform.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
    }

    private void Start()
    {
        PatrolChoose();
        currentMoveSpeed = GetComponent<NavMeshAgent>().speed;
         sprintSpeed = GetComponent<NavMeshAgent>().speed * 2.5f;
    }
    private void Update()
    {
        //FOV detector
        if(player.transform.position.y < 1.2f)
        isInFov = InFOV(this.transform, player.transform, maxAngle, maxRadius);        

        //AI move
        MoveingAI();

        if (isInFov)
        {
            foundPlayer = true;
            GetComponent<NavMeshAgent>().speed = sprintSpeed;
        }
        else if(foundPlayer && !isInFov)
        {
            StartCoroutine("SearchForPlayer");
        }
    }

    //player detection
    public static bool InFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        Collider[] overlap = new Collider[100];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlap);

        for (int i = 0; i < count + 1; i++)
        {
            if (overlap[i] != null)
            {             
                if (overlap[i].transform == target)
                {
                    Vector3 dirBetween = (target.position - checkingObject.position).normalized;
                    dirBetween.y *= -0.69f;

                    float angle = Vector3.Angle(checkingObject.forward, dirBetween);

                    if (angle <= maxAngle)
                    {
                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {
                            if (hit.transform == target)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    //Patrol
    void PatrolChoose()
    {   
        currentSpotIndex = Random.Range(0, patrolSpots.Count);
        currentPosToMove = patrolSpots[currentSpotIndex].transform.position;      
    }

    void MoveingAI()
    {
        if (foundPlayer)
        {   
            if(GetComponent<NavMeshAgent>().speed != 0)
            {
                //transform.LookAt(player.position);
                //transform.position = Vector3.MoveTowards(transform.position, player.position, GetComponent<NavMeshAgent>().speed * Time.deltaTime);
                GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
            }

            //if on same place as player but what i want is to do if it is in damage range of the enemy do damage
            if (transform.position == player.transform.position)
            {
                Debug.Log("Do Damage");
            }
        }
        else
        {
            //transform.LookAt(currentPosToMove);
            //transform.position = Vector3.MoveTowards(transform.position, currentPosToMove, GetComponent<NavMeshAgent>().speed * Time.deltaTime);
            GetComponent<NavMeshAgent>().SetDestination(currentPosToMove);

            if (GetComponent<NavMeshAgent>().remainingDistance == 0f)
            {
                PatrolChoose();                            
            }
        }
    }

    IEnumerator SearchForPlayer()
    {
        if (!isInFov)
        {
            GetComponent<NavMeshAgent>().speed = 0;
            foundPlayer = false;
            yield return new WaitForSeconds(1);

            if (isInFov)
            {
                Debug.Log("found player again");
                foundPlayer = true;
                //call search Animation
                GetComponent<NavMeshAgent>().speed = currentMoveSpeed;
                StopCoroutine("SearchForPlayer");
            }
            else
            {
                Debug.Log("searching");
                //call search Animation
                if (isInFov)
                {
                    Debug.Log("found player again");
                    foundPlayer = true;
                    //call search Animation
                    GetComponent<NavMeshAgent>().speed = currentMoveSpeed;
                    StopCoroutine("SearchForPlayer");
                }
                yield return new WaitForSeconds(5);

                GetComponent<NavMeshAgent>().speed = currentMoveSpeed;
                PatrolChoose();
                Debug.Log("not found");
                StopCoroutine("SearchForPlayer");
            }
        }
    }

}
