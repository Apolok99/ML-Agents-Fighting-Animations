using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

// Class that implements the sword agent
public class SwordDefender : Agent
{
    // --- VARIABLES ---
    // PUBLIC VARIABLES

    public Transform attacker;
    public Transform attackerJoint;

    public float forceMultiplier = 10f;

    public Transform[] goal;

    public Transform trainingAreaAxis;

    public GameObject demoHandler; // can be null

    // PRIVATE VARIABLES
    private Rigidbody rBody;


    // --- METHODS ---
    // PRIVATE METHODS

    // Called when the script instance is being loaded 
    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }


    // PUBLIC METHODS

    // Set up an Agent instance at the beginning of an episode 
    public override void OnEpisodeBegin()
    {
        // Agent's inital positon and velocity
        gameObject.transform.localPosition = new Vector3(1.77999997f, 6.1930995f, 0.180999994f);
        rBody.velocity = Vector3.zero;
        gameObject.transform.localRotation = new Quaternion(0.707106829f, 0, -0.707106829f, 0);
        rBody.angularVelocity = Vector3.zero;

        // Attacker's initial position and velocity
        if (demoHandler == null)
        {
            attacker.GetComponent<MoveAttacker>().PlayNextAnim();
            attacker.localPosition = new Vector3(-0.735548317f, -1.41793692f, 0.0554279089f);
            attacker.localRotation = new Quaternion(0.000568372896f, 4.47034871e-08f, -5.15475813e-06f, 0.999999881f);
        }
        // If it's the "Demo Scene", play only one animation.
        else
        {
            if (demoHandler.GetComponent<HandlerDemo>().startAnimation)
            {
                attacker.GetComponent<MoveAttacker>().PlayNextAnim();
                attacker.localPosition = new Vector3(-0.735548317f, -1.41793692f, 0.0554279089f);
                attacker.localRotation = new Quaternion(0.000568372896f, 4.47034871e-08f, -5.15475813e-06f, 0.999999881f);
            }
            // If it has already played, play the IDLE animation
            else
            {
                attacker.GetComponent<MoveAttacker>().PlayIDLE();
            }    
        }
    }


    // Collect the vector observations of the agent for the step.
    // The agent observation describes the current environment from the perspective of the agent.
    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent's observations
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(rBody.velocity);

        // Attacker's observations
        sensor.AddObservation(trainingAreaAxis.InverseTransformPoint(attackerJoint.position));
        sensor.AddObservation(Quaternion.Inverse(attackerJoint.rotation) * trainingAreaAxis.rotation);
        sensor.AddObservation(attacker.GetComponent<MoveAttacker>().jointVelocity);

        // Goal's observations
        // SPINE
        sensor.AddObservation(trainingAreaAxis.InverseTransformPoint(goal[0].position));

        // HEAD
        sensor.AddObservation(trainingAreaAxis.InverseTransformPoint(goal[1].position));
        sensor.AddObservation(Quaternion.Inverse(goal[1].rotation) * trainingAreaAxis.rotation);

        // LEFT ARM
        sensor.AddObservation(trainingAreaAxis.InverseTransformPoint(goal[2].position));
        sensor.AddObservation(trainingAreaAxis.InverseTransformPoint(goal[3].position));

        // RIGHT ARM
        sensor.AddObservation(trainingAreaAxis.InverseTransformPoint(goal[4].position));
        sensor.AddObservation(trainingAreaAxis.InverseTransformPoint(goal[5].position));

        // LEFT LEG
        sensor.AddObservation(trainingAreaAxis.InverseTransformPoint(goal[6].position));
        sensor.AddObservation(trainingAreaAxis.InverseTransformPoint(goal[7].position));

        // RIGHT LEG
        sensor.AddObservation(trainingAreaAxis.InverseTransformPoint(goal[8].position));
        sensor.AddObservation(trainingAreaAxis.InverseTransformPoint(goal[9].position));

    }

    // Allow the Agent to execute actions based on the ActionBuffers contents.
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Get the action index for movement
        int movementX = actionBuffers.DiscreteActions[0];
        int movementY = actionBuffers.DiscreteActions[1];
        int movementZ = actionBuffers.DiscreteActions[2];

        // Get the action index for rotating
        int rotationX = actionBuffers.DiscreteActions[3];
        int rotationZ = actionBuffers.DiscreteActions[4];

        float dirX = 0f;
        float dirY = 0f;
        float dirZ = 0f;

        float rotX = 0f;
        float rotZ = 0f;

        // Look up the index in the movement action list:
        if (movementX == 1) { dirX = -1f; }
        if (movementX == 2) { dirX = 1f; }

        if (movementY == 1) { dirY = 1f; }
        if (movementY == 2) { dirY = -1f; }

        if (movementZ == 1) { dirZ = -1f; }
        if (movementZ == 2) { dirZ = 1f; }

        // Look up the index in the rotation action list:
        if (rotationX == 1) { rotX = -0.1f; }
        if (rotationX == 2) { rotX = 0.1f; }

        if (rotationZ == 1) { rotZ = -0.1f; }
        if (rotationZ == 2) { rotZ = 0.1f; }


        // Create the action's vectors
        Vector3 movementVector = new Vector3(dirX, dirY, dirZ);
        Vector3 rotationVector = new Vector3(rotX, 0f, rotZ);

        // Apply the action results to move the Agent
        rBody.AddRelativeForce(movementVector * forceMultiplier);
        rBody.AddRelativeTorque(rotationVector, ForceMode.VelocityChange);

        // If the position of the Agents surpase one of this limits, the reward will be -1 and the episode will be over
        if (transform.localPosition.x > 2f || transform.localPosition.x < 0.6f ||
            transform.localPosition.y > 9f || transform.localPosition.y < 5f || 
            transform.localPosition.z > 2f || transform.localPosition.z < -1f ||
            transform.rotation.eulerAngles.z < 85f || transform.rotation.eulerAngles.z > 300f)
        {
            SetReward(-1f);

            // If it's the "Demo Scene", set the animation to false
            if (demoHandler != null)
            {
                demoHandler.GetComponent<HandlerDemo>().startAnimation = false;
            }

            EndEpisode();

            // If it's the "Demo Scene", restart the positions
            if (demoHandler != null)
            {
                demoHandler.GetComponent<HandlerDemo>().Restart();
            }
        }
    }

    // Choose an action for this agent using a custom heuristic.
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        // Initialate at neutral for each discreta action
        discreteActionsOut[0] = 0;
        discreteActionsOut[1] = 0;
        discreteActionsOut[2] = 0;
        discreteActionsOut[3] = 0;
        discreteActionsOut[4] = 0;

        // Input for the movement in X-Axis
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[0] = 2;
        }
        // Input for the movement in Y-Axis
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[1] = 2;
        }
        // Input for the movement in Z-Axis
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[2] = 2;
        }
        // Input for the rotation in X-Axis
        if (Input.GetKey(KeyCode.F))
        {
            discreteActionsOut[3] = 1;
        }
        if (Input.GetKey(KeyCode.G))
        {
            discreteActionsOut[3] = 2;
        }
        // Input for the rotation in Z-Axis
        if (Input.GetKey(KeyCode.C))
        {
            discreteActionsOut[4] = 1;
        }
        if (Input.GetKey(KeyCode.V))
        {
            discreteActionsOut[4] = 2;
        }
    }

    // OnTriggerEnter happens on the FixedUpdate function when two GameObjects collide.
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            SetReward(-1f);

            // If it's the "Demo Scene", set the animation to false
            if (demoHandler != null)
            {
                demoHandler.GetComponent<HandlerDemo>().startAnimation = false;
            }

            EndEpisode();

            // If it's the "Demo Scene", restart the positions
            if (demoHandler != null)
            {
                demoHandler.GetComponent<HandlerDemo>().Restart();
            }
        }
    }
}
