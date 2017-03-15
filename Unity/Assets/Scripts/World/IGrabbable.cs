using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IGrabbable
{

    bool CanGrab();

    void OnGrab();

}
