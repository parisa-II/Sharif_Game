using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Saver;
    //public Transform[] Poses;
    public Transform[] StartPoses;
    public Transform[] StartFallPoses;

    private int TotalBlockNum;

    private int[] ValidStartIndeses;

    public Transform BlockPref;
    private Transform Block;
    private int MaxRange = 30 + 1;
    private int MinRange = 1;
    private float Time;
    private float CreateTime;
    private int StarterCounter;
    private float TempTime;

    void Start()
    {
        Time = 0f;
        TempTime = 0f;
        CreateTime = 2.5f;
        StarterCounter = 0;
        StartCoroutine(CreateBlock1());
        
        StartCoroutine(Timer());
        //  FirstSpawner();
        //StartCoroutine(CreateBlock2());
        //SecondSpawner();
    }

    public void CreateNewBlock(int index)
    {
        StartCoroutine(CreateBlock(index));
        
    }

    private void FirstSpawner()
    {
        for (int i = 0; i < StartPoses.Length; i++) //ValidNum * ValidNum
        {
            //int _index = i;
            Block = Instantiate(BlockPref, StartPoses[i].transform.position, BlockPref.rotation);
            Block.SetParent(StartPoses[i], false);
            Block.SetParent(Saver.transform, true);
            Vector3 temp_pos = Block.transform.localPosition;
            temp_pos.z = 0f;
            Block.transform.localPosition = temp_pos;
            Block.GetComponent<BlockScript>().Index = i;
            Block.gameObject.SetActive(true);
            Block.gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = Random.Range(MinRange, MaxRange).ToString();
        }
    }

    private void FallerSpawner()
    {
        for (int i = 0; i < StartFallPoses.Length; i++) //ValidNum * ValidNum
        {
            //int _index = i;
            Block = Instantiate(BlockPref, StartFallPoses[i].transform.position, BlockPref.rotation);
            Block.SetParent(StartFallPoses[i], false);
            Block.SetParent(Saver.transform, true);
            Vector3 temp_pos = Block.transform.localPosition;
            temp_pos.z = 0f;
            Block.transform.localPosition = temp_pos;
            Block.GetComponent<BlockScript>().Index = i;
            Block.gameObject.SetActive(true);
            Block.gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = Random.Range(MinRange, MaxRange).ToString();
        }
    }

    private void SecondFallerSpawner()
    {
        int rand = Random.Range(0, StartFallPoses.Length);
        Block = Instantiate(BlockPref, StartFallPoses[rand].transform.position, BlockPref.rotation);
        Block.SetParent(StartFallPoses[rand], false);
        Block.SetParent(Saver.transform, true);
        Vector3 temp_pos = Block.transform.localPosition;
        temp_pos.z = 0f;
        Block.transform.localPosition = temp_pos;
        Block.gameObject.SetActive(true);
        Block.gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = Random.Range(MinRange, MaxRange).ToString();
    }

    private void SecondSpawner()
    {
        int[] _indexes = new int[StartPoses.Length];
        for (int i = 0; i < _indexes.Length; i++)
            _indexes[i] = i;
        Shuffle(_indexes);

        for (int i = 0; i < _indexes.Length; i++)
        {
            print("1");
            if (StartPoses[_indexes[i]].GetComponentInChildren<BlockScript>() == null)
            {
                print("2");
                Block = Instantiate(BlockPref, StartPoses[_indexes[i]].transform.position, BlockPref.rotation);
                Block.SetParent(StartPoses[_indexes[i]], false);
                Block.SetParent(Saver.transform, true);
                Vector3 temp_pos = Block.transform.localPosition;
                temp_pos.z = 0f;
                Block.transform.localPosition = temp_pos;
                Block.gameObject.SetActive(true);
                Block.gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = Random.Range(MinRange, MaxRange).ToString();

                break;
            }
        }
    }

    private void Shuffle(int[] _indexes)
    {
        for(int i = 0; i < _indexes.Length; i++)
        {
            int temp = _indexes[i];
            int rand = Random.Range(0, _indexes.Length);
            _indexes[i] = _indexes[rand];
            _indexes[rand] = temp;
        }
    }

    IEnumerator CreateBlock(int index)
    {
        yield return new WaitForSeconds(10f);

        Block = Instantiate(BlockPref, StartPoses[index].transform.position, BlockPref.rotation);
        Block.SetParent(StartPoses[index], false);
        Block.SetParent(Saver.transform, true);
        Vector3 temp_pos = Block.transform.localPosition;
        temp_pos.z = 0f;
        Block.transform.localPosition = temp_pos;
        Block.GetComponent<BlockScript>().Index = index;
        Block.gameObject.SetActive(true);
        Block.gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = Random.Range(MinRange, MaxRange).ToString();
    }

    IEnumerator CreateBlock1()
    {
        yield return new WaitForSeconds(0.3f);
        FallerSpawner();
        StarterCounter++;
        if(StarterCounter < 3)
            StartCoroutine(CreateBlock1());
        else
            StartCoroutine(CreateBlock2());
    }

    IEnumerator CreateBlock2()
    {
        print(Fire.HoldFire);
        yield return new WaitForSeconds(0.2f);
        if (Fire.HoldFire)
            TempTime += 0.2f;
        if (TempTime >= CreateTime)
        {
            SecondFallerSpawner();
            TempTime = 0f;
        }

        StartCoroutine(CreateBlock2());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.2f);

        if (Fire.HoldFire)
            Time += 0.2f;

        print(Time);

        switch (Time)
        {
            case 20:
                CreateTime = 2.25f;
                MaxRange = 35 + 1;
                MinRange = 15;
                break;
            case 40:
                CreateTime = 2f;
                MaxRange = 45 + 1;
                MinRange = 25;
                break;
            case 60:
                CreateTime = 1.75f;
                MaxRange = 55 + 1;
                MinRange = 35;
                break;
            case 80:
                CreateTime = 1.25f;
                MaxRange = 65 + 1;
                MinRange = 45;
                break;
            case 100:
                CreateTime = 1f;
                MaxRange = 75 + 1;
                MinRange = 55;
                break;
        }
        StartCoroutine(Timer());
    }
}
