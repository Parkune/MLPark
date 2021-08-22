using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Manager : MonoBehaviour
{
    public GameObject goodSpace;
    public GameObject badSpace;

    [Range(1, 2)]
    public int goodItemCount = 2;
    [Range(3, 4)]
    public int badItemCount = 4;




    public List<GameObject> goodList = new List<GameObject>();
    public List<GameObject> badList = new List<GameObject>();
    // Start is called before the first frame update

    public void InitStage()
    {
        // ������ �����ƴ� ������ ���� �� List �ʱ�ȭ
        foreach (var obj in goodList)
        {
            Destroy(obj);
        }
        foreach (var obj in badList)
        {
            Destroy(obj);
        }

       


        goodList.Clear();
        badList.Clear();
        //�� ������ ���� ��ġ�� ����
        for (int i = 0; i < goodItemCount; i++)
        {
            
            Vector3 pos = new Vector3(transform.position.x - 10,
                                      0.2f,
                                      Random.Range(-10.0f, 10.0f));
           

            goodList.Add(Instantiate(goodSpace, transform.position + pos,transform.rotation  ,transform));
        }
        //��� ������ ���� ��ġ�� ����
        for (int i = 0; i <badItemCount; i++)
        {
            Vector3 pos = new Vector3(transform.position.x-10,
                                      0.2f,
                                      Random.Range(-10.0f, 10.0f));


            badList.Add(Instantiate(badSpace, transform.position + pos, transform.rotation, transform));
        }

    }
        void Start()
    {
        InitStage();
    }

    // Update is called once per frame

}
