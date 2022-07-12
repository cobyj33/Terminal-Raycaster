using System;
using System.ComponentModel;
using System.Threading;
using System.Collections.Concurrent;

public class EventSystem : UserThread {
    IEnumerable<KeyAction> keyActions = new List<KeyAction>();
    public static EventSystem instance = new EventSystem();
    private EventSystem(  ) : base() {  }
    protected override void ThreadLoop() {
        Thread.CurrentThread.Name = "Event Thread";
        while (!forceStopped) {
            ConsoleKeyInfo currentKey = Console.ReadKey(true);
            foreach (KeyAction keyAction in keyActions) {
                if (keyAction.Matches(currentKey)) {
                    keyAction.Fire();
                }
            }
        }
        forceStopped = false;
    }

    public void AddAction(KeyAction action) {
        List<KeyAction> newList = new List<KeyAction>(keyActions);
        newList.Add(action);
        keyActions = newList;
    }

    public void RemoveAction(KeyAction action) {
        List<KeyAction> newList = new List<KeyAction>(keyActions);
        newList.Remove(action);
        keyActions = newList;
    }

    public void ClearActions() {
        keyActions = new List<KeyAction>();
    }
}