public struct LocationForTower
{
    public int x;
    public int z;
    public bool isOcuppied;

    public LocationForTower(int x, int z, bool isOcuppied = false)
    {
        this.x = x;
        this.z = z;
        this.isOcuppied = isOcuppied;
    }
}
