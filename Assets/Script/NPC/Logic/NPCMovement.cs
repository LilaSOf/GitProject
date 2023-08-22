using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.AStar;
using System;
using MFarm.TimeManage;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
public class NPCMovement : MonoBehaviour
{
    // Start is called before the first frame update

    //��ʱ����
   [SerializeField] private string currentScene;
    private string targetScene;

    private Vector3Int currentGridPostion;//��ǰ����������
    private Vector3Int targetGridPostion;//Ŀ������������
    private Vector3Int nextGridPostion;//��һ������������

    public ScheduleData_SO scheduleData;
    private SortedSet<NPCDetails> scheduleSet;
    private NPCDetails currentSchedule;

    //�ƶ���ʵʱʱ��
    public TimeSpan GameTime => TimeManage.Instance.GameTime;
    public string StartScene { set => currentScene = value; }

    [Header("�ƶ�����")]
    public float speed = 2f;
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    private Vector2 dir;

    public bool isMoving;

    //�����ȡ
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D colli;
    private Rigidbody2D rb;
    private Animator animator;
    private Grid grid;

    private Stack<MovementStep> movementSteps = new Stack<MovementStep>();

    private bool isInit;//NPC�Ƿ��Ѿ�����ʼ��

    private string SceneName;//��ȡ��ǰ����������

    private bool npcMove;//�ж�npc�Ƿ����ƶ�

    private Vector3 nextWorldPos;//npc����������

    private bool sceneLoad;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colli = GetComponent<BoxCollider2D>();

        movementSteps = new Stack<MovementStep>();
    }

    private void OnEnable()
    {
        EventHandler.AfterFade += OnAfetFade;
        EventHandler.BeforeFade += OnBeforeFade;
    }
    private void OnDisable()
    {
        EventHandler.AfterFade -= OnAfetFade;
        EventHandler.BeforeFade -= OnBeforeFade;
    }
    private void FixedUpdate()
    {
        if(!sceneLoad)
          Movement();
    }
    private void OnAfetFade(string sceneName)
    {
        sceneLoad = false;
        grid = FindObjectOfType<Grid>();
        CheckVisable(sceneName);
        SceneName = sceneName;
        if (!isInit)
        {
            InitNPC();
            isInit = true;
        }
    }
    private void OnBeforeFade(string obj)
    {
        sceneLoad = true;
    }

    private void InitNPC()
    {
        targetScene = currentScene;
        //�������������ĵ��λ��
        currentGridPostion = grid.WorldToCell(transform.position);
        transform.position = new Vector3(currentGridPostion.x + Settings.gridCellSize /2, currentGridPostion.y+ Settings.gridCellSize/2,transform.position.z);

        targetGridPostion = currentGridPostion;
    }
    private void Movement()
    {
        if (!npcMove)
        {

            if (movementSteps.Count > 0)
            {
                MovementStep step = movementSteps.Pop();
                currentScene = step.sceneName;
                nextGridPostion = (Vector3Int)step.gridCoordinate;
                CheckVisable(SceneName);
                TimeSpan stepTime = new TimeSpan(step.hour, step.minute, step.second);
                MoveToGridPosition(nextGridPostion, stepTime);
            }
        }
    }

    private void MoveToGridPosition(Vector3Int gridPosition,TimeSpan timeSpan)
    {
        StartCoroutine(MoveRounite(gridPosition, timeSpan));
    }
    private IEnumerator MoveRounite(Vector3Int gridPos,TimeSpan timeSpan)
    {
        npcMove = true;
        nextWorldPos = GetWorldPos(gridPos);
        float distance = Vector3.Distance(nextWorldPos, transform.position);
        float timeToMove = (float)(timeSpan.TotalSeconds - GameTime.TotalSeconds);
        float Speed = Mathf.Max(minSpeed, distance / timeToMove / Settings.secondTreahsHold);

        if(Speed< maxSpeed)
        {
            while(Vector3.Distance(transform.position, nextWorldPos) > Settings.pixelSize)
            {
                dir =  (nextWorldPos - transform.position).normalized;
                Vector2 posOffset = new Vector2(dir.x * Speed *Time.fixedDeltaTime, dir.y * Speed * Time.fixedDeltaTime);
                rb.MovePosition((Vector2)transform.position + posOffset);
                yield return new WaitForFixedUpdate();
            }
        }
        rb.position = nextWorldPos;
        currentGridPostion = gridPos;
        nextWorldPos = currentGridPostion;
        npcMove = false;
    }


    private void CheckVisable(string sceneName)
    {
        if(currentScene == sceneName)
        {
            SetActiveInScene();
        }
        else
        {
            SetInactiveInScene();
        }
    }

    /// <summary>
    /// ����Schedule����·��
    /// </summary>
    /// <param name="scheduleDetails"></param>
    public void  BulidPath(NPCDetails scheduleDetails)
    {
        movementSteps.Clear();
        currentSchedule = scheduleDetails;

        if(scheduleDetails.targetName == currentScene)
        {
            AStar.Instance.BuildPath(scheduleDetails.targetName,(Vector2Int)currentGridPostion, (Vector2Int)scheduleDetails.targetGridPosition, movementSteps);
        }

        if(movementSteps .Count > 1) 
        {
            //����ÿһ����Ӧʱ���
            UpdateTimeOnPath();
        }
    }
    private void UpdateTimeOnPath()
    {
        MovementStep previousStep = null;
        TimeSpan currentGameTime = GameTime;

        foreach(MovementStep step in movementSteps)
        {
            if(previousStep == null)
                previousStep = step;
            step.hour = currentGameTime.Hours;
            step.minute = currentGameTime.Minutes;
            step.second = currentGameTime.Seconds;

            TimeSpan gridMovementStepTime;
            if (MoveInDiagonal(step,previousStep))
                gridMovementStepTime = new TimeSpan(0, 0, (int)(Settings.gridCellDiagonalSize / speed / Settings.secondTreahsHold));
            else
                gridMovementStepTime = new TimeSpan(0,0,(int)(Settings.gridCellSize/speed/Settings.secondTreahsHold));
            //�ۼӻ����һ����ʱ��
            currentGameTime.Add(gridMovementStepTime);

            //��ǰ��һ��
            previousStep = step;
        }
    }

   /// <summary>
   /// ���񷵻����������е�
   /// </summary>
   /// <param name="gridPosition"></param>
   /// <returns></returns>
   private Vector3 GetWorldPos(Vector3Int gridPosition)
    {
        Vector3 worldPos = grid.CellToWorld(gridPosition);
        return new Vector3(worldPos.x + Settings.gridCellSize/2, worldPos.y + Settings.gridCellSize/2);
    }

    private bool MoveInDiagonal(MovementStep currentStep,MovementStep previousStep)
    {
        return (currentStep.gridCoordinate.x != previousStep.gridCoordinate.x) &&(currentStep.gridCoordinate.y != previousStep.gridCoordinate.y);
    }
    #region ����NPC��ʾ���
    private void SetActiveInScene()
    {
        spriteRenderer.enabled = true;
        colli.enabled = true;

        //TODO:Ӱ�ӿ���
        //transform.GetChild(0).gameObject.SetActive(true);
    }

    private void SetInactiveInScene()
    {
        spriteRenderer.enabled = false;
        colli.enabled = false;

        //TODO:Ӱ�ӿ���
        //transform.GetChild(0).gameObject.SetActive(false);
    }
    #endregion
}
