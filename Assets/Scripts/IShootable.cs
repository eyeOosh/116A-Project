public interface IShootable
{
    /// <summary>
    /// invoked when object is hit
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>if the hit was valid</returns>
    public bool Hit(float damage);
}