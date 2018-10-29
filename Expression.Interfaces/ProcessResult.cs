using System;
using System.Collections.Generic;

namespace Expression.Interfaces
{
    public class ProcessResult
    {
        public decimal Progress { get; set; }
        public IEnumerable<string> Variables { get; set; }
    }
}
