using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Files;

namespace TileEngine.YGUI
{
    public class FileGadget : Gadget
    {
        private TableGadget table;
        private StrGadget fileName;
        private StrGadget dirName;
        private ButtonGadget butOk;
        private ButtonGadget butVolumes;
        private ButtonGadget butParent;
        private ButtonGadget butCancel;
        private IFileResolver fileResolver;
        private FileInfo selectedFile;
        private string currentDir;

        public FileGadget(Gadget parent, IFileResolver fileResolver)
            : base(parent)
        {
            this.fileResolver = fileResolver;
            layout = new BoxLayout(Orientation.Vertical, Alignment.Fill);
            table = new TableGadget(this)
            {
                EvenColumns = false,
                SelectedCellChangedEvent = (o, i) => { TableSelectedChanged(); }
            };
            table.AddColumn("Name", -1);
            table.AddColumn("Size", 64);
            table.AddColumn("Date", 130);
            table.AddColumn("Comment", 140);
            var dirBox = new BoxGadget(this, Orientation.Horizontal, Alignment.Fill);
            new LabelGadget(dirBox, "Drawer")
            {
                FixedWidth = 100
            };
            dirName = new StrGadget(dirBox)
            {
                FixedWidth = 300
            };

            var fileBox = new BoxGadget(this, Orientation.Horizontal, Alignment.Fill);
            new LabelGadget(fileBox, "File")
            {
                FixedWidth = 100
            };
            fileName = new StrGadget(fileBox)
            {
                FixedWidth = 300
            };
            var buttonBox = new BoxGadget(this, Orientation.Horizontal, Alignment.Fill, 0, 100);
            butOk = new ButtonGadget(buttonBox, "Open")
            {
                GadgetUpEvent = (o, i) => { Ok(); }
            };
            butVolumes = new ButtonGadget(buttonBox, "Volumes")
            {
                GadgetUpEvent = (o, i) => { Volumes(); }
            };
            butParent = new ButtonGadget(buttonBox, "Parent")
            {
                GadgetUpEvent = (o, i) => { GoToParent(); }
            };
            butCancel = new ButtonGadget(buttonBox, "Cancel")
            {
                GadgetUpEvent = (o, i) => { Cancel(); }
            };
        }

        public event EventHandler<EventArgs> OkSelected;
        public EventHandler<EventArgs> OkSelectedEvent
        {
            set { OkSelected += value; }
        }

        protected virtual void OnOkSelected()
        {
            OkSelected?.Invoke(this, EventArgs.Empty);
        }

        public FileInfo SelectedFile
        {
            get { return selectedFile; }
        }

        private void TableSelectedChanged()
        {
            int row = table.SelectedRow;
            if (row >= 0 && row < table.Rows.Count)
            {
                var tr = table.Rows[row];
                FileInfo info = tr.Tag as FileInfo;
                if (info != null)
                {
                    if (info.IsDirectory)
                    {
                        GoToDir(info.Path);
                    }
                    else
                    {
                        selectedFile = info;
                        fileName.Buffer = info.Name;
                    }
                }
            }
        }

        public void Volumes()
        {
            FillTable(fileResolver.GetVolumeInfos());
        }

        public void GoToDir(string dir)
        {
            if (!string.IsNullOrEmpty(dir))
            {
                currentDir = dir;
                dirName.Buffer = dir;
                fileName.Buffer = "";
                selectedFile = null;
                FillTable(fileResolver.GetFileInfos(dir));
            }
            else
            {
                currentDir = "";
                dirName.Buffer = "";
                fileName.Buffer = "";
                selectedFile = null;
                Volumes();
            }
        }

        public void GoToParent()
        {
            if (!string.IsNullOrEmpty(currentDir))
            {
                string fn = fileResolver.GetParent(currentDir);
                GoToDir(fn);
            }
        }

        public void Ok()
        {
            Screen?.CloseWindow(this.Window);
            OnOkSelected();
        }

        public void Cancel()
        {
            Screen?.CloseWindow(this.Window);
        }

        private void FillTable(IList<FileInfo> infos)
        {
            table.ClearRows();
            if (infos != null)
            {
                foreach (var info in infos)
                {
                    var name = info.Name;
                    string sizeS = info.IsDirectory ? "Drawer" : info.Size.ToString();
                    string dateS = info.Date.ToString();
                    var row = table.AddRow(name, sizeS, dateS);
                    row.Tag = info;
                }
            }
            Window?.Invalidate();
        }

        public static FileGadget ShowSelectFile(IFileResolver fileResolver, Screen screen, string initialDir, EventHandler<EventArgs> handler)
        {
            Window dialog = new Window(screen, "Open File")
            {
                CloseGadget = true,
                SizeGadget = true,
                DepthGadget = true
            };
            FileGadget fg = new FileGadget(dialog, fileResolver)
            {
                OkSelectedEvent = handler
            };
            fg.GoToDir(initialDir);
            dialog.WindowCloseEvent = (o, i) => { fg.Cancel(); };
            screen.ShowWindow(dialog);
            screen.WindowToFront(dialog);
            screen.ActivateWindow(dialog);
            return fg;
        }
    }
}
