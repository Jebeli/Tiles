/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
 */

namespace TileEngine.YGUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;
    using TileEngine.Fonts;
    using TileEngine.Graphics;
    using TileEngine.Input;

    public enum TableSelectMode
    {
        Rows,
        Cells,
        Cols
    }

    public class TableGadget : Gadget
    {

        public class TableColumn
        {
            public TableGadget Table { get; set; }
            public int Index { get; set; }
            public string Label { get; set; }
            public int Width { get; set; }
            public int PixelWidth { get; set; }
            public HorizontalTextAlign HTextAlign { get; set; }
            public VerticalTextAlign VTextAlign { get; set; }
            public int Height
            {
                get { return Table.VisibleHeaderHeight; }
            }
            public int X { get; set; }
        }

        public class TableRow
        {
            public TableGadget Table { get; set; }
            public int Index { get; set; }
            public List<TableCell> Cells { get; set; }
            public int Id { get; set; }
            public object Tag { get; set; }
            public int Y
            {
                get
                {
                    //int y = Table.TopEdge + Table.VisibleColumnHeight;
                    //int y = Table.VisibleHeaderHeight;
                    int y = 0;
                    int offset = Index - Table.FirstVisibleRow;
                    y += offset * Table.rowHeight;
                    y -= Table.FirstVisibleRowMod;
                    return y;
                }
            }
            public TableRow()
            {
                Cells = new List<TableCell>();
            }
        }

        public class TableCell
        {
            public bool Hover { get; set; }
            public bool Selected { get; set; }
            public bool MouseSelected { get; set; }
            public bool DrawSelected { get { return Selected || MouseSelected; } }
            public Font Font { get { return Table.Font; } }
            public TableGadget Table { get; set; }
            public TableRow Row { get; set; }
            public TableColumn Column { get; set; }
            public string Label { get; set; }
            public Icons Icon { get; set; }
            public TextureRegion Image { get; set; }
            public HorizontalTextAlign HTextAlign { get; set; }
            public VerticalTextAlign VTextAlign { get; set; }
            public int Width
            {
                get { return Column != null ? Column.PixelWidth : Table.Width - 20; }
            }
            public int Height
            {
                get { return Table.rowHeight; }
            }
            public int X
            {
                get { return Column != null ? Column.X : 0; }
            }
            public int Y
            {
                get { return Row.Y; }
            }
            public int RowIndex
            {
                get { return Row.Index; }
            }
            public int ColIndex
            {
                get { return Column != null ? Column.Index : 0; }
            }
            public bool IsFirstInRow { get { return ColIndex == 0; } }
            public bool IsLastInRow { get { return ColIndex == Table.LastColumnIndex; } }
            public bool IsFirstInCol { get { return Row.Index == 0; } }
            public bool IsLastInCol { get { return Row.Index == Table.LastRowIndex; } }
        }

        private List<TableColumn> columns;
        private List<TableRow> rows;
        private ScrollbarGadget vScroll;
        private int headerHeight;
        private int rowHeight;
        private TableCell hoverCell;
        private TableCell mouseSelectedCell;
        private TableSelectMode selectMode;
        private TableCell selectedCell;
        private bool showHeader;
        private bool evenColumns;

        public TableGadget(Gadget parent)
            : base(parent)
        {
            evenColumns = true;
            showHeader = true;
            rowHeight = 20;
            headerHeight = 24;
            columns = new List<TableColumn>();
            rows = new List<TableRow>();
            vScroll = new ScrollbarGadget(this, Orientation.Vertical);
        }

        public event EventHandler<EventArgs> SelectedCellChanged;
        public EventHandler<EventArgs> SelectedCellChangedEvent
        {
            set { SelectedCellChanged += value; }
        }

        protected virtual void OnSelectedCellChanged()
        {
            SelectedCellChanged?.Invoke(this, EventArgs.Empty);
        }

        public TableSelectMode SelectMode
        {
            get { return selectMode; }
            set
            {
                if (selectMode != value)
                {
                    selectMode = value;
                    if (selectedCell != null)
                    {
                        ClearSelection();
                        SetCellSelection(selectedCell, true);
                    }
                }
            }
        }

        public Rect HeaderRect
        {
            get
            {
                if (showHeader)
                {
                    var rect = Bounds;
                    rect.Inflate(-1, -1);
                    rect.Width -= vScroll.Width;
                    rect.Height = headerHeight;
                    return rect;
                }
                return new Rect();
            }
        }

        public Rect TableRect
        {
            get
            {
                var rect = Bounds;
                rect.Inflate(-1, -1);
                rect.Width -= vScroll.Width;
                if (showHeader)
                {
                    rect.Height -= headerHeight;
                    rect.Y += headerHeight;
                }
                return rect;
            }
        }

        public bool EvenColumns
        {
            get { return evenColumns; }
            set { evenColumns = value; }
        }

        public TableColumn AddColumn(string label, int width)
        {
            TableColumn col = new TableColumn()
            {
                Table = this,
                Label = label,
                Width = width,
                PixelWidth = width,
                HTextAlign = HorizontalTextAlign.Left,
                VTextAlign = VerticalTextAlign.Center
            };
            columns.Add(col);
            InitColumns();
            return col;
        }

        public TableRow FindRow(int id)
        {
            foreach (var row in rows)
            {
                if (row.Id == id)
                {
                    return row;
                }
            }
            return null;
        }

        public void SelectRow(TableRow row)
        {
            if (row != null)
            {
                SetSelecedCell(row.Cells[0]);
            }
            else
            {
                SetSelecedCell(null);
                ScrollToRow(0);
            }
        }

        public void SetHoverCell(TableCell cell)
        {
            if (cell != hoverCell)
            {
                if (hoverCell != null)
                {
                    hoverCell.Hover = false;
                }
                hoverCell = cell;
                if (hoverCell != null)
                {
                    hoverCell.Hover = true;
                }
            }
        }

        public void SetMouseSelectedCell(TableCell cell)
        {
            if (cell != mouseSelectedCell)
            {
                if (mouseSelectedCell != null)
                {
                    SetCellMouseSelection(mouseSelectedCell, false);
                }
                mouseSelectedCell = cell;
                if (mouseSelectedCell != null)
                {
                    SetCellMouseSelection(mouseSelectedCell, true);
                }
            }
        }

        public void ClearSelection()
        {
            foreach (var row in rows)
            {
                foreach (var c in row.Cells)
                {
                    c.Selected = false;
                }
            }
        }

        public void SetSelecedCell(TableCell cell)
        {
            if (selectedCell != cell)
            {
                if (selectedCell != null)
                {
                    SetCellSelection(selectedCell, false);
                }
                selectedCell = cell;
                if (selectedCell != null)
                {
                    SetCellSelection(selectedCell, true);
                    ScrollToRow(selectedCell.RowIndex);
                }
            }
        }

        public void SetCellMouseSelection(TableCell cell, bool sel)
        {
            switch (selectMode)
            {
                case TableSelectMode.Cells:
                    cell.MouseSelected = sel;
                    break;
                case TableSelectMode.Rows:
                    foreach (var c in cell.Row.Cells)
                    {
                        c.MouseSelected = sel;
                    }
                    break;
                case TableSelectMode.Cols:
                    int idx = cell.ColIndex;
                    foreach (var row in rows)
                    {
                        row.Cells[idx].MouseSelected = sel;
                    }
                    break;
            }
        }


        public void SetCellSelection(TableCell cell, bool sel)
        {
            switch (selectMode)
            {
                case TableSelectMode.Cells:
                    cell.Selected = sel;
                    break;
                case TableSelectMode.Rows:
                    foreach (var c in cell.Row.Cells)
                    {
                        c.Selected = sel;
                    }
                    break;
                case TableSelectMode.Cols:
                    int idx = cell.ColIndex;
                    foreach (var row in rows)
                    {
                        row.Cells[idx].Selected = sel;
                    }
                    break;
            }
        }

        public void ClearRows()
        {
            selectedCell = null;
            rows.Clear();
        }

        public TableRow AddRow(params string[] labels)
        {
            TableRow row = new TableRow()
            {
                Table = this
            };
            MakeCells(row);
            for (int i = 0; i < row.Cells.Count && i < labels.Length; i++)
            {
                row.Cells[i].Label = labels[i];
            }
            row.Index = rows.Count;
            rows.Add(row);
            if (!IsUpdating)
            {
                AdjustScrollbar();
            }
            return row;
        }

        public int LastColumnIndex
        {
            get { return columns.Count - 1; }
        }

        public int LastRowIndex
        {
            get { return rows.Count - 1; }
        }

        public void InitColumns()
        {
            int x = 0;
            int index = 0;
            foreach (var col in columns)
            {
                col.X = x;
                col.Index = index;
                x += col.PixelWidth;
                index++;
            }
        }

        public void InitColumnWidths()
        {
            if (evenColumns)
            {
                int numCols = Math.Max(1, columns.Count);
                int w = Width - 20 - 2;
                int cw = w / numCols;
                if (numCols > 1)
                {
                    for (int i = 0; i < numCols; i++)
                    {
                        if (i < columns.Count)
                        {
                            if (columns[i].Width >= 0)
                            {
                                columns[i].PixelWidth = cw;
                            }
                            if (i == numCols - 1)
                            {
                                columns[i].PixelWidth = w - (numCols - 1) * cw;
                            }
                        }
                    }
                }
            }
            else
            {
                int numCols = Math.Max(1, columns.Count);
                int w = Width - 20 - 2;
                int numAutoCols = numCols;
                if (numCols > 1)
                {
                    for (int i = 0; i < numCols; i++)
                    {
                        if (i < columns.Count)
                        {
                            int cw = columns[i].Width;
                            if (cw >= 0)
                            {
                                columns[i].PixelWidth = cw;
                                w -= cw;
                                numAutoCols--;
                            }
                            else
                            {
                                columns[i].PixelWidth = -1;
                            }
                        }
                    }
                    if (numAutoCols > 0)
                    {
                        int acw = w / numAutoCols;
                        for (int i = 0; i < numCols; i++)
                        {
                            if (i < columns.Count)
                            {
                                if (columns[i].PixelWidth < 0)
                                {
                                    columns[i].PixelWidth = acw;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void MakeCells(TableRow row)
        {
            int num = Math.Max(1, columns.Count);
            int i = 0;
            while (row.Cells.Count < num)
            {
                TableCell cell = new TableCell()
                {
                    Row = row,
                    Table = this,
                    HTextAlign = HorizontalTextAlign.Left,
                    VTextAlign = VerticalTextAlign.Center
                };
                if (i < columns.Count)
                {
                    cell.Column = columns[i];
                }
                row.Cells.Add(cell);
                i++;
            }
            while (row.Cells.Count > num)
            {
                row.Cells.RemoveAt(row.Cells.Count - 1);
            }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            int w = 20;
            if (columns.Count > 0)
            {
                foreach (var c in columns)
                {
                    if (c.PixelWidth > 0)
                    {
                        w += c.PixelWidth;
                    }
                    else
                    {
                        w += 100;
                    }
                }
            }
            else
            {
                w += 180;
            }
            return new Vector2(w, 200);
        }

        protected override void OnEndUpdate()
        {
            AdjustScrollbar();
        }

        public override void PerformLayout(IGraphics gfx)
        {
            //base.PerformLayout(gfx);
            vScroll.LeftEdge = Width - 20;
            vScroll.TopEdge = 0;
            vScroll.Width = 20;
            vScroll.Height = Height;
            AdjustScrollbar();
            InitColumnWidths();
            InitColumns();
            vScroll.PerformLayout(gfx);
        }

        internal int VisibleHeaderHeight
        {
            get { return showHeader ? headerHeight : 0; }
        }

        private void AdjustScrollbar()
        {
            int h = Height - VisibleHeaderHeight - 4;
            vScroll.Min = 0;
            vScroll.Max = rows.Count * rowHeight;
            vScroll.VisibleAmount = h;
            vScroll.Increment = h;
        }

        public int FirstVisibleRowMod
        {
            get
            {
                if (FirstVisibleRow > 0)
                {
                    int f = FirstVisibleRow * rowHeight;
                    int v = vScroll.Value - f;
                    return v;
                }
                return 0;
            }
        }

        public int FirstVisibleRow
        {
            get
            {
                int v = vScroll.Value / rowHeight;
                return v;
            }
        }

        public int NumVisibleRows
        {
            get
            {
                int v = vScroll.VisibleAmount / rowHeight + 2;
                return v;
            }
        }

        public int LastVisibleRow
        {
            get
            {
                return FirstVisibleRow + NumVisibleRows;
            }
        }

        public int HeaderHeight
        {
            get { return headerHeight; }
            set { headerHeight = value; }
        }

        public int RowHeight
        {
            get { return rowHeight; }
            set { rowHeight = value; }
        }

        public bool ShowHeader
        {
            get { return showHeader; }
            set { showHeader = value; }
        }

        public IList<TableColumn> Columns
        {
            get { return columns; }
        }

        public IList<TableRow> Rows
        {
            get { return rows; }
        }

        private int X2Col(int x)
        {
            if (x < 0) return -1;
            if (columns.Count == 0)
            {
                return 0;
            }
            foreach (var col in columns)
            {
                if (x < col.PixelWidth)
                {
                    return col.Index;
                }
                x -= col.PixelWidth;
            }
            return -1;
        }

        private int Y2Row(int y)
        {
            y -= 2;
            y -= VisibleHeaderHeight;
            if (y < 0) return -1;
            y += FirstVisibleRowMod;
            y /= rowHeight;
            y += FirstVisibleRow;
            return y;
        }

        private TableCell XY2Cell(Vector2 p)
        {
            int x = X2Col(p.X);
            int y = Y2Row(p.Y);
            return GetCell(x, y);
        }

        public TableCell GetCell(int x, int y)
        {
            if (y >= 0 && y < rows.Count)
            {
                if (columns.Count == 0)
                {
                    return rows[y].Cells[0];
                }
                if (x >= 0 && x < columns.Count)
                {
                    return rows[y].Cells[x];
                }
            }
            return null;
        }

        protected override void HandleSelectDown(Vector2 p)
        {
            SetMouseSelectedCell(XY2Cell(p));
            base.HandleSelectDown(p);
        }

        protected override void HandleSelectUp(Vector2 p)
        {
            TableCell tc = XY2Cell(p);
            if (tc == mouseSelectedCell)
            {
                SetSelecedCell(tc);
                OnSelectedCellChanged();
            }
            SetMouseSelectedCell(null);
            base.HandleSelectUp(p);
        }

        public void ScrollToRow(int row)
        {
            if (row <= FirstVisibleRow)
            {
                vScroll.Value = row * rowHeight;
            }
            else if (row >= (LastVisibleRow - 2))
            {
                vScroll.Value = (row - (NumVisibleRows - 3)) * rowHeight;
            }
        }

        public int SelectedRowId
        {
            get
            {
                if (selectedCell != null)
                {
                    return selectedCell.Row.Id;
                }
                return -1;
            }
        }

        public int SelectedRow
        {
            get
            {
                if (selectedCell != null)
                {
                    return selectedCell.RowIndex;
                }
                return -1;
            }
            set
            {
                TableCell tc = null;
                if (value >= rows.Count) value = rows.Count - 1;
                if (value < 0) value = 0;
                if (selectedCell != null)
                {
                    tc = GetCell(selectedCell.ColIndex, value);
                }
                else
                {
                    tc = GetCell(0, value);
                }
                SetSelecedCell(tc);
            }
        }

        protected override void HandleSelectMove(Vector2 p)
        {
            SetHoverCell(XY2Cell(p));
            base.HandleSelectMove(p);
        }

        public override void HandleKeyDown(Key keyData, Key keyCode, char code)
        {
            switch (keyData)
            {
                case Key.Down:
                    SelectedRow++;
                    OnSelectedCellChanged();
                    break;
                case Key.PageDown:
                    SelectedRow += NumVisibleRows;
                    OnSelectedCellChanged();
                    break;
                case Key.Up:
                    SelectedRow--;
                    OnSelectedCellChanged();
                    break;
                case Key.PageUp:
                    SelectedRow -= NumVisibleRows;
                    OnSelectedCellChanged();
                    break;
            }
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }
    }
}
