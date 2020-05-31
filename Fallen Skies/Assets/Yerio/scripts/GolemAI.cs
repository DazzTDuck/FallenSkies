using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class GolemAI : MonoBehaviour
{
    //FOV detector
    public GameObject player;
    public float maxAngle;
    public float maxRadius;
    [HideInInspector]
    public float savedRadius;
    bool isInFov = false;

    //Patrol
    public List<GameObject> patrolSpots = new List<GameObject>();
    int currentSpotIndex;
    Vector3 currentPosToMove;
    float currentMoveSpeed;
    float sprintSpeed;
    [HideInInspector]
    public bool foundPlayer;
    [HideInInspector]
    public bool isSearching;

    [Header("Attack")]
    public float damage;
    public float damageDelay;
    public Transform attackPoint;
    public float attackRad;
    float damageTimer;

    float animationSpeed;

    int playerIndex;

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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRad);
    }

    private void Start()
    {
        PatrolChoose();
        currentMoveSpeed = GetComponent<NavMeshAgent>().speed;
         sprintSpeed = GetComponent<NavMeshAgent>().speed * 2.5f;
        animationSpeed = GetComponent<Animator>().speed;
        isSearching = false;
        savedRadius = maxRadius;
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

        CheckAttackRad();

        CheckIfGamePaused();
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
            if(GetComponent<NavMeshAgent>().speed != 0 && !player.GetComponent<OtherPlayerFunctions>().isPaused)
            {
                GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
                if (!isSearching)
                {
                    GetComponent<Animator>().SetTrigger("Walk");
                }                    
               
                if(GetComponent<Animator>().speed < 1.5f)
                {
                    GetComponent<Animator>().speed += 0.5f;                
                }             
            }
           
        }
        else
        {
            if (!player.GetComponent<OtherPlayerFunctions>().isPaused)
            {
                GetComponent<NavMeshAgent>().SetDestination(currentPosToMove);
                if (!isSearching)
                {
                    GetComponent<Animator>().SetTrigger("Walk");
                }

                if (GetComponent<Animator>().speed > 1)
                {
                    GetComponent<Animator>().speed -= 0.5f;
                }

                if (GetComponent<NavMeshAgent>().remainingDistance < 1.6f)
                {
                    PatrolChoose();
                }
            }           
        }
    }

    void CheckAttackRad()
    {
        Collider[] coll = Physics.OverlapSphere(attackPoint.position, attackRad);

        for (int i = 0; i < coll.Length; i++)
        {
            if (coll[i].CompareTag("Player"))
            {
                playerIndex = i;
                if (!player.GetComponent<OtherPlayerFunctions>().isPaused)
                {
                    damageTimer += Time.deltaTime;
                    //do damage to the player and slow down the golem
                    GetComponent<NavMeshAgent>().speed /= 2f;
                    GetComponent<Animator>().speed /= 1.3f;
                    if (damageTimer > damageDelay)
                    {
                        coll[playerIndex].GetComponent<OtherPlayerFunctions>().DoDamage(damage);
                        damageTimer = 0f;
                    }
                }              
            }

            if (playerIndex > coll.Length && !isSearching)
            {
                if (GetComponent<NavMeshAgent>().speed != currentMoveSpeed)
                    GetComponent<NavMeshAgent>().speed = currentMoveSpeed;
                if (GetComponent<Animator>().speed != animationSpeed)
                    GetComponent<Animator>().speed = animationSpeed;
            }
        }
    }

    IEnumerator SearchForPlayer()
    {
        if (!isInFov)
        {
            GetComponent<NavMeshAgent>().speed = 0;
            isSearching = true;
            GetComponent<Animator>().ResetTrigger("Walk");
            GetComponent<Animator>().SetTrigger("Idle");
            foundPlayer = false;          

            yield return new WaitForSeconds(1);


            if (isInFov)
            {
                Debug.Log("found player again");
                foundPlayer = true;
                isSearching = false;
                GetComponent<NavMeshAgent>().speed = currentMoveSpeed;
                StopCoroutine("SearchForPlayer");
            }
            else
            {
                Debug.Log("searching");
                if (isInFov)
                {
                    Debug.Log("found player again");
                    foundPlayer = true;
                    isSearching = false;
                    GetComponent<NavMeshAgent>().speed = currentMoveSpeed;
                    StopCoroutine("SearchForPlayer");
                }
                yield return new WaitForSeconds(4.5f);

                GetComponent<NavMeshAgent>().speed = currentMoveSpeed;
                PatrolChoose();
                Debug.Log("not found");
                isSearching = false;
                StopCoroutine("SearchForPlayer");
            }
        }
    }

    void CheckIfGamePaused()
    {
        
        if (player.GetComponent<OtherPlayerFunctions>().isPaused)
        {
            GetComponent<NavMeshAgent>().speed = 0;
            GetComponent<Animator>().speed = 0f;
        }
   
    }

    public void ResetFOV()
    {
        isInFov = false;
    }

}
