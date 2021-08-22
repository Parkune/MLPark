/*using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static CarController;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    private Vector3 originalPosition;

    private BehaviorParameters behaviorParameters;

    private CarController carController;

    private Rigidbody carControllerRigidBody;


    public override void Initialize()
    {
        originalPosition = transform.localPosition;
        behaviorParameters = GetComponent<BehaviorParameters>();
        carController = GetComponent<CarController>();
        carControllerRigidBody = carController.GetComponent<Rigidbody>();

        ResetParkingLotArea();    
    }

    public override void OnEpisodeBegin()
    {
        ResetParkingLotArea();
    }

    private void ResetParkingLotArea()
    {
        // important to set car to automonous during default behavior
        carController.IsAutonomous = behaviorParameters.BehaviorType == BehaviorType.Default;
        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
        carControllerRigidBody.velocity = Vector3.zero;
        carControllerRigidBody.angularVelocity = Vector3.zero;

        // reset which cars show or not show

    }

    void Update()
    {
        if(transform.localPosition.y <= 0)
        {
            TakeAwayPoints();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.rotation);

        sensor.AddObservation(carControllerRigidBody.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.DiscreteActions;
        Vector3 dir = Vector3.zero;

        Vector3 rot = Vector3.zero;
        var direction = Mathf.FloorToInt(action[0]);

        switch (direction)
        {
            case 0: // idle
                carController.CurrentDirection = Direction.Idle;
                break;
            case 1: // forward
                carController.CurrentDirection = Direction.MoveForward;
                break;
            case 2: // backward
                carController.CurrentDirection = Direction.MoveBackward;
                break;
            case 3: // turn left
                carController.CurrentDirection = Direction.TurnLeft;
                break;
            case 4: // turn right
                carController.CurrentDirection = Direction.TurnRight;
                break;
        }

        AddReward(-1f / MaxStep);
    }

    public void GivePoints(float amount = 1.0f, bool isFinal = false)
    {
        AddReward(amount);

        if(isFinal)
        {

            EndEpisode();
        }
    }

    public void TakeAwayPoints()
    {

        AddReward(-0.01f);
        
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;
        actions.Clear();

        if (Input.GetKey(KeyCode.UpArrow))
        {
            actions[0] = 1;
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            actions[0] = 2;
        }

        if(Input.GetKey(KeyCode.LeftArrow) && carController.canApplyTorque())
        {
            actions[0] = 3;
        }

        if(Input.GetKey(KeyCode.RightArrow) && carController.canApplyTorque())
        {
            actions[0] = 4;
        }
    }
}
*/