using System.Collections;
using UnityEngine;

// Class that handles the movement of the Attacker's GameObject
public class MoveAttacker : MonoBehaviour
{
    // --- VARIABLES ---
    // PUBLIC VARIABLES
    public Animator attackerAnimator;
    public Transform attackerModel;

    public GameObject defender;

    public Transform jointPosition;
    [HideInInspector] public Vector3 lastPosition;
    [HideInInspector] public Vector3 jointVelocity;

    [HideInInspector] public int numAnim;

    public GameObject demoHandler;


    // PRIVATE VARIABLES
    private bool alreadyHit;

    // --- METHODS ---
    // PRIVATE METHODS

    // Start is called before the first frame update
    private void Start()
    {
        numAnim = 0;
        alreadyHit = false;
        lastPosition = jointPosition.position;
    }

    // Called every physics timestep.
    private void FixedUpdate()
    {
        Vector3 actualPosition = jointPosition.position;
        jointVelocity = (actualPosition - lastPosition) / Time.fixedDeltaTime;

        lastPosition = actualPosition;

        if(jointVelocity.x > 100f  || jointVelocity.x < -100f || 
            jointVelocity.y > 100f || jointVelocity.y < -100f || 
            jointVelocity.z > 100f || jointVelocity.z < -100f)
        {
            jointVelocity = Vector3.zero;
        }
    }

    // PUBLIC METHODS

    // Reproduce the next animation for the training
    public void PlayNextAnim()
    {
        // First, rebind all the animated properties and mesh data with the Animator.
        attackerAnimator.Rebind();

        // Create the information for the next animation
        numAnim++;
        string nameAnim = "Attack" + numAnim;

        //Debug.Log(nameAnim);

        // Move the Attacker according to the animation to play
        switch (numAnim)
        {
            case 1:
                attackerModel.transform.localPosition = new Vector3(5.8f, 0f, -1.42f);
                attackerModel.transform.rotation = new Quaternion(0, -0.56640625f, 0, 0.824126244f);
                break;
            case 2:
                attackerModel.transform.localPosition = new Vector3(12f, 0f, 0f);
                attackerModel.transform.rotation = new Quaternion(0, -0.707106829f, 0, 0.707106829f);
                break;
            case 3:
                attackerModel.transform.localPosition = new Vector3(7f, 0f, 0f);
                break;
            case 4:
                attackerModel.transform.localPosition = new Vector3(8f, 0f, 0f);
                break;
            case 5:
                attackerModel.transform.localPosition = new Vector3(7f, 0f, 0f);
                break;
            case 6:
                attackerModel.transform.localPosition = new Vector3(8f, 0f, 0f);
                break;
            case 7:
                attackerModel.transform.localPosition = new Vector3(7f, 0f, 0f);
                break;
            case 8:
                attackerModel.transform.localPosition = new Vector3(9f, 0f, 0f);
                break;
            case 9:
                attackerModel.transform.localPosition = new Vector3(9f, 0f, 0f);
                break;
            case 10:
                attackerModel.transform.localPosition = new Vector3(8f, 0f, 0f);
                break;
            case 11:
                attackerModel.transform.localPosition = new Vector3(8f, 0f, 0f);
                break;
            case 12:
                attackerModel.transform.localPosition = new Vector3(8f, 0f, 0f);
                break;
            case 13:
                attackerModel.transform.localPosition = new Vector3(7f, 0f, 0f);
                break;
            case 14:
                attackerModel.transform.localPosition = new Vector3(10f, 0f, 0f);
                break;
        }

        // If it's the last animation, begin again
        if (numAnim == 14)
        {
            numAnim = 0;
        }

        // Play the animation
        attackerAnimator.Play(nameAnim);
    }

    // Reproduce the "IDLE" animation
    public void PlayIDLE()
    {
        attackerAnimator.Play("IDLE");
        attackerModel.localPosition = new Vector3(9f, 0, 0);
        attackerModel.rotation = new Quaternion(0, -0.707106829f, 0, 0.707106829f);
    }

