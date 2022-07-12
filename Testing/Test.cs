public enum TestStatus {
    ACCEPTED, FAILED, UNKNOWN
}

public class Test {
    private TestStatus testStatus = TestStatus.UNKNOWN;
    public TestStatus CurrentTestStatus { get => testStatus; }

    private string testName = "Test";
    public string TestName { get => testName; }

    private Func<bool> test;
    private string OnFailMessage { get; set; }
    private string OnSuccessMessage { get; set; }

    public Test(string name, Func<bool> test, string OnFailMessage = "TEST FAILED", string OnSuccessMessage = "TEST SUCCESS") {
        this.testName = name;
        this.test = test;
        this.OnFailMessage = OnFailMessage;
        this.OnSuccessMessage = OnSuccessMessage;
    }

    private string GetTestMessage(string message) {
        return $"[{testName}]: {message}";
    }

    public void Assert() {
        testStatus = test() ? TestStatus.ACCEPTED : TestStatus.FAILED;
        
    }

    public void PrintMessage() {
        switch (testStatus) {
            case TestStatus.ACCEPTED: PrintSuccessMessage(); break;
            case TestStatus.FAILED: PrintErrorMessage(); break;
            default: Console.WriteLine(GetTestMessage("ERROR: UNKNOWN STATUS AFTER ASSERTION (did you call PrintMessage before assertion?)")); break;
        }
    }

    private void PrintErrorMessage() {
        ConsoleColor previousConsoleColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(GetTestMessage(OnFailMessage));
        Console.ForegroundColor = previousConsoleColor;
    }

    private void PrintSuccessMessage() {
        ConsoleColor previousConsoleColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(GetTestMessage(OnSuccessMessage));
        Console.ForegroundColor = previousConsoleColor;
    }
}