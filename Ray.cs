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
        int rowStepDirection = direction.row < 0 ? -1 : 1;
        while (firstRowHit == null && Vector2Double.Distance(currentRowPosition, this.origin) < distance  && map.InBounds(currentRowPosition) ) {
            // Console.WriteLine(currentRowPosition);
            Vector2Int tileToCheck = rowStepDirection > 0 ? new Vector2Int((int)currentRowPosition.row, (int)currentRowPosition.col) : new Vector2Int((int)(currentRowPosition.row) - 1, (int)currentRowPosition.col);
            if (map.InBounds(tileToCheck)) {
                Tile tile = map.At(tileToCheck);
                if (tile is IHittable) {
                    firstRowHit = new RayCastHit(currentRowPosition, direction.row <= 0 ? Cardinal.NORTH : Cardinal.SOUTH, tile as IHittable);
                    break;
                }
            }
            double nextRow = Math.Floor(currentRowPosition.row + rowStepDirection);
            currentRowPosition += direction.AlterToRow(nextRow - currentRowPosition.row); 
        }

        Vector2Double currentColPosition = this.origin;
        int colStepDirection = direction.col < 0 ? -1 : 1;
        while (firstColHit == null && Vector2Double.Distance(currentColPosition, this.origin) < distance && map.InBounds(currentColPosition) ) {
            // Console.WriteLine(currentColPosition);
            Vector2Int tileToCheck = colStepDirection > 0 ? new Vector2Int((int)currentColPosition.row, (int)currentColPosition.col) : new Vector2Int((int)currentColPosition.row, (int)(currentColPosition.col) - 1);
            if (map.InBounds(tileToCheck) ) {
                Tile tile = map.At(tileToCheck);
                if (tile is IHittable) {
                    firstColHit = new RayCastHit(currentColPosition, direction.col <= 0 ? Cardinal.WEST : Cardinal.EAST, tile as IHittable);
                    break;
                }
            }

            double nextCol = Math.Floor(currentColPosition.col + colStepDirection);
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