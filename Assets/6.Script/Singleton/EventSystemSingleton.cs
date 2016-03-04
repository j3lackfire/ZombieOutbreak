/// <summary>
/// Event system singleton.
/// There is only one event system needed for each scene.
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent (typeof(EventSystem),typeof(StandaloneInputModule),typeof(TouchInputModule))]
public class EventSystemSingleton : Singleton<EventSystemSingleton> {

}
