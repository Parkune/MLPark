using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarAgent2))]
public class Car_Agent02 : Agent
    {

        private CarAgent2 m_Car;

        private StageManeger stageManager;

        private Transform tr;
        private Transform targetTr;
        private Rigidbody rb;

        private Vector3 origin;


        public void Awake()
        {
         
            // get the car controller
            m_Car = GetComponent<CarAgent2>();

        }


        // Start is called before the first frame update
        void Start()
        {

            origin = transform.localPosition;

        }
        public override void Initialize()
        {


            MaxStep = 10000;
            tr = GetComponent<Transform>();
            rb = GetComponent<Rigidbody>();
            stageManager = tr.parent.GetComponent<StageManeger>();

        }

        public override void OnEpisodeBegin()
        {
            stageManager.InitStage();

            // 물리력을 초기화
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // 에이젼트의 위치를 불규칙하게 변경
            tr.localPosition = new Vector3(Random.Range(origin.x - 3.0f, origin.x + 3.0f), origin.y
                                           ,
                                           Random.Range(origin.z - 3.0f, origin.z+3.0f));

        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var actions = actionsOut.ContinuousActions;
            // pass the input to the car!

            actions[0] = Input.GetAxis("Horizontal");
            actions[1] = Input.GetAxis("Vertical");
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(tr.localPosition);
            sensor.AddObservation(rb.velocity.x);
            sensor.AddObservation(rb.velocity.y);
        }




        public override void OnActionReceived(ActionBuffers actions)
        {
            var action = actions.ContinuousActions;
           // Vector3 dir = Vector3.zero;

         /*   Vector3 rot = Vector3.zero;
            Vector3 dir = (Vector3.forward * action[0]) + (Vector3.right * action[1]);
            rb.AddForce(dir.normalized * 50.0f);*/

            float h = action[0];
            float v = action[1];


            SetReward(-0.001f);

            m_Car.Move(h * 2.0f, v*2.0f, v * 2.0f, 0f);

        }


 /*      private void FixedUpdate()
        {
            // pass the input to the car!
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
#if !MOBILE_INPUT
            float handbrake = Input.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }*/
        private void OnCollisionEnter(Collision coll)
        {
            if (coll.collider.CompareTag("PERFECTSPACE"))
            {
                AddReward(+1.0f);
                EndEpisode();
                
             

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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BADSPACE"))
            {
                AddReward(-0.5f);
            }
            if (other.CompareTag("INGOODSPACE"))
            {
                AddReward(+0.2f);

            }
        }

    }
  }

