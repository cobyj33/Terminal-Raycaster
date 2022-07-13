using System;

struct Angle {
    public const double RadiansToDegrees = 180.0 / Math.PI;
    public const double DegreesToRadians = Math.PI / 180.0;

    private double radians = 0;
    private double degrees = 0;
    public double Radians {
        get => radians;
        set {
            radians = value;
            degrees = value * RadiansToDegrees;
        }
    }

    public double Degrees {
        get => degrees;
        set {
            degrees = value;
            radians = value * DegreesToRadians;
        }
    }

    public Angle() { }

    public Angle(double? radians, double? degrees) {
        if (radians.HasValue) {
            Radians = radians.Value;
       } else if (degrees.HasValue) {
            Degrees = degrees.Value;
       }
    }

    public static Angle FromDegrees(double degrees) {
        return new Angle(null, degrees);
    }

    public static Angle FromRadians(double radians) {
        return new Angle(radians, null);
    }

    public override string ToString()
    {
        return $"[Angle]: [Radians: {radians}, Degrees: {degrees}]";
    }

}