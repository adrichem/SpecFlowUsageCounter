namespace Adrichem.SpecFlowUsageCounter
{
    using System.Text.RegularExpressions;
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
