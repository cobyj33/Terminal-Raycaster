using System;

public class Ray {
    private Vector2Double origin;
    private Vector2Double direction;
    private Action<RayCastHit> OnHit;
    private Action OnNoHit;

    public Ray(Vector2Double origin, Vector2Double direction, Action<RayCastHit> onHit) {
        this.origin = origin;
        this.direction = direction;
        this.OnHit += onHit;
    }

    public Ray(Vector2Double origin, Vector2Double direction, Action<RayCastHit> onHit, Action onNoHit) {
        this.origin = origin;
        this.direction = direction;
        this.OnHit += onHit;
        this.OnNoHit = onNoHit;
    }

    public void Cast(int distance, Map map) {
        RayCastHit? firstRowHit = null;
        RayCastHit? firstColHit = null;
        // Console.WriteLine("RAY CAST:  ");

        Vector2Double currentRowPosition = this.origin;
        while (firstRowHit == null && Vector2Double.Distance(currentRowPosition, this.origin) < distance  && map.InBounds(currentRowPosition) ) {
            // Console.WriteLine(currentRowPosition);
            Tile tile = map.At((int)currentRowPosition.row, (int)currentRowPosition.col);
            if (tile is IHittable) {
                firstRowHit = new RayCastHit(currentRowPosition, direction.row <= 0 ? Cardinal.NORTH : Cardinal.SOUTH, tile as IHittable);
                break;
            }
            double nextRow = currentRowPosition.row + (direction.row < 0 ? -1 : 1);
            currentRowPosition += direction.AlterToRow(nextRow - currentRowPosition.row); 
        }

        Vector2Double currentColPosition = this.origin;
        while (firstColHit == null && Vector2Double.Distance(currentColPosition, this.origin) < distance && map.InBounds(currentColPosition) ) {
            // Console.WriteLine(currentColPosition);
            Tile tile = map.At((int)currentColPosition.row, (int)currentColPosition.col);
            if (tile is IHittable) {
                firstColHit = new RayCastHit(currentColPosition, direction.col <= 0 ? Cardinal.WEST : Cardinal.EAST, tile as IHittable);
                break;
            }

            double nextCol = currentColPosition.col + (direction.col < 0 ? -1 : 1);
            currentColPosition += direction.AlterToCol(nextCol - currentColPosition.col);
        }

        if (!firstRowHit.HasValue && !firstColHit.HasValue) {
            OnNoHit?.Invoke();
        } else if (!firstRowHit.HasValue && firstColHit.HasValue) {
            OnHit(firstColHit.Value);
        } else if (firstRowHit.HasValue && !firstColHit.HasValue) {
            OnHit(firstRowHit.Value);
        } else if (firstRowHit.HasValue && firstColHit.HasValue) {
            if (Vector2Double.Distance(firstRowHit.Value.Position, this.origin) < Vector2Double.Distance(firstColHit.Value.Position, this.origin)) {
                OnHit(firstRowHit.Value);
            } else {
                OnHit(firstColHit.Value);
            }
        }


    }
}