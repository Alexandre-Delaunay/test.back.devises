using System.Collections.Generic;

namespace test.back.devises.Models
{
    internal class TreeNode
    {
        public TreeNode Parent { get; set; }
        public IEnumerable<TreeNode> Children { get; set; }
        public string Value { get; set; }
        public int Depth { get; set; }
    }
}
