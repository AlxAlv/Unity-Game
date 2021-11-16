using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTarget : EntityComponent
{
    [SerializeField] public PossibleTargetHelper PossibleTargetHelper;

    public GameObject CurrentTarget { get; set; }
    public GameObject _potentialTarget { get; set; }
    
    [SerializeField] private LineRenderer _lineRenderer;

    private GameObject _currentObjectUnderMouse { get; set; }
    private bool HasCancelled = true;
    private bool HasMentionedSKey = false;

    protected override void Start()
    {
        _lineRenderer = GameObject.FindWithTag("AimingLine").GetComponent<LineRenderer>();
        PossibleTargetHelper = GameObject.FindWithTag("UIManager").GetComponent<PossibleTargetHelper>();

        CurrentTarget = null;
        HasCancelled = true;

        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (_potentialTarget != null && !_potentialTarget.activeSelf)
        {
            _potentialTarget = null;
        }

        if (CurrentTarget == null && !HasCancelled)
        {
            //HasCancelled = true;
            //m_entityWeapon.CancelAllSkills();

            if (Input.GetKey(KeyCode.S))
                FindNearesetEnemy();
        }

        if (_potentialTarget != null && Input.GetKey(KeyCode.S))
            Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), _potentialTarget.transform.position, Color.green);

        UpdateLine();
        HightlightUnderCursor();
        UpdatePlayerHighlight();
    }

    private void UpdatePlayerHighlight()
    {
        if (RaycastHelper.Instance.IsPlayerUnderCursor())
            GetComponent<OutlineHelper>().SetOutlineAmount(0.005f);
        else
            GetComponent<OutlineHelper>().SetOutlineAmount(0.0f);
    }

    private void HightlightUnderCursor()
    {
        GameObject enemyUnderCursor = RaycastHelper.Instance.GetEnemyUnderCursor();

        if (enemyUnderCursor != null)
        {
            if ((enemyUnderCursor != _currentObjectUnderMouse) && (_currentObjectUnderMouse != null))
            {
                ChangeGameObjectSelections(_currentObjectUnderMouse, false);
            }

            _currentObjectUnderMouse = enemyUnderCursor;
            ChangeGameObjectSelections(_currentObjectUnderMouse, true);
        }
        else if (_currentObjectUnderMouse != null && (_currentObjectUnderMouse != _potentialTarget && _currentObjectUnderMouse != CurrentTarget))
        {
            ChangeGameObjectSelections(_currentObjectUnderMouse, false);
            _currentObjectUnderMouse = null;
        }
    }

    private void UpdateLine()
    {
        if (_lineRenderer != null)
        {
            if (_potentialTarget != null && Input.GetKey(KeyCode.S))
            {
                _lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                _lineRenderer.SetPosition(1, _potentialTarget.transform.position);
            }
            else
            {
                _lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
                _lineRenderer.SetPosition(1, new Vector3(0, 0, 0));
            }
        }
    }

    protected override void HandleComponent()
    {
        base.HandleComponent();
    }

    public bool IsTargettingEnemy()
    {
        return (_potentialTarget != null && Input.GetKey(KeyCode.S));
    }

    private void FindNearesetEnemy()
    {
        if (_potentialTarget == null)
        {
            List<TargetHelper> enemyEntities = new List<TargetHelper>();
            List<TargetHelper> visibleEnemies = new List<TargetHelper>();
            TargetHelper[] currentEntities = FindObjectsOfType(typeof(TargetHelper)) as TargetHelper[];

            // Find all enemies
            foreach (var entity in currentEntities)
            {
                enemyEntities.Add(entity);
            }

            // Weed out enemies not on the screen
            foreach (var entity in enemyEntities)
            {
                Vector3 screenPoint = Camera.main.WorldToViewportPoint(entity.transform.position);
                bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

                if (onScreen)
                    visibleEnemies.Add(entity);
            }

            // Find the closest enemy
            float closestDistance = 9000;
            foreach (var entity in visibleEnemies)
            {
                var distanceFromTarget = Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), entity.transform.position);

                if (distanceFromTarget < closestDistance)
                {
                    if (_potentialTarget != CurrentTarget)
                        ChangeGameObjectSelections(_potentialTarget, false);

                    //We have a new closest target.
                    _potentialTarget = entity.gameObject;

                    ChangeGameObjectSelections(_potentialTarget, true);

                    closestDistance = distanceFromTarget;
                }
            }
        }
    }

    protected override void HandleInput()
    {
        if (Input.GetKey(KeyCode.S))
        {
            FindNearesetEnemy();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            if (_potentialTarget != CurrentTarget)
                ChangeGameObjectSelections(_potentialTarget, false);

            _potentialTarget = null;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeGameObjectSelections(CurrentTarget, false);
            CurrentTarget = null;
            HasCancelled = true;
        }

        GameObject target = _potentialTarget;
        if (target == null)
            target = PossibleTargetHelper.CurrentTarget;

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && target != null)
        {
            ChangeGameObjectSelections(CurrentTarget, false);
            CurrentTarget = target;
            HasCancelled = false;
            ChangeGameObjectSelections(CurrentTarget, true);

            if (!HasMentionedSKey && !Input.GetKey(KeyCode.S))
            {
	            HasMentionedSKey = true;
	            DialogManager.Instance.InstantSystemMessage("Hold the [S] key instead of having to manually hover over targets!");
            }
        }
    }

    private void ChangeGameObjectSelections(GameObject gameObject, bool targetted)
    {
        if (targetted)
        {
            ChangeGameObjectOutline(gameObject, 0.005f);
            ChangeGameObjectInfo(gameObject, true);
        }
        else
        {
            ChangeGameObjectOutline(gameObject, 0.0f);
            ChangeGameObjectInfo(gameObject, false);
        }
    }

    private void ChangeGameObjectOutline(GameObject gameObject, float amount)
    {
        if (gameObject != null && gameObject.GetComponent<OutlineHelper>() != null)
            gameObject.GetComponent<OutlineHelper>().SetOutlineAmount(amount);
    }

    private void ChangeGameObjectInfo(GameObject gameObject, bool show)
    {
        if (gameObject != null && gameObject.GetComponent<EntityInfo>() != null)
            gameObject.GetComponent<EntityInfo>().SetShowInfo(show);
    }
}
