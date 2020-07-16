using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<Vector3> _path;
    [SerializeField] private float _minDinstanceToPoint = 0.1f;

    private float _distanceToPoint;
    private Vector3 _startPosition;
    private Vector3 _currentPosition;
    private IEnumerator<Vector3> _currentPoint;
    private bool _gameStarted;

    public Vector3 CurrentPoint => _startPosition + _currentPoint.Current;
    
    private void Start()
    {
        _gameStarted = true;
        _startPosition = transform.position;
        _currentPosition = transform.position;
        _currentPoint = GetPoint();
        _currentPoint.MoveNext();

        transform.position = _currentPosition + _currentPoint.Current;
    }

    private void Update()
    {
        if (_path != null || _path.Count > 0)
        {
            ComputePath();
        }
    }

    private void ComputePath()
    {
        _distanceToPoint = (transform.position - (_currentPosition + _currentPoint.Current)).magnitude;

        if (_distanceToPoint < _minDinstanceToPoint)
        {
            _currentPoint.MoveNext();
        }
    }

    public IEnumerator<Vector3> GetPoint()
    {
        int index = 0;

        while(true)
        {
            yield return _path[index];

            if (_path.Count <= 1)
            {
                continue;
            }

            index++;
            if (index < 0)
            {
                index = _path.Count - 1;
            }
            else if (index > _path.Count - 1)
            {
                index = 0;
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (!_gameStarted && transform.hasChanged)
        {
            _currentPosition = transform.position;
        }

        for (int i = 0; i < _path.Count; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_currentPosition + _path[i], 0.3f);

            if (i < _path.Count - 1)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(_currentPosition  + _path[i], _currentPosition + _path[i + 1]);
            }

            if (i == _path.Count - 1)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(_currentPosition + _path[i], _currentPosition + _path[0]);
            }
        }
    }
}
