public enum Cardinal {
    NORTH, SOUTH, EAST, WEST
}

public struct RayCastHit {
    public Vector2Double Position { get; }
    public Cardinal Side { get; }
    public IHittable HitObject { get; }

    public RayCastHit(Vector2Double position, Cardinal side, IHittable hitObject) {
        this.Position = position;
        this.Side = side;
        this.HitObject = hitObject;
    }
}