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
    public Transform BallPref;
    public Transform BombPref;
    public Transform BombExplotionPref;
    private Transform Block;
    private Transform Ball;
    private Transform Bomb;
    private Transform BombExplotion;
    private int MaxRange = 30 + 1;
    private int MinRange = 1;
    private float Time;
    private float CreateTime;
    private int StarterCounter;
    private float TempTime;
    private int Increaser_P;
    private int Bomb_P;
    private int Ball_P;
    private int blockFallerCounter;
    //private bool ActiveBallBlock;
    //private bool ActiveBombBlock;

    void Start()
    {
        Time = 0f;
        TempTime = 0f;
        CreateTime = 2f;
        StarterCounter = 0;
        MinRange = 1;
        blockFallerCounter = 1;
        MaxRange = 12;
        Increaser_P = 0;
        Bomb_P = 0;
        Ball_P = 0;
        BlockScript.IncreaseRate = 0.5f;

        //ActiveBallBlock = false;
        //ActiveBombBlock = false;

        StartCoroutine(CreateBlock1());
        
        //StartCoroutine(Timer());
        //  FirstSpawner();
        //StartCoroutine(CreateBlock2());
        //SecondSpawner();
    }

    public void CreateNewBlock()
    {
        MaxRange = 12 * LvlSceneManager.FireRatio;
        CreateTime -= 0.2f;// * LvlSceneManager.FireRatio;
        if (CreateTime < 1)
            CreateTime = 1;

        Increaser_P = 10 + 5 * (LvlSceneManager.FireRatio - 1);
        if (Increaser_P > 35)
            Increaser_P = 35;

        BlockScript.IncreaseRate = 0.5f - 0.05f * (LvlSceneManager.FireRatio - 1);
        if (BlockScript.IncreaseRate < 0.25f)
            BlockScript.IncreaseRate = 0.25f;

        if(LvlSceneManager.FireRatio >= 3)
        {
            Bomb_P = 10 + 5 * (LvlSceneManager.FireRatio - 3);

            if (Bomb_P > 30)
                Bomb_P = 30;
        }

        if (LvlSceneManager.FireRatio >= 5)
        {
            Ball_P = 10 + 5 * (LvlSceneManager.FireRatio - 5);

            if (Ball_P > 25)
                Ball_P = 25;
        }

        if (LvlSceneManager.FireRatio > 16)
            blockFallerCounter = 2;

        StarterCounter = 0;
        StartCoroutine(CreateBlock1());      
    }

    public void SetBombExplotion(Transform pos)
    {
        BombExplotion = Instantiate(BombExplotionPref, pos.position, BombExplotionPref.rotation);
        BombExplotion.SetParent(pos, false);
        BombExplotion.SetParent(Saver.transform, true);
        Vector3 temp_pos = BombExplotion.transform.localPosition;
        temp_pos.z = 0f;
        BombExplotion.transform.localPosition = temp_pos;
        BombExplotion.gameObject.SetActive(true);
        Destroy(BombExplotion.gameObject, 0.125f);
    }

    public void SetBall(Transform pos)
    {
        Ball = Instantiate(BallPref, pos.position, BallPref.rotation);
        Ball.SetParent(pos, false);
        Ball.SetParent(Saver.transform, true);
        Vector3 temp_pos = Ball.transform.localPosition;
        temp_pos.z = 0f;
        Ball.transform.localPosition = temp_pos;
        Ball.gameObject.SetActive(true);
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

            if(LvlSceneManager.FireRatio >= 12) //
            {
                int rand_increaser = Random.Range(0, 100);
                if (rand_increaser < 25) //Increaser_P
                {
                    Block.tag = "IncreaserBlock";
                }
            }

            LvlSceneManager.StartNewWave = true;
            LvlSceneManager.BlockCounter++;
        }
    }

    private void SecondFallerSpawner()
    {
        int[] _indexs = new int[blockFallerCounter];
        int rand = Random.Range(0, StartFallPoses.Length);
        _indexs[0] = rand;
        rand = Random.Range(0, StartFallPoses.Length);
        while(rand == _indexs[0])
        {
            rand = Random.Range(0, StartFallPoses.Length);
        }
        _indexs[1] = rand;

        for (int i = 0; i < blockFallerCounter; i++)
        {       
            Block = Instantiate(BlockPref, StartFallPoses[_indexs[i]].transform.position, BlockPref.rotation);
            Block.SetParent(StartFallPoses[_indexs[i]], false);
            Block.SetParent(Saver.transform, true);
            Vector3 temp_pos = Block.transform.localPosition;
            temp_pos.z = 0f;
            Block.transform.localPosition = temp_pos;
            Block.gameObject.SetActive(true);
            Block.gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = Random.Range(MinRange, MaxRange).ToString();

            int rand_increaser = Random.Range(0, 100);
            if (rand_increaser < Increaser_P)
            {
                Block.tag = "IncreaserBlock";
            }

            int rand_bomb = Random.Range(0, 100);
            if (rand_bomb < Bomb_P)
            {
                Block.tag = "Bomb";
                Block.gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = "1";
            }

            int rand_Ball = Random.Range(0, 100);
            if (rand_Ball < Ball_P)
            {
                Block.tag = "BallBlock";
            }

            LvlSceneManager.StartNewWave = true;
            LvlSceneManager.BlockCounter++;
        }
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
                LvlSceneManager.StartNewWave = true;
                LvlSceneManager.BlockCounter++;

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
        LvlSceneManager.StartNewWave = true;
        LvlSceneManager.BlockCounter++;
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
        yield return new WaitForSeconds(0.2f);
        if (Fire.HoldFire)
            TempTime += 0.2f;
        if (TempTime >= CreateTime && !LvlSceneManager.EndOfWave)//
        {
            SecondFallerSpawner();
            TempTime = 0f;
        }

        if(!LvlSceneManager.EndOfWave)
            StartCoroutine(CreateBlock2());
    }

    //IEnumerator Timer()
    //{
    //    yield return new WaitForSeconds(0.2f);

    //    if (Fire.HoldFire)
    //        Time += 0.2f;

    //    switch (Time)
    //    {
    //        case 20:
    //            CreateTime = 2.25f;
    //            MaxRange = 35 + 1;
    //            MinRange = 15;
    //            break;
    //        case 40:
    //            CreateTime = 2f;
    //            MaxRange = 45 + 1;
    //            MinRange = 25;
    //            break;
    //        case 60:
    //            CreateTime = 1.75f;
    //            MaxRange = 55 + 1;
    //            MinRange = 35;
    //            break;
    //        case 80:
    //            CreateTime = 1.25f;
    //            MaxRange = 65 + 1;
    //            MinRange = 45;
    //            break;
    //        case 100:
    //            CreateTime = 1f;
    //            MaxRange = 75 + 1;
    //            MinRange = 55;
    //            break;
    //    }
    //    StartCoroutine(Timer());
    //}
}
