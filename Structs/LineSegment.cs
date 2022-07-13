struct LineSegment {
    Vector2Double first;
    Vector2Double second;

    public Vector2Double First { get => first; }
    public Vector2Double Second { get => second; }

    public LineSegment(Vector2Double first,  Vector2Double second) {
        this.first = first;
        this.second = second;
    }
}