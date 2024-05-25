using System.Collections.Generic;

namespace Minesweeper_Milestone350.Services
{
    public class HardCodedRulesRepository : IRulesInterface
    {
        private List<string> _rules;

        public HardCodedRulesRepository()
        {
            _rules = new List<string>
            {
                "This is the first rule",
                "This is the second rule",
                "This is the third rule"
            };
        }

        public List<string> GetAllRules()
        {
            return _rules;
        }
    }
}
