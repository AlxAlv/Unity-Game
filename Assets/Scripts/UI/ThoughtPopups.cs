using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ThoughtTypes { ExclamationMark, QuestionMark, StandbyMark, BerserkerMark };

public class ThoughtPopups : MonoBehaviour
{
    private static List<int> instanceIds = new List<int>();

    public static ThoughtPopups Create(Vector3 position, int instanceId, ThoughtTypes type)
    {
	    // Constant Strings To Display To The Player
	    Dictionary<ThoughtTypes, string> _thoughtTypeToMark = new Dictionary<ThoughtTypes, string>()
	    {
		    {ThoughtTypes.ExclamationMark, "ExclamationMark"},
		    {ThoughtTypes.QuestionMark, "QuestionMark"},
		    {ThoughtTypes.StandbyMark, "StandbyMark"},
		    {ThoughtTypes.BerserkerMark, "AggroMark"}
	    };

	    string path = "Prefabs/NumberPopups/" + _thoughtTypeToMark[type];

	    if (instanceIds.Contains(instanceId))
        {
            return null;
        }

        instanceIds.Add(instanceId);
        GameObject thoughtPrefab = Instantiate((Resources.Load(path) as GameObject), position, Quaternion.identity);
        ThoughtPopups thought = thoughtPrefab.GetComponent<ThoughtPopups>();
        thought.Setup();

        return thought;
    }

    public static void RemoveInstanceFromList(int instanceId)
    {
        if (instanceIds.Contains(instanceId))
            instanceIds.Remove(instanceId);
    }

    private SpriteRenderer _image;
    private Color _textColor;
    private float _disappearTimer;
    private const float DISAPPEAR_TIMER_MAX = 0.5f;
    private Vector3 moveVector;
    private static int _sortingOrder;

    private void Awake()
    {
         _image = transform.GetComponent<SpriteRenderer>();
        _textColor = _image.color;

        _disappearTimer = DISAPPEAR_TIMER_MAX;
    }

    // Start is called before the first frame update
    private void Setup()
    {
        moveVector = new Vector3(0, 1) * 3f;
        _sortingOrder++;
    }

    private void Update()
    {
        float moveYSpeed = 2f;

        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        _disappearTimer -= Time.deltaTime;

        if (_disappearTimer > DISAPPEAR_TIMER_MAX * .7f)
        {
            // First half of the popup
            float increaseScaleAmount = 1.5f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // Second half
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        if (_disappearTimer < 0)
        {
            // Start Disppearing
            float disappearSpeed = 3f;
            _textColor.a -= disappearSpeed * Time.deltaTime;

            _image.color = _textColor;

            if (_textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
