using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : Singleton<DialogManager>
{
    [SerializeField] TextMeshProUGUI _systemTextDisplay;
    [SerializeField] float _typingSpeed;
    [SerializeField] float _displayTime;

    public bool HeldHostage = false;
    private Vector2 _originalSize;
    private List<Coroutine> _pendingMessages = new List<Coroutine>();

    private void Start()
    {
        _originalSize = _systemTextDisplay.gameObject.GetComponent<RectTransform>().sizeDelta;
    }

    private void Update()
    {
        if (_systemTextDisplay.text == "")
            _systemTextDisplay.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        else
            _systemTextDisplay.gameObject.GetComponent<RectTransform>().sizeDelta = _originalSize;
    }

    public void AddSystemMessage(string message)
    {
        if (!HeldHostage)
            _pendingMessages.Add(StartCoroutine(UpdateSystemText(message)));
    }

    public void SetText(string message)
    {
        _systemTextDisplay.text = message;
    }

    public void InstantSystemMessage(string message)
    {
        if (!HeldHostage)
        {
            foreach (var msg in _pendingMessages)
                StopCoroutine(msg);

            _systemTextDisplay.text = "";
            _pendingMessages.Add(StartCoroutine(UpdateSystemText(message)));
        }
    }

    IEnumerator UpdateSystemText(string message)
    {
        while (_systemTextDisplay.text.Length != 0)
            yield return null;

        foreach (char letter in message)
        {
            _systemTextDisplay.text += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }

        yield return new WaitForSeconds(_displayTime);
        _systemTextDisplay.text = "";
    }
}
