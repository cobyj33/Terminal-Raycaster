using System;
using System.Threading;

public abstract class UserThread {
    Thread thread;
    public static event Action OnProgramEnd;
    public event Action OnThreadStart;
    public static void EndProgram() {
        OnProgramEnd?.Invoke();
    }
    protected bool forceStopped;

    public UserThread() {
        OnProgramEnd += () => forceStopped = true;
        thread = new Thread(ThreadLoop);
    }

    public void StopThread() {
        forceStopped = true;
    }
    public void StartThread() {
        if (!thread.IsAlive) {
            OnThreadStart?.Invoke();
            thread.Start();
        }
    }

    protected abstract void ThreadLoop();
}