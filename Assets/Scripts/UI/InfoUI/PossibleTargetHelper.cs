using System.Collections.Generic;
using UnityEngine;

public class PossibleTargetHelper : MonoBehaviour
{
    [SerializeField] private float _maximumDistanceFromCursor = 5.0f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public GameObject CurrentTarget { get; set; }
    
    [SerializeField] private LineRenderer _lineRenderer;

    private void Start()
    {
        CurrentTarget = null;
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.S))
            FindEnemyNearCursor();

        if (CurrentTarget != null && !Input.GetKey(KeyCode.S))
            Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), CurrentTarget.transform.position, Color.green);

        UpdateLine();
        UpdateKey();
    }

    private void UpdateLine()
    {
        if (_lineRenderer != null)
        {
            if (CurrentTarget != null && !Input.GetKey(KeyCode.S))
            {
                _lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                _lineRenderer.SetPosition(1, CurrentTarget.transform.position);
            }
            else
            {
                _lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
                _lineRenderer.SetPosition(1, new Vector3(0, 0, 0));
            }
        }
    }

    private void UpdateKey()
    {
        if (_spriteRenderer != null)
        {
            if (CurrentTarget != null && !Input.GetKey(KeyCode.S))
            {
                _spriteRenderer.gameObject.SetActive(true);
                _spriteRenderer.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

                HideCursorHelper.Instance.TargetterFlag = false;
            }
            else
            {
                _spriteRenderer.gameObject.SetActive(false);
                HideCursorHelper.Instance.TargetterFlag = true;
            }
        }
    }

    private void FindEnemyNearCursor()
    {
        CurrentTarget = null;

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

            if ((distanceFromTarget < closestDistance) && (distanceFromTarget < _maximumDistanceFromCursor))
            {
                //We have a new closest target.
                CurrentTarget = entity.gameObject;

                closestDistance = distanceFromTarget;
            }
        }
    }
}
