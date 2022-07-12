using System;
using System.ComponentModel;

public class KeyAction {
    public ConsoleKeyInfo Binding { get; set; }
    Action action;
    private AsyncOperation asyncOp;

    public KeyAction(ConsoleKey bindingKey, Action action, bool shift = false, bool alt = false, bool control = false) {
        asyncOp = AsyncOperationManager.CreateOperation(null);
        Binding = new ConsoleKeyInfo((char)bindingKey, bindingKey, shift, alt, control);
        this.action = action;
    }

    public void Fire() {
        asyncOp.Post((o) => action?.Invoke(), EventArgs.Empty);
    }
    
    public bool Matches(ConsoleKeyInfo info) {
        return info.Key == Binding.Key;
    }
}
