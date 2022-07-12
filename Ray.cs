using System;

public class Ray {
    private Vector2Double origin;
    private Vector2Double direction;
    private Action<Vector2Double> OnHit;

    public Ray(Vector2Double origin, Vector2Double direction, Action<Vector2Double> onHit) {
        this.origin = origin;
        this.direction = direction;
        this.OnHit += onHit;
    }

    public void Cast(int distance, Map map) {
        Vector2Double? firstRowHit = null;
        Vector2Double? firstColHit = null;

        Vector2Double currentRowPosition = this.origin;
        while (firstRowHit == null && Vector2Double.Distance(currentRowPosition, this.origin) < distance  && map.InBounds(currentRowPosition) ) {
            if (map.At((int)currentRowPosition.row, (int)currentRowPosition.col) is IHittable) {
                firstRowHit = currentRowPosition;
                break;
            }
            double nextRow = currentRowPosition.row + (direction.row < 0 ? -1 : 1);
            currentRowPosition += Vector2Double.AlterToRow(direction, nextRow - currentRowPosition.row); 
        }

        Vector2Double currentColPosition = this.origin;
        while (firstColHit == null && Vector2Double.Distance(currentColPosition, this.origin) < distance && map.InBounds(currentColPosition) ) {
            if (map.At((int)currentColPosition.row, (int)currentColPosition.col) is IHittable) {
                firstColHit = currentColPosition;
                break;
            }
            double nextCol = currentColPosition.col + (direction.col < 0 ? -1 : 1);
            currentColPosition += Vector2Double.AlterToCol(direction, nextCol - currentColPosition.col); 
        }

        if (firstRowHit == null && firstColHit != null) {
            OnHit(firstColHit.Value);
        } else if (firstRowHit != null && firstColHit == null) {
            OnHit(firstRowHit.Value);
        } else if (firstRowHit != null && firstColHit != null) {
            if (Vector2Double.Distance(firstRowHit.Value, this.origin) < Vector2Double.Distance(firstColHit.Value, this.origin)) {
                OnHit(firstRowHit.Value);
            } else {
                OnHit(firstColHit.Value);
            }
        }
    }
}