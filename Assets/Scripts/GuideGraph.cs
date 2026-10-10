using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Afterglow/Guide Graph")]
public sealed class GuideGraph : ScriptableObject
{
    [System.Serializable] public struct Edge { public string from,to,exitId; public Vector2 position; }
    public Edge[] edges;
    // Ability gates deliberately ignored for this development guide.
    public bool NextExit(string from,string target,out Edge next)
    {
        next=default;if(string.IsNullOrEmpty(target)||from==target)return false;
        var queue=new Queue<string>();var visited=new HashSet<string>{from};var first=new Dictionary<string,Edge>();queue.Enqueue(from);
        while(queue.Count>0)
        {
            string current=queue.Dequeue();
            foreach(var edge in edges)
            {
                if(edge.from!=current||!visited.Add(edge.to))continue;
                first[edge.to]=current==from?edge:first[current];
                if(edge.to==target){next=first[edge.to];return true;}
                queue.Enqueue(edge.to);
            }
        }
        return false;
    }
}
