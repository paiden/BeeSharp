using System.Collections.Generic;
using System.Linq;
using BeeSharp.Traits;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Traits
{
    public class TreeNodeTraitTests
    {
        [Fact]
        public void EnumeateBreadthFirst_DeliversTopToBottomOrder()
        {
            // Arrange
            var t = SimpleTree();

            // Act
            var res = t.TreeTrait().EnumerateBreadthFirst();

            // Assert
            StringRep(res).Should().Be("1-23-456");
        }

        [Fact]
        public void EnumeateDepthFirstPre_DeliversCorrectNodeSequence()
        {
            // Arrange
            var t = SimpleTree();

            // Act
            var res = t.TreeTrait().EnumerateDepthFirstPre();

            // Assert
            StringRep(res).Should().Be("124536");
        }

        [Fact]
        public void EnumeateDepthFirstPost_DeliversCorrectNodeSequence()
        {
            // Arrange
            var t = SimpleTree();

            // Act
            var res = t.TreeTrait().EnumerateDepthFirstPost();

            // Assert
            StringRep(res).Should().Be("452631");
        }

        [Fact]
        public void Map_DeliversMappedTree()
        {
            // Arrange
            var t = SimpleTree();

            // Act
            var res = t.TreeTrait().Map(n => new TestNode(n.Val * 2));

            // Assert
            var lt = res.TreeTrait().EnumerateDepthFirstPre();
            StringRep(lt).Should().Be("24810612");
        }

        private static string StringRep(IEnumerable<IEnumerable<TestNode>> nodes)
            => string.Join("-", nodes.Select(e => StringRep(e)));

        private static string StringRep(IEnumerable<TestNode> nodes)
            => string.Join(string.Empty, nodes.Select(n => n.ToString()));

        /// <summary>
        ///           1
        ///          / \
        ///         2   3
        ///        / \   \
        ///       4   5   6
        /// </summary>
        /// <returns></returns>
        private static TestNode SimpleTree()
        {
            return new TestNode(1,
                new TestNode(2,
                    new TestNode(4),
                    new TestNode(5)),
                new TestNode(3,
                    new TestNode(6)));
        }

        private class TestNode : ITreeNodeTrait<TestNode>, ITreeNodeBuildTrait<TestNode>
        {
            public int Val { get; }

            private List<TestNode> children;

            public TestNode(int val, params TestNode[] children)
            {
                this.Val = val;
                this.children = new List<TestNode>(children);
            }

            public IEnumerable<TestNode> GetChildren() => this.children;

            public override string ToString() => this.Val.ToString();

            void ITreeNodeBuildTrait<TestNode>.SetChildren(IEnumerable<TestNode> children)
                => this.children = children.ToList();
        }
    }
}
