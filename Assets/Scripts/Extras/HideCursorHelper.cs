using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursorHelper : Singleton<HideCursorHelper>
{
    public bool MovementFlag = true;
    public bool TargetterFlag = true;

    // Update is called once per frame
    void Update()
    {
        if (MovementFlag && TargetterFlag)
            Cursor.visible = true;
        else
            Cursor.visible = false;
    }
}
