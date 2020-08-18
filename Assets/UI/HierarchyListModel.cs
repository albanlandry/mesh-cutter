using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HierarchyListModel
{
    List<HierarchyListNode> nodes;

    public HierarchyListModel()
    {
        nodes = new List<HierarchyListNode>();
    }

    public void AddNode(HierarchyListNode node) {
        this.nodes.Add(node);
    }

    public List<HierarchyListNode> Nodes { get { return this.nodes; } }

    public int Count { get { return this.nodes.Count; } }

    public HierarchyListNode GetNodeAt(int index) {
        return this.nodes[index];
    }



    public void Remove(HierarchyListNode node) {
        foreach (HierarchyListNode n in nodes) {
            if (n.Id.ToLower().Equals(node.Id.ToLower())) {
                this.nodes.Remove(n);
            }
        }
    }
}
