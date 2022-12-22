public enum Cardinal {
    NORTH, SOUTH, EAST, WEST
}

public struct RayCastHit {
    public Vector2Double Position { get; }
    public Cardinal Side { get; }
    public Tile HitObject { get; }

    public RayCastHit(Vector2Double position, Cardinal side, Tile hitObject) {
        if (!hitObject.canBeHit()) {
            throw new ArgumentException("Provided a non-hittable object to a Raycast Hit");
        }
        
        this.Position = position;
        this.Side = side;
        this.HitObject = hitObject;
    }
}