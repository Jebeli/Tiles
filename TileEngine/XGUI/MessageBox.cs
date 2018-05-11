using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.XGUI
{
    public enum MessageType
    {
        Information,
        Question,
        Warning
    };
    public class MessageBox : Window
    {
        private Label messageLabel;
        public MessageBox(Widget parent, MessageType type, string title = "Untitled", string message = "Message", string buttonText = "OK", string altButtonText = "Cancel", bool altButton = false)
            : base(parent, title)
        {

            Layout = new BoxLayout(Orientation.Vertical, Alignment.Middle, 10, 10);
            Modal = true;
            Widget panel1 = new Widget(this);
            panel1.Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 10, 15);
            Icons icon = Icons.NONE;
            switch (type)
            {
                case MessageType.Information: icon = Theme.MessageInformationIcon; break;
                case MessageType.Question: icon = Theme.MessageQuestionIcon; break;
                case MessageType.Warning: icon = Theme.MessageWarningIcon; break;
            }
            Label iconLabel = new Label(panel1, "", icon);
            //iconLabel.FontSize = 50;
            messageLabel = new Label(panel1, message);
            messageLabel.FixedWidth = 200;
            Widget panel2 = new Widget(this);
            panel2.Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 15);
            Button button2 = new Button(panel2, buttonText, Theme.MessagePrimaryButtonIcon);
            button2.Click += (o, i) => { CloseWindow(); OnButtonSelected(0); };
            if (altButton)
            {
                Button button1 = new Button(panel2, altButtonText, Theme.MessageAltButtonIcon);
                button1.Click += (o, i) => { CloseWindow(); OnButtonSelected(1); };
            }
            CenterWindow();
            RequestFocus();
        }

        public event EventHandler<IndexEventArgs> ButtonSelected;
        public EventHandler<IndexEventArgs> ButtonSelectedEvent
        {
            set { ButtonSelected += value; }
        }
        public Label MessageLabel
        {
            get { return messageLabel; }
            set { messageLabel = value; }
        }

        protected void OnButtonSelected(int index)
        {
            ButtonSelected?.Invoke(this, new IndexEventArgs(index));
        }
    }
}
