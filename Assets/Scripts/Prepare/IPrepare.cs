using System;
using System.Collections;

public interface IPrepare
{
    IEnumerator Prepare(Action<bool, string> onComplete);
}
