using System;
using System.Collections.Generic;

class Tester {
    List<Test> tests = new List<Test>();
    public Tester() { }

    public void RunTests() {
        foreach (Test test in tests) {
            test.Assert();
            test.PrintMessage();
        }
    }

    public void AddTest(Test test) {
        tests.Add(test);
    }

    public void RemoveTest(Test test) {
        tests.Remove(test);
    }

    public bool HasTest(Test test) {
        return tests.Contains(test);
    }
}