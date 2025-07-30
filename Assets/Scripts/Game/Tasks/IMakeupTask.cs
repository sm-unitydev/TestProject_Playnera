using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMakeupTask
{
    event System.Action<bool> OnFinish;

    IEnumerator Start();
}
