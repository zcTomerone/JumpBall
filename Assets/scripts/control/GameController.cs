using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int difficulty = 1;
    public GameObject loopPrefab;
    public GameObject cylinderPrefab;
    public float loopGap = 8.0f;
    public int loopCount = 10;
    public float cylinderCount = 3;
    public float cylinderHeight = 16.0f;
    private int oldDifficulty = 1;
    public int[] difficultyScoreStage = new int[10];
 
    GameObject ball;
    ScoreCounter scoreCounter;
    LinkedList<GameObject> loopList;
    LinkedList<GameObject> cylinderList;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("Ball");
        InitializeLoop();
        InitializeCylinder();
        scoreCounter = GameObject.Find("GameController").GetComponent<ScoreCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        AutoChangeDIfficulty();
        UpdateCylinder();
        UpdateLoop();
        oldDifficulty = difficulty;
    }
    void InitializeLoop()
    {
        loopList = new LinkedList<GameObject>();
        Transform ballTrans = ball.transform;
        Vector3 ballPos = ballTrans.position;
        Vector3 tmpLoopRotation = Vector3.zero;
        Vector3 tmpLoopPos = Vector3.zero;
        for (int i = 0; i < loopCount; i++)
        {
            //tmpLoopPos.y = ballPos.y + (-loopCount / 2 - 1 + i) * loopGap;
            tmpLoopPos.y = ballPos.y +(-loopCount  + i) * loopGap;
            tmpLoopRotation.y = Random.Range(0.0f, 360.0f);
            GameObject tmpLoop = Instantiate(loopPrefab, tmpLoopPos, Quaternion.Euler(tmpLoopRotation));
            tmpLoop.transform.Find("DeathLoop").GetComponent<LoopMesh>().randomRotate();
            loopList.AddFirst(tmpLoop);
        }
    }
    void InitializeCylinder()
    {
        cylinderList = new LinkedList<GameObject>();
        Transform ballTrans = ball.transform;
        Vector3 ballPos = ballTrans.position;
        Quaternion CylinderRotation = Quaternion.Euler(Vector3.zero);
        Vector3 tmpCylinderPos = Vector3.zero;
        for (int i = 0; i < cylinderCount; i++)
        {
            tmpCylinderPos.y = ballPos.y + (-cylinderCount / 2 - 1 + i) * cylinderHeight;
            GameObject tmpCylinder = Instantiate(cylinderPrefab, tmpCylinderPos, CylinderRotation);
            Vector3 scale = tmpCylinder.transform.localScale;
            tmpCylinder.transform.localScale = new Vector3(scale.x, cylinderHeight / 2, scale.z);
            cylinderList.AddFirst(tmpCylinder);
        }
    }
    void UpdateCylinder()
    {
        Transform ballTrans = GameObject.Find("Ball").transform;
        Vector3 ballPos = ballTrans.position;
        var cylinderNode = cylinderList.First;
        while (cylinderNode != null && cylinderNode.Value.transform.position.y - ballPos.y >= 2*cylinderHeight )
        {
            var tmpLast = cylinderList.Last;
            cylinderList.RemoveFirst();
            Vector3 newPos = new Vector3(0, tmpLast.Value.transform.position.y - cylinderHeight, 0);
            cylinderNode.Value.transform.position = newPos;
            cylinderList.AddLast(cylinderNode);
            cylinderNode = cylinderList.First;
        }
    }
    void UpdateLoop()
    {
        Transform ballTrans = GameObject.Find("Ball").transform;
        Vector3 ballPos = ballTrans.position;
        var loopNode = loopList.First;
        Vector3 tmpLoopRotation = Vector3.zero;
        while (loopNode != null && loopNode.Value.transform.position.y - ballPos.y >= 2*loopGap )
        {
            var tmpLast = loopList.Last;
            loopList.RemoveFirst();
            Vector3 newPos = new Vector3(0, tmpLast.Value.transform.position.y - loopGap, 0);
            tmpLoopRotation.y = Random.Range(0.0f, 360.0f);
            if(DifficultyChanged(loopNode))
            {
                Regenerate(loopNode);
            }
            loopNode.Value.transform.position = newPos;
            loopNode.Value.transform.rotation = Quaternion.Euler(tmpLoopRotation);
            loopNode.Value.transform.Find("DeathLoop").GetComponent<LoopMesh>().randomRotate();
            loopList.AddLast(loopNode);
            loopNode = loopList.First;
        }
    }
    void Regenerate(LinkedListNode<GameObject> loopNode)
    {
        loopNode.Value.transform.Find("Loop").gameObject.GetComponent<LoopMesh>().GeneratePieByDifficulty();
        //loopNode.Value.GetComponent<LoopMesh>().GeneratePieByDifficulty();
        loopNode.Value.transform.Find("DeathLoop").gameObject.GetComponent<LoopMesh>().GeneratePieByDifficulty();
    }
    bool DifficultyChanged(LinkedListNode<GameObject> node)
    {
        if(difficulty!=node.Value.transform.Find("DeathLoop").gameObject.GetComponent<LoopMesh>().loopDifficulty)
        { 
            return true;
        }
        return false;
    }
    void AutoChangeDIfficulty()
    {
        if (difficulty >= 10)
            return;
        else
        {
            if(scoreCounter.score>difficultyScoreStage[difficulty])
            {
                difficulty++;
            }
        }
    }
}
