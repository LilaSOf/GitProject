using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.AStar;
using System;
using MFarm.TimeManage;
using UnityEditor.Build.Reporting;
using static UnityEditor.VersionControl.Asset;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
public class NPCMovement : MonoBehaviour
{
    // Start is called before the first frame update

    //��ʱ����
   [SerializeField] private string currentScene;//NPC��ǰ���ڵĳ���
   private string targetScene;

    private Vector3Int currentGridPostion;//��ǰ����������
    private Vector3Int targetGridPostion;//Ŀ������������
    private Vector3Int nextGridPostion;//��һ������������

    [Header("����ʱ�������г�")]
    public ScheduleData_SO scheduleData;
   [SerializeField] private SortedSet<NPCDetails> scheduleSet = new SortedSet<NPCDetails>();
    private NPCDetails currentSchedule;

    //�ƶ���ʵʱʱ��
    public TimeSpan GameTime => TimeManage.Instance.GameTime;
    public string StartScene { set => currentScene = value; }

    [Header("�ƶ�����")]
    public float speed = 2f;
    public float minSpeed = 5f;
    public float maxSpeed = 8f;

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

    [SerializeField]private string playerSceneName;//��ȡ��ҵ�ǰ����������

   [SerializeField] private bool npcMove;//�ж�npc�Ƿ����ƶ�

    private Vector3 nextWorldPos;//npc����������

    private bool sceneLoad;//�����Ƿ����л�
    [Header("��������")]
    private AnimatorOverrideController animatorOverride;
    public AnimationClip thinkingClip;//˼������
    public AnimationClip normalClip;//û�����⶯��
   [SerializeField]private float timeClip;//��ʱ��
    public bool interactable;
    //test
    public AStarTest starTest;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colli = GetComponent<BoxCollider2D>();

        targetScene = currentScene;

        animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverride;
        foreach(var schedule in scheduleData.nPCDetails)
        {
             scheduleSet.Add(schedule);
        }

    }
    private void Update()
    {
        
        //Debug.Log(timeClip + Settings.animationChange + "Time: " +Time.time);
    }

    private void OnEnable()
    {
        EventHandler.AfterFade += OnAfetFade;
        EventHandler.BeforeFade += OnBeforeFade;
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
    }
    private void OnDisable()
    {
        EventHandler.AfterFade -= OnAfetFade;
        EventHandler.BeforeFade -= OnBeforeFade;
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
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
        playerSceneName = sceneName;
        CheckVisable();

       // currentScene = sceneName;
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
    private void OnGameMinuteEvent(int minute, int hour, int day, Season season)
    {
        int Time = (hour * 100) + minute;
        NPCDetails matchSchedule = null;
        foreach(var schedule in scheduleSet)
        {
            if (Time == schedule.Time)
            {
                if (day != schedule.day && schedule.day !=0)
                    continue;
                if (season != schedule.season)
                    continue;
                matchSchedule = schedule;
            }
            else if(schedule.Time >Time)
            {
                break;
            }
        }
        if(matchSchedule != null) { BulidPath(matchSchedule); }
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
                MovementStep step = movementSteps.Pop();//�Ӷ�ջ�а�˳��ȡ��ÿһ������Ϣ
                currentScene = step.sceneName;
                nextGridPostion = (Vector3Int)step.gridCoordinate;//������һ������������
                if(nextGridPostion.x - currentGridPostion.x >3)
                {
                    transform.position = nextGridPostion;
                }
                CheckVisable();
                TimeSpan stepTime = new TimeSpan(step.hour, step.minute, step.second);
                //NPC������һ���ķ���
                MoveToGridPosition(nextGridPostion, stepTime);
                //���ö����������泯����
                animator.SetFloat("DirX", dir.x);
                animator.SetFloat("DirY", dir.y);
                animator.SetBool("Exit", true);
            }
            else if(!isMoving)
            {
                StartCoroutine(SetStopAnimation());//����ƶ������ö������
            }
        }
    }

    private void MoveToGridPosition(Vector3Int gridPosition,TimeSpan timeSpan)
    {
        StartCoroutine(MoveRounite(gridPosition, timeSpan));
    }
  /// <summary>
  /// ����A*�㷨�����õ�·�������ƶ���Э��
  /// </summary>
  /// <param name="gridPos">��һ����λ��</param>
  /// <param name="timeSpan">��Ӧʱ��</param>
  /// <returns></returns>
  private IEnumerator MoveRounite(Vector3Int gridPos,TimeSpan timeSpan)
    {
        npcMove = true;
        animator.SetBool("isMoving", true);
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
        animator.SetBool("isMoving", false);
        timeClip = Time.time;
        npcMove = false;
    }


    private void CheckVisable()
    {
        //Debug.Log("IsCheckVisable:   " + playerSceneName + "---CurrentScene:"+currentScene);
        if(playerSceneName == currentScene)
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
        if(!sceneLoad)
        {
            interactable = scheduleDetails.interactable;
        movementSteps.Clear();
        currentSchedule = scheduleDetails;

            if (scheduleDetails.targetName == currentScene)//��ǰ�����ƶ�
            {
                AStar.Instance.BuildPath(scheduleDetails.targetName, (Vector2Int)currentGridPostion, (Vector2Int)scheduleDetails.targetGridPosition, movementSteps);
            }
            else if (scheduleDetails.targetName != currentScene)//�糡���ƶ�
            {
               // Debug.Log("�糡���ƶ�");
                SceneRoute sceneRoute = NPCManage.Instance.GetSceneRouteFormKey(currentScene.Trim(), scheduleDetails.targetName.Trim());
                if (sceneRoute != null)
                {
                    for (int i = 0; i < sceneRoute.scenePaths.Count; i++)
                    {
                        Vector2Int fromPath, goPath;
                        ScenePath path = sceneRoute.scenePaths[i];
                       
                        if (path.fromGridCell.x > Settings.maxCellSize || path.fromGridCell.y > Settings.maxCellSize)
                        {
                            fromPath = (Vector2Int)currentGridPostion;
                        }
                        else
                        {
                            fromPath = path.fromGridCell;
                        }

                        if (path.goToGridCell.x > Settings.maxCellSize || path.goToGridCell.y > Settings.maxCellSize)
                        {
                            goPath = (Vector2Int)scheduleDetails.targetGridPosition;
                        }
                        else
                        {
                            goPath = path.goToGridCell;
                        }
                        targetScene = path.sceneName;
                      //transform.position = (Vector3Int)fromPath;
                        Debug.Log("from:" + fromPath.ToString() + "---to:" + goPath + "targetSceneName"+path.sceneName);
                        AStar.Instance.BuildPath(path.sceneName, fromPath, goPath, movementSteps);
                    }
                }
            }
        }
        if(movementSteps.Count > 1) 
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

   /// <summary>
   /// Э�̸ı����ﶯ�����
   /// </summary>
   /// <returns></returns>
   private IEnumerator SetStopAnimation()
    {
        animator.SetFloat("DirX", 0);
        animator.SetFloat("DirY", -1);
        animator.SetBool("Exit", false);
        if (timeClip + Settings.animationChange <Time.time)
        {
            timeClip = Time.time;
            animatorOverride[normalClip.name] = thinkingClip;
            animator.SetBool("EventAnimation", true);
            yield return null;
            animator.SetBool("EventAnimation", false);
        }
        else
        {
            animatorOverride[thinkingClip.name] = normalClip;
            // animator.SetBool("Exit", true);
        }
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
