

public struct CameraLine {
    public static CameraLine Empty = new CameraLine(null, 0);
    public RayCastHit? Hit { get; } = null;
    public double LineLengthPercentage { get; }

    public CameraLine(RayCastHit? hit, double lineLengthPercentage) {
        if (hit.HasValue) {
            this.Hit = hit.Value;
        }
        this.LineLengthPercentage = lineLengthPercentage;
    }
}
