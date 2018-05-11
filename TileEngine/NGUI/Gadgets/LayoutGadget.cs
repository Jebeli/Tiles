using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI.Gadgets
{
    public class LayoutGadget : GroupGadget
    {
        private int spacing;
        private int margin;
        private Orientation orientation;
        private LayoutAlignment alignment;
        private LayoutKind kind;
        private int resolution;

        public LayoutGadget()
        {
            Frame = true;
            resolution = 2;
            margin = 2;
            spacing = 2;
            orientation = Orientation.Horizontal;
            alignment = LayoutAlignment.Minimum;
            kind = LayoutKind.Box;
        }


        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                }
            }
        }

        public LayoutAlignment Alignment
        {
            get { return alignment; }
            set
            {
                if (alignment != value)
                {
                    alignment = value;
                }
            }
        }

        public int Spacing
        {
            get { return spacing; }
            set
            {
                if (spacing != value)
                {
                    spacing = value;
                }
            }
        }

        public int Margin
        {
            get { return margin; }
            set
            {
                if (margin != value)
                {
                    margin = value;
                }
            }
        }

        public LayoutKind Kind
        {
            get { return kind; }
            set
            {
                if (kind != value)
                {
                    kind = value;
                }
            }
        }

        public int Resolution
        {
            get { return resolution; }
            set
            {
                if (resolution != value)
                {
                    resolution = value;
                }
            }
        }

        public override void Layout()
        {
            if (IsUpdating) return;
            BeginUpdate();
            Box minBox = new Box();
            Box maxBox = new Box();
            foreach (var element in Children)
            {
                element.BeginUpdate();
            }
            PerformLayout();
            foreach (var element in Children)
            {
                element.EndUpdate();
            }
            EndUpdate();
            base.Layout();
        }

        public override IBox GetPreferredSize()
        {
            switch (kind)
            {
                case LayoutKind.Box:
                    return GetBoxPreferredSize();
                case LayoutKind.Grid:
                    return GetGridPreferredSize();
            }
            return base.GetPreferredSize();
        }

        protected virtual void PerformLayout()
        {
            switch (kind)
            {
                case LayoutKind.Box:
                    PerformBoxLayout();
                    break;
                case LayoutKind.Group:
                    break;
                case LayoutKind.Grid:
                    PerformGridLayout();
                    break;
            }
        }

        private IBox GetGridPreferredSize()
        {
            int[][] grid = new int[2][];
            ComputeGridLayout(ref grid);
            int[] size = new int[]
            {
                2*margin + grid[0].Sum() + Math.Max(grid[0].Length-1,0)*spacing,
                2*margin + grid[1].Sum() + Math.Max(grid[1].Length-1,0)*spacing
            };
            if (Frame)
            {
                size[0] += 4;
                size[1] += 4;
            }
            return new Box() { Width = size[0], Height = size[1] };
        }

        private void PerformGridLayout()
        {
            IBox fs_w = GetFixedSize();
            IBox fs_p = GetPreferredSize();
            int width = fs_w.Width > 0 ? fs_w.Width : fs_p.Width;
            int height = fs_w.Height > 0 ? fs_w.Height : fs_p.Height;
            if (width > Width ) Width = width;
            if (height > Height) Height = height;
            int[] containerSize = new int[] { width, height };
            int[][] grid = new int[2][];
            ComputeGridLayout(ref grid);
            int[] dim = new int[] { grid[0].Length, grid[1].Length };
            int[] extra = new int[2];
            if (dim[0] > 0 && dim[1] > 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    int gridSize = 2 * margin + extra[i];
                    foreach (int s in grid[i])
                    {
                        gridSize += s;
                        if (i + 1 < dim[i])
                        {
                            gridSize += spacing;
                        }
                    }
                    if (gridSize < containerSize[i])
                    {
                        int gap = containerSize[i] - gridSize;
                        int g = gap / dim[i];
                        int rest = gap - g * dim[i];
                        for (int j = 0; j < dim[i]; ++j)
                        {
                            grid[i][j] += g;
                        }
                        for (int j = 0; rest > 0 && j < dim[j]; --rest, ++j)
                        {
                            grid[i][j] += 1;
                        }
                    }
                }
            }
            int axis1 = (int)orientation;
            int axis2 = (((int)orientation) + 1) % 2;
            int[] start = new int[] { margin + extra[0], margin + extra[1] };
            List<IGUIElement> children = new List<IGUIElement>(Children);
            int numChildren = children.Count;
            int child = 0;
            int[] pos = new int[] { start[0], start[1] };
            for (int i2 = 0; i2 < dim[axis2]; i2++)
            {
                pos[axis1] = start[axis1];
                for (int i1 = 0; i1 < dim[axis1]; i1++)
                {
                    IGUIElement w = null;
                    do
                    {
                        if (child >= numChildren)
                        {
                            return;
                        }
                        w = children[child];
                        child++;
                    } while (!w.Visible);
                    var ps = w.GetPreferredSize();
                    var fs = w.GetFixedSize();
                    int[] targetSize = new int[] { fs.Width > 0 ? fs.Width : ps.Width, fs.Height > 0 ? fs.Height : ps.Height };
                    int[] itemPos = new int[] { pos[0], pos[1] };
                    for (int j = 0; j < 2; j++)
                    {
                        int axis = (axis1 + j) % 2;
                        int item = j == 0 ? i1 : i2;
                        switch (alignment)
                        {
                            case LayoutAlignment.Minimum:
                                break;
                            case LayoutAlignment.Middle:
                                itemPos[axis] += (grid[axis][item] - targetSize[axis]) / 2;
                                break;
                            case LayoutAlignment.Maximum:
                                itemPos[axis] += grid[axis][item] - targetSize[axis];
                                break;
                            case LayoutAlignment.Fill:
                                //targetSize[axis] = fs[axis] ? fs[axis] : grid[axis][item];
                                break;
                        }
                    }
                    w.SetBounds(itemPos[0], itemPos[1], targetSize[0], targetSize[1], DimFlags.None);
                    pos[axis1] += grid[axis1][i1] + spacing;
                }
                pos[axis2] += grid[axis2][i2] + spacing;
            }
        }

        private void ComputeGridLayout(ref int[][] grid)
        {
            int axis1 = (int)orientation;
            int axis2 = (((int)orientation) + 1) % 2;
            int numChildren = 0;
            int visibleChildren = 0;
            List<IGUIElement> children = new List<IGUIElement>();
            foreach (var w in Children)
            {
                children.Add(w);
                visibleChildren += w.Visible ? 1 : 0;
                numChildren++;
            }
            int[] dim = new int[2];
            dim[axis1] = resolution;
            dim[axis2] = (visibleChildren + resolution - 1) / resolution;
            grid[axis1] = new int[dim[axis1]];
            grid[axis2] = new int[dim[axis2]];
            int child = 0;
            for (int i2 = 0; i2 < dim[axis2]; i2++)
            {
                for (int i1 = 0; i1 < dim[axis1]; i1++)
                {
                    IGUIElement w = null;
                    do
                    {
                        if (child >= numChildren)
                        {
                            return;
                        }
                        w = children[child];
                        child++;
                    } while (!w.Visible);
                    var ps = w.GetPreferredSize();
                    var fs = w.GetFixedSize();
                    int[] targetSize = new int[] { fs.Width > 0 ? fs.Width : ps.Width, fs.Height > 0 ? fs.Height : ps.Height };
                    grid[axis1][i1] = Math.Max(grid[axis1][i1], targetSize[axis1]);
                    grid[axis2][i2] = Math.Max(grid[axis2][i2], targetSize[axis2]);
                }
            }

        }

        private IBox GetBoxPreferredSize()
        {
            int[] size = new int[] { 2 * margin, 2 * margin };
            int yOffset = 0;
            bool first = true;
            int axis1 = (int)orientation;
            int axis2 = (((int)orientation) + 1) % 2;
            foreach (var w in Children)
            {
                if (!w.Visible)
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    size[axis1] += spacing;
                }
                var ps = w.GetPreferredSize();
                var fs = w.GetFixedSize();
                int[] targetSize = new int[] { fs.Width > 0 ? fs.Width : ps.Width, fs.Height > 0 ? fs.Height : ps.Height };
                size[axis1] += targetSize[axis1];
                size[axis2] += Math.Max(size[axis2], targetSize[axis2] + 2 * margin);
                first = false;
            }
            if (Frame)
            {
                size[0] += 4;
                size[1] += 4;
            }
            return new Box() { Width = size[0], Height = size[1] + yOffset };
        }

        private void PerformBoxLayout()
        {
            IBox fs_w = GetFixedSize();
            IBox fs_p = GetPreferredSize();
            int width = fs_w.Width > 0 ? fs_w.Width : fs_p.Width;
            int height = fs_w.Height > 0 ? fs_w.Height : fs_p.Height;
            if (width > Width) Width = width;
            if (height > Height) Height = height;
            int[] containerSize = new int[] { width, height };
            int axis1 = (int)orientation;
            int axis2 = (((int)orientation) + 1) % 2;
            int position = margin;
            int yOffset = 0;
            bool first = true;
            foreach (var w in Children)
            {
                if (!w.Visible)
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    position += spacing;
                }
                var ps = w.GetPreferredSize();
                var fs = w.GetFixedSize();
                int[] targetSize = new int[] { fs.Width > 0 ? fs.Width : ps.Width, fs.Height > 0 ? fs.Height : ps.Height };
                int[] pos = new int[] { 0, yOffset };
                pos[axis1] = position;
                switch (alignment)
                {
                    case LayoutAlignment.Minimum:
                        pos[axis2] += margin;
                        break;
                    case LayoutAlignment.Middle:
                        pos[axis2] += (containerSize[axis2] - targetSize[axis2]) / 2;
                        break;
                    case LayoutAlignment.Maximum:
                        pos[axis2] += containerSize[axis2] - targetSize[axis2] - margin * 2;
                        break;
                    case LayoutAlignment.Fill:
                        pos[axis2] += margin;
                        targetSize[axis2] = (containerSize[axis2] - margin * 2);
                        break;
                }

                w.SetBounds(pos[0], pos[1], ps.Width, ps.Height, DimFlags.None);
                position += targetSize[axis1];
            }
        }


    }
}
