using System.Text.RegularExpressions;

namespace Adrichem.SpecFlowUsageCounter
{
    /// <inheritdoc cref="ISpecFlowAttribute"/>
    public class SpecFlowAttribute
    {
        private string _Text;
        public string Keyword { get; set; }

        public string Text { 
            get => _Text;
            set { _Text = value; Re = new Regex($"^{value}$"); } 
        }

        public string File { get; set; }

        public int Line { get; set; }

        internal Regex Re { get; set; }
    }
}
