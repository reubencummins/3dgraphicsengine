using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sample;

namespace GraphicsEngine
{
    class QuadTree
    {
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public QuadTree NodeTL { get; set; }
        public QuadTree NodeTR { get; set; }
        public QuadTree NodeBL { get; set; }
        public QuadTree NodeBR { get; set; }
        private List<QuadTree> childNodes;

        public int MaxObjectsPerNode { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public List<SimpleModel> Objects { get; set; }

        public QuadTree(Vector2 position, Vector2 size)
        {
            
            Position = position;
            Size = size;
            BoundingBox = new BoundingBox(new Vector3(Position - Size / 2, 0), new Vector3(Position + Size / 2, 0));
        }

        public void SubDivide()
        {
            NodeTL = new QuadTree(Position - Size / 4, Size / 4);
            NodeBR = new QuadTree(Position + Size / 4, Size / 4);
            NodeTR = new QuadTree(Position + new Vector2(-Size.X / 4, Size.Y / 4), Size / 4);
            NodeTR = new QuadTree(Position + new Vector2(Size.X / 4, -Size.Y / 4), Size / 4);
            childNodes = new List<QuadTree>()
            {
                NodeTL, NodeTR, NodeBL, NodeBR
            };

            Distribute();
            Objects.Clear();
        }

        public void AddObject(SimpleModel _object)
        {
            Objects.Add(_object);
            if (childNodes==null)
            {
                if (Objects.Count>MaxObjectsPerNode)
                {
                    SubDivide();
                }
            }
            else
            {
                Distribute();
            }
        }

        private void Distribute()
        {
            foreach (var _object in Objects)
            {
                Vector2 objectPosition = new Vector2(_object.World.Translation.X, _object.World.Translation.Y);
                QuadTree target = NodeTL;
                foreach (QuadTree node in childNodes)
                {
                    if ((objectPosition-node.Position).Length()<(objectPosition-target.Position).Length())
                    {
                        target = node;
                    }
                }
                target.AddObject(_object);
                Objects.Remove(_object);
            }
        }

        public List<SimpleModel> Process(BoundingFrustum frustum)
        {
            List<SimpleModel> drawnObjects = new List<SimpleModel>();
            if (frustum.Contains(BoundingBox) != ContainmentType.Disjoint)
            {
                drawnObjects.AddRange(Objects);
                foreach (var node in childNodes)
                {
                    drawnObjects.AddRange(node.Process(frustum));
                }
            }
            return drawnObjects;
        }

        public void ClearNode(QuadTree node)
        {
            if (node!=null)
            {
                node.Clear();
                node = null;
            }
        }

        public void Clear()
        {
            Objects.Clear();
            foreach (var node in childNodes)
            {
                ClearNode(node);
            }
            childNodes.Clear();
        }
    }
}
