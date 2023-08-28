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

    //临时变量
   [SerializeField] private string currentScene;//NPC当前所在的场景
   private string targetScene;

    private Vector3Int currentGridPostion;//当前的网格坐标
    private Vector3Int targetGridPostion;//目标点的网格坐标
    private Vector3Int nextGridPostion;//下一步的网格坐标

    [Header("按照时间表控制行程")]
    public ScheduleData_SO scheduleData;
   [SerializeField] private SortedSet<NPCDetails> scheduleSet = new SortedSet<NPCDetails>();
    private NPCDetails currentSchedule;

    //移动的实时时间
    public TimeSpan GameTime => TimeManage.Instance.GameTime;
    public string StartScene { set => currentScene = value; }

    [Header("移动属性")]
    public float speed = 2f;
    public float minSpeed = 5f;
    public float maxSpeed = 8f;

    private Vector2 dir;

    public bool isMoving;

    //组件获取
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D colli;
    private Rigidbody2D rb;
    private Animator animator;
    private Grid grid;

    private Stack<MovementStep> movementSteps = new Stack<MovementStep>();

    private bool isInit;//NPC是否已经被初始化

    [SerializeField]private string playerSceneName;//获取玩家当前场景的名称

   [SerializeField] private bool npcMove;//判断npc是否在移动

    private Vector3 nextWorldPos;//npc的世界坐标

    private bool sceneLoad;//场景是否在切换
    [Header("动画控制")]
    private AnimatorOverrideController animatorOverride;
    public AnimationClip thinkingClip;//思考动画
    public AnimationClip normalClip;//没有特殊动画
   [SerializeField]private float timeClip;//计时器
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
        //保持再网格中心点的位置
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
                MovementStep step = movementSteps.Pop();//从堆栈中按顺序取出每一步的信息
                currentScene = step.sceneName;
                nextGridPostion = (Vector3Int)step.gridCoordinate;//构建下一步的网格坐标
                if(nextGridPostion.x - currentGridPostion.x >3)
                {
                    transform.position = nextGridPostion;
                }
                CheckVisable();
                TimeSpan stepTime = new TimeSpan(step.hour, step.minute, step.second);
                //NPC走向下一步的方法
                MoveToGridPosition(nextGridPostion, stepTime);
                //设置动画（人物面朝方向）
                animator.SetFloat("DirX", dir.x);
                animator.SetFloat("DirY", dir.y);
                animator.SetBool("Exit", true);
            }
            else if(!isMoving)
            {
                StartCoroutine(SetStopAnimation());//完成移动后设置动画情况
            }
        }
    }

    private void MoveToGridPosition(Vector3Int gridPosition,TimeSpan timeSpan)
    {
        StartCoroutine(MoveRounite(gridPosition, timeSpan));
    }
  /// <summary>
  /// 按照A*算法构建好的路径具体移动的协程
  /// </summary>
  /// <param name="gridPos">下一步的位置</param>
  /// <param name="timeSpan">对应时间</param>
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
    /// 根据Schedule设置路径
    /// </summary>
    /// <param name="scheduleDetails"></param>
    public void  BulidPath(NPCDetails scheduleDetails)
    {
        if(!sceneLoad)
        {
            interactable = scheduleDetails.interactable;
        movementSteps.Clear();
        currentSchedule = scheduleDetails;

            if (scheduleDetails.targetName == currentScene)//当前场景移动
            {
                AStar.Instance.BuildPath(scheduleDetails.targetName, (Vector2Int)currentGridPostion, (Vector2Int)scheduleDetails.targetGridPosition, movementSteps);
            }
            else if (scheduleDetails.targetName != currentScene)//跨场景移动
            {
               // Debug.Log("跨场景移动");
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
            //更新每一步对应时间戳
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
            //累加获得下一步的时间
            currentGameTime.Add(gridMovementStepTime);

            //向前走一步
            previousStep = step;
        }
    }

   /// <summary>
   /// 网格返回世界坐标中点
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
   /// 协程改变人物动画情况
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
    #region 设置NPC显示情况
    private void SetActiveInScene()
    {
        spriteRenderer.enabled = true;
        colli.enabled = true;

        //TODO:影子控制
        //transform.GetChild(0).gameObject.SetActive(true);
    }

    private void SetInactiveInScene()
    {
        spriteRenderer.enabled = false;
        colli.enabled = false;

        //TODO:影子控制
        //transform.GetChild(0).gameObject.SetActive(false);
    }
    #endregion
}
