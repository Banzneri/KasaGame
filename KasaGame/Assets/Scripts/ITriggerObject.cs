using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerObject<T> {
    void Trigger(T actionObject);
}
