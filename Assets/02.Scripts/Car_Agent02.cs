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

        public List<Transform> checkPoints = new List<Transform>();

        [SerializeField]
        private int nextIdx = 0;




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

            
            
            tr.parent.Find("GoodSpace").GetComponentsInChildren<Transform>(checkPoints);

          /*  for (int i = 0; i < 13; i++)
            {
                tr.parent.Find("GoodSpace" + i).GetComponentsInChildren<Transform>(checkPoints);
            }
*/
        }

        public override void OnEpisodeBegin()
        {
            stageManager.InitStage();

            nextIdx = 0;
            // 물리력을 초기화
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            tr.eulerAngles = new Vector3(0, -180, 0);

            // 에이젼트의 위치를 불규칙하게 변경
            tr.localPosition = new Vector3(Random.Range(origin.x - 3.0f, origin.x + 3.0f), origin.y
                                           ,
                                           Random.Range(origin.z - 3.0f, origin.z + 3.0f));

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

            /*           //INGOODSPACE 태그를 가진 녀석들을 배열에 담고
                       ChPoint = GameObject.FindGameObjectsWithTag("INGOODSPACE");
                       for (int i = 0; i < ChPoint.Length; i++)
                       {
                           //배열에 담긴 오브젝트들의 좌표를 얻어낸다.
                           Vector3 checkPoint = ChPoint[i].transform.position;


                           //float 거리비교 = Vector3.Distance(checkPoint, tr.position);
                           //그 좌표 중 가장 가까이 있는 친구를 타겟으로 잡고
                           float shortDis = Vector3.Distance(gameObject.transform.position, ChPoint[0].transform.position);

                           target = ChPoint[0];

                           foreach (GameObject found in ChPoint)
                           {

                               float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);

                               if (Distance < shortDis) // 위에서 잡은 기준으로 거리 재기
                               {
                                   target = found;
                                   shortDis = Distance;
                                   //타겟 제일 가까운에 잡기

                                   //그좌표와 카에이전트의 좌표가 일치하면 I를 1상승시킨다.
                                   Vector3 checkforward = ChPoint[i].transform.forward;
                                   float dirDot = Vector3.Dot(tr.forward, checkforward);
                                   sensor.AddObservation(dirDot);
                                   AddReward(dirDot * 0.002f);
                                   print(dirDot);
                               }


                           }


                       }*/
        }



        //Vector3 checkPointPosition = GameObject.FindGameObjectWithTag("INGOODSPACE").transform.localPosition;





        /*  Vector3 dirPosition = checkPointPosition - tr.localPosition;
          float positionLength = Vector3.Magnitude(dirPosition);
          print(positionLength+"랭스가 이만해");
          if (positionLength <= 2)
          {
              print("add먹었다.");
              AddReward(0.002f);
          }*/






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



            if (nextIdx > 1 && nextIdx % 2 == 0 )
            {

                AddReward(1.0f);
            }

            if (StepCount == 1000 && nextIdx > 3)
            {
                AddReward(0.001f);

            } else if (StepCount == 5000 && nextIdx > 6)
            {
                AddReward(0.01f);

            } else if (StepCount == 8000 && nextIdx > 9)
            {
                AddReward(0.1f);
            }
            else if (StepCount == 11000 && nextIdx > 12)
            {
                AddReward(0.5f);
            }
            else if (StepCount == 14000 && nextIdx > 15)
            {
                AddReward(0.7f);
            }
            else if (StepCount == 14000 && nextIdx > 18)
            {
                AddReward(0.8f);
            }
            // 전진 방향 
            float direction = Vector3.Dot(tr.forward, checkPoints[nextIdx].forward);
            SetReward(direction * 0.001f);

       /*     print(tr.localPosition);
            print(checkPoints[nextIdx+1].localPosition+"체크포인트와의 거리");

            float 너와나의연결거리 = Vector3.Distance(tr.localPosition ,checkPoints[nextIdx+1].localPosition);
            if (너와나의연결거리 >= 100)
            {
                print(너와나의연결거리+"너와나의 격차다");
                SetReward(-너와나의연결거리 * 0.0001f);
            }*/

            m_Car.Move(h, v, v, 0f);

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
            if (coll.collider.CompareTag("CAR"))
            {
                AddReward(-1.0f);
                EndEpisode();
            }
            if (coll.collider.CompareTag("WALL"))
            {
                AddReward(-0.3f);

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
                ++nextIdx;
            }
        }

        private void OnCollisionStay(Collision collision)
        {

            if (collision.collider.CompareTag("FLOOR"))
            {

                AddReward(-0.001f);
            }
            if(collision.collider.CompareTag("WALL"))
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


