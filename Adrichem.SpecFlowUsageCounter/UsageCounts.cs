namespace Adrichem.SpecFlowUsageCounter
{
    using System.Collections.Generic;
    using System.Linq;

    public class UsageCounts : Dictionary<SpecFlowAttribute, int>
    {
        internal static UsageCounts FromDictionary(IDictionary<SpecFlowAttribute, int> dict)
        {
            var tmp = new UsageCounts();
            foreach(var item in dict)
                tmp.Add(item.Key, item.Value);
            return tmp;
        }

        public IOrderedEnumerable<SpecFlowAttribute> UnusedDefinitions()
        {
            return this
                .Where(kvp => kvp.Value == 0)
                .Select(kvp => kvp.Key)
                .OrderBy(attr => attr.File)
                .ThenBy(attr => attr.Line)
            ;
        }
    }
}
