using UnityEngine;
[CreateAssetMenu(fileName = "maze", menuName = "ScriptableObjects/MazeInformation", order = 1)]
internal class MazeInformation : ScriptableDictionary<Vector2, Grid>
{
    private int cols;
    private int rows;
    private int minCol;
    private int maxCol;
    private int minRow;
    private int maxRow;

    public void SetCol(int col)
    {
        this.cols = col;
        this.minCol = -(int)(col / 2);
        this.maxCol = (int)((col - 1) / 2);
    }

    public void SetRow(int row)
    {
        this.rows = row;
        this.minRow = -(int)(row / 2);
        this.maxRow = (int)((row - 1) / 2);
    }
    public int[] GetColInfo()
    {
        int[] colInfo = {cols, minCol, maxCol};
        return colInfo;
        // Debug.Log("Cols: " + cols + " MinCol: " + minCol + " MaxCol: " + maxCol);
    }

    public int[] GetRowInfo()
    {
        int[] rowInfo = {rows, minRow, maxRow};
        return rowInfo;
        // Debug.Log("Rows: " + rows + " MinRow: " + minRow + " MaxRow: " + maxRow);
    }
}

[System.Serializable]
internal class Grid
{
    public bool isOccupied;
    public int x;
    public int y;
    // public Vector3 pos;
    public ObstacleType obstacle;
    public Grid()
    {
        this.x = 0;
        this.y = 0;
        this.isOccupied = false;
        this.obstacle = ObstacleType.None;
    }
    public Grid(int idxX, int idxY)
    {
        this.x = idxX;
        this.y = idxY;
        // this.pos = pos;
        this.isOccupied = false;
        this.obstacle = ObstacleType.None;
    }

    public void SetObstacle(ObstacleType type)
    {
        this.obstacle = type;
        if(type != ObstacleType.None)
        {
            this.isOccupied = true;
        }
        else
        {
            this.isOccupied = false;
        }
    }
}
