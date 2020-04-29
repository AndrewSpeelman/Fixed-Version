public class Filter : Module
{
    public int PurityIndex;
    /// <summary>
    /// This onflow override cleans water when passed through based on a specified purity index set on the filter object.
    /// </summary>

    public override bool IsFilter()
    {
        return true;
    }
}