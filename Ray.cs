using System;

public class Ray {
    private Vector2Double origin;
    private Vector2Double direction;
    private event Action<RayCastHit> OnHit;
    private event Action? OnNoHit;

    public Ray(Vector2Double origin, Vector2Double direction, Action<RayCastHit> onHit) {
        this.origin = origin;
        this.direction = direction;
        this.OnHit += onHit;
    }

    public Ray(Vector2Double origin, Vector2Double direction, Action<RayCastHit> onHit, Action onNoHit) {
        this.origin = origin;
        this.direction = direction.normalized();
        this.OnHit += onHit;
        this.OnNoHit += onNoHit;
    }

    // public void Cast(int distance, Map map) {
    //     RayCastHit? hit = null;
    //     Vector2Double currentPosition = this.origin;
    //     double directionAngle = direction.ToAngle();

    //     while ( hit == null && Vector2Double.Distance(currentPosition, origin) < distance && map.InBounds(currentPosition) ) {
    //     }

    //     if (hit.HasValue) {
    //         OnHit?.Invoke(hit.Value);
    //     } else {
    //         OnNoHit?.Invoke();
    //     }
    // }

    public void Cast(int distance, Map map) {
        RayCastHit? firstRowHit = null;
        RayCastHit? firstColHit = null;

        Vector2Double currentRowPosition = this.origin;
        int rowStepDirection = direction.row < 0 ? -1 : 1;
        while (firstRowHit == null && Vector2Double.Distance(currentRowPosition, this.origin) < distance  && map.InBounds(currentRowPosition) ) {
            double nextRow = rowStepDirection > 0 ? Math.Floor(currentRowPosition.row + rowStepDirection) : Math.Ceiling(currentRowPosition.row + rowStepDirection);
            currentRowPosition += direction.AlterToRow(nextRow - currentRowPosition.row); 
            Vector2Int tileToCheck = rowStepDirection > 0 ? currentRowPosition.Int() : new Vector2Int((int)Math.Round(currentRowPosition.row + rowStepDirection), (int)currentRowPosition.col);
            if (map.InBounds(tileToCheck)) {
                Tile tile = map.At(tileToCheck);
                if (tile.canBeHit()) {
                    firstRowHit = new RayCastHit(currentRowPosition, direction.row <= 0 ? Cardinal.NORTH : Cardinal.SOUTH, tile);
                    break;
                }
            }
        }

        Vector2Double currentColPosition = this.origin;
        int colStepDirection = direction.col < 0 ? -1 : 1;
        while (firstColHit == null && Vector2Double.Distance(currentColPosition, this.origin) < distance && map.InBounds(currentColPosition) ) {
            double nextCol = colStepDirection > 0 ? Math.Floor(currentColPosition.col + colStepDirection) : Math.Ceiling(currentColPosition.col + colStepDirection);
            currentColPosition += direction.AlterToCol(nextCol - currentColPosition.col);

            Vector2Int tileToCheck = colStepDirection > 0 ? currentColPosition.Int() : new Vector2Int((int)currentColPosition.row, (int)Math.Round(currentColPosition.col + colStepDirection));
            if (map.InBounds(tileToCheck) ) {
                Tile tile = map.At(tileToCheck);
                if (tile.canBeHit()) {
                    firstColHit = new RayCastHit(currentColPosition, direction.col <= 0 ? Cardinal.WEST : Cardinal.EAST, tile);
                    break;
                }
            }
        }


        if (!firstRowHit.HasValue && !firstColHit.HasValue) {
            OnNoHit?.Invoke();
        } else if (!firstRowHit.HasValue && firstColHit.HasValue) {
            OnHit(firstColHit.Value);
        } else if (firstRowHit.HasValue && !firstColHit.HasValue) {
            OnHit(firstRowHit.Value);
        } else if (firstRowHit.HasValue && firstColHit.HasValue) {
            if (Vector2Double.Distance(firstRowHit.Value.Position, this.origin) <= Vector2Double.Distance(firstColHit.Value.Position, this.origin)) {
                OnHit(firstRowHit.Value);
            } else {
                OnHit(firstColHit.Value);
            }
        }


    }
}