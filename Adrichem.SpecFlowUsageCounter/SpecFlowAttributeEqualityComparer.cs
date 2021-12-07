namespace Adrichem.SpecFlowUsageCounter
{
    using System.Collections.Generic;

    /// <summary>
    /// Compares <see cref="SpecFlowAttribute"/> objects for logical equality.
    /// </summary>
    public class SpecFlowAttributeEqualityComparer : IEqualityComparer<SpecFlowAttribute>
    {
        public bool Equals(SpecFlowAttribute x, SpecFlowAttribute y)
        {
            if (x is null) return false;
            if (y is null) return false;
            if (ReferenceEquals(x, y)) return true;

            return x.Line == y.Line
                && x.File == y.File
                && x.Keyword == y.Keyword
                && x.Text == y.Text
            ;
        }

        public int GetHashCode(SpecFlowAttribute obj)
        {
            if (obj is null) return 0;
            return obj.Line.GetHashCode() ^ obj.Keyword.GetHashCode() ^ obj.Text.GetHashCode() ^ obj.File.GetHashCode();
        }
    }
}
