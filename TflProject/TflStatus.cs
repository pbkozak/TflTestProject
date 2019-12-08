using System.Collections.Generic;

namespace TflProject
{
    public class TflStatus
    {
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;

        public List<string> Messages = new List<string>();
    }
}
