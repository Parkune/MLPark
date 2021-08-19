using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Car_Agent : Agent
{

    private Stage_Manager stageManager;

    private Transform tr;
    private Transform targetTr;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void Initialize()
    {
        MaxStep = 1000;
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        stageManager = tr.parent.GetComponent<Stage_Manager>();

    }

    public override void OnEpisodeBegin()
    {
        stageManager.InitStage();

        // 물리력을 초기화
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 에이젼트의 위치를 불규칙하게 변경
        tr.localPosition = new Vector3(transform.position.x,
                                       transform.position.y,
                                       Random.Range(-4.0f, 4.0f));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;
        actions.Clear();

        // Branch 0 전진/후진
        if (Input.GetKey(KeyCode.W)) actions[0] = 1;
        if (Input.GetKey(KeyCode.S)) actions[0] = 2;

        // Branch 1 좌/우 이동
        if (Input.GetKey(KeyCode.A)) actions[1] = 1;
        if (Input.GetKey(KeyCode.D)) actions[1] = 2;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.DiscreteActions;
        Vector3 dir = Vector3.zero;
        Vector3 rot = Vector3.zero;

        switch (action[0])
        {
            case 1: dir = tr.forward; break;
            case 2: dir = -tr.forward; break;
        }

        switch (action[1])
        {
            case 1: dir = -tr.right; break;
            case 2: dir = tr.right; break;
        }
        rb.AddForce(dir * 1.0f, ForceMode.VelocityChange);

    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.CompareTag("PERFECTSPACE"))
        {
            AddReward(+1.0f);
            EndEpisode();

        }
        if (coll.collider.CompareTag("GOODSPACE"))
        {
            AddReward(+0.2f);

        }
        if (coll.collider.CompareTag("INGOODSPACE"))
        {
            AddReward(+0.5f);

        }
        if (coll.collider.CompareTag("CAR"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
        if (coll.collider.CompareTag("WALL"))
        {
            AddReward(-0.2f);

        }
        if (coll.collider.CompareTag("BADSPACE"))
        {
            AddReward(-0.5f);
        }


    }

}