    // OnTriggerEnter happens on the FixedUpdate function when two GameObjects collide.
    public void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Goal" || other.gameObject.tag == "GoalHand") && !alreadyHit)
        {
            alreadyHit = true;  
            defender.GetComponent<SwordDefender>().SetReward(-1f);

            if (demoHandler != null)
            {
                demoHandler.GetComponent<HandlerDemo>().startAnimation = false;
            }
            
            defender.GetComponent<SwordDefender>().EndEpisode();

            if (demoHandler != null)
            {
                demoHandler.GetComponent<HandlerDemo>().Restart();
            }

            StartCoroutine(WaitEndEpisode());
        }
    }

    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
    public void OnCollisionEnter(Collision collision)
    {
        // Check if the collider has been already activated
        if (!alreadyHit)
        {
            // Check which collider has been activated.
            Collider myCollider = collision.GetContact(0).thisCollider;

            // If it has collided with the grip of the attacker's sword, bad
            if (myCollider.gameObject.tag == "Grip")
            {
                alreadyHit = true;

                if (demoHandler != null)
                {
                    demoHandler.GetComponent<HandlerDemo>().startAnimation = false;
                }

                defender.GetComponent<SwordDefender>().EndEpisode();

                if (demoHandler != null)
                {
                    demoHandler.GetComponent<HandlerDemo>().Restart();
                }

                StartCoroutine(WaitEndEpisode());
            }
            // If it has collided with the fuller of the attacker's sword, good
            else if (myCollider.gameObject.tag == "Attacker")
            {
                alreadyHit = true;

                // Check if the cilinder has collided with the grip or the fuller of the sword
                float distanceGrip = Vector3.Distance(defender.transform.GetChild(1).position, collision.GetContact(0).point);
                float distanceFuller = Vector3.Distance(defender.transform.GetChild(2).position, collision.GetContact(0).point);

                // If it has collided with the grip, bad
                if (distanceGrip <= distanceFuller)
                {
                    
                    defender.GetComponent<SwordDefender>().SetReward(-1f);

                    if (demoHandler != null)
                    {
                        demoHandler.GetComponent<HandlerDemo>().startAnimation = false;
                    }

                    defender.GetComponent<SwordDefender>().EndEpisode();

                    if (demoHandler != null)
                    {
                        demoHandler.GetComponent<HandlerDemo>().Restart();
                    }
                    StartCoroutine(WaitEndEpisode());
                }
                // If it has collided with the fuller, good
                else
                {
                    float distanceDefender = Vector3.Distance(collision.GetContact(0).point, collision.gameObject.transform.GetChild(0).position);
                    float distanceAttacker = Vector3.Distance(collision.GetContact(0).point, transform.GetChild(0).position);

                    float rewardDefender = (1f - (Mathf.Clamp(distanceDefender, 0f, 3f) / 3f)) / 2f;
                    float rewardAttacker = (1f - (Mathf.Clamp(distanceAttacker, 0f, 3f) / 3f)) / 2f;

                    float totalReward = rewardAttacker + rewardDefender;

                    defender.GetComponent<SwordDefender>().SetReward(totalReward);
                    
                    if (demoHandler != null)
                    {
                        GetComponent<AudioSource>().Play();
                        demoHandler.GetComponent<HandlerDemo>().startAnimation = false;
                    }

                    defender.GetComponent<SwordDefender>().EndEpisode();

                    if (demoHandler != null)
                    {
                        demoHandler.GetComponent<HandlerDemo>().Restart();
                    }

                    StartCoroutine(WaitEndEpisode());
                }
            }
        }
    }

    IEnumerator WaitEndEpisode()
    {
        yield return new WaitForSeconds(0.75f);
        alreadyHit = false;
    }
}
