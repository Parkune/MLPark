using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManeger : MonoBehaviour
{

    //배열에 피벗 좌표(고정 좌표)의 위치를 확인하고 싶다.
    public List<Transform> parkingarea = new List<Transform>();
    public int parkingareacount;
    public Transform pullSpaceTr;
    public GameObject car;

    int carCount = 0;
    public int maxCarCount = 1;

    // Start is called before the first frame update
    void Start()
    {

        //InitStage();
        
    }
    public void InitStage()
    {
        parkingarea.Clear();
        carCount = 0;

        foreach (var parkObject in transform.Find("ParkZone").GetComponentsInChildren<Transform>())
        {
            parkingarea.Add(parkObject);
        }
        parkingarea.RemoveAt(0);
        parkingareacount = parkingarea.Count;

        // 기존에 생성된 PullSpace 삭제
        foreach (Transform obj in pullSpaceTr.transform)
        {
            Destroy(obj.gameObject);
        }


        for (int i = 0; i < parkingareacount; i++)
        {
            int carindex = Random.Range(0, parkingarea.Count);

            var pullSpace = Instantiate(car, parkingarea[carindex].position, parkingarea[carindex].rotation, pullSpaceTr);
            pullSpace.name = "PullSpace";

            parkingarea.RemoveAt(carindex);
            if (++carCount >= maxCarCount)
            {
                break;
            }
        }

    }

    private void Update()
    {

    }


    // Update is called once per frame

}
