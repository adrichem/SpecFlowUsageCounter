namespace Adrichem.SpecFlowUsageCounter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [Serializable]
    public class UsageCounts : Dictionary<SpecFlowAttribute, int>
    {
        public UsageCounts() : base() { }
        protected UsageCounts(SerializationInfo info, StreamingContext context) : base(info, context) { }

        internal static UsageCounts FromDictionary(IDictionary<SpecFlowAttribute, int> dict)
        {
            var tmp = new UsageCounts();
            foreach (var item in dict)
            {
                tmp.Add(item.Key, item.Value);
            }
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
