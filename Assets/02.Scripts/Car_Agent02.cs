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
        //private Vector3 vector;

        public GameObject startLine;

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
            tr.eulerAngles = new Vector3(0, -180, 0);

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
            //sensor.AddObservation(vector);
            Vector3 checkPoint = GameObject.FindGameObjectWithTag("INGOODSPACE").transform.forward;
            //Vector3 checkPointPosition = GameObject.FindGameObjectWithTag("INGOODSPACE").transform.localPosition;
            float dirDot = Vector3.Dot(tr.forward , checkPoint);
            sensor.AddObservation(dirDot);
            AddReward(dirDot * 0.002f);
          //  print(dirDot);

          /*  Vector3 dirPosition = checkPointPosition - tr.localPosition;
            float positionLength = Vector3.Magnitude(dirPosition);
            print(positionLength+"랭스가 이만해");
            if (positionLength <= 2)
            {
                print("add먹었다.");
                AddReward(0.002f);
            }*/
            


        }


        public void FixedUpdate()
        {
            Vector3 vector = transform.position;

        }

        /*   Vector3 checkStartLine = startLine.transform.position;

        Vector3 dir = transform.position - checkStartLine;

        print(dir + "스타트라인과의 거리");

        if (dir.x <= -1)
        {
            AddReward(-0.001f);
            print(dir + "10m 이내에 위치해있음");
        }*/




        public override void OnActionReceived(ActionBuffers actions)
        {
            var action = actions.ContinuousActions;
           Vector3 dir = Vector3.zero;
           Vector3 rot = Vector3.zero;

           /* Vector3 dir = (Vector3.forward * action[0]) + (Vector3.right * action[1]);
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
                AddReward(-0.1f);

            }
            if (coll.collider.CompareTag("BADSPACE"))
            {
                AddReward(-0.05f);
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
                AddReward(+0.8f);
                other.gameObject.SetActive(false);

            }
        }

        private void OnCollisionStay(Collision collision)
        {
            
            if(collision.gameObject.CompareTag("FLOOR"))
            {
                
                AddReward(-0.01f);
            }


        }



        private void Update()
        {
           // print(GetCumulativeReward());
        }
    }


  }

