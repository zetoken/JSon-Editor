using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace ZTn.Json.Editor.Forms
{
    public partial class JTokenTreeView : TreeView
    {
        #region >> Fields

        JTokenTreeNode dragDropSource;
        JTokenTreeNode lastDragDropTarget;
        DateTime lastDragDropDateTime;
        Color lastDragDropBackColor;
        DragDropEffects lastValidDragDropEffect;

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public JTokenTreeView()
            : base()
        {
            InitializeComponent();

            ItemDrag += ItemDragHandler;
            DragEnter += DragEnterHandler;
            DragDrop += DragDropHandler;
            DragOver += DragOverHandler;
        }

        #endregion

        #region >> TreeView

        /// <inheritdoc />
        /// <remarks>
        /// Style change disabling automatic creation of tooltip on each node of the TreeView (no other C# way of doing this).
        /// </remarks>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.Style |= 0x80;    // Turn on TVS_NOTOOLTIPS
                return cp;
            }
        }

        #endregion

        private void ItemDragHandler(object sender, ItemDragEventArgs e)
        {
            dragDropSource = e.Item as JTokenTreeNode;

            if (dragDropSource != null)
            {
                lastValidDragDropEffect = DragDropEffects.Move;
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void DragEnterHandler(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void DragDropHandler(object sender, DragEventArgs e)
        {
            if (lastDragDropTarget != null)
            {
                lastDragDropTarget.BackColor = lastDragDropBackColor;
                lastDragDropTarget = null;
            }

            JTokenTreeNode node = e.Data.GetData(e.Data.GetFormats().FirstOrDefault(), true) as JTokenTreeNode;

            if (node != null)
            {
                MessageBox.Show("Drag & Drop Ready");
            }
        }

        private void DragOverHandler(object sender, DragEventArgs e)
        {
            Point targetPoint = this.PointToClient(new Point(e.X, e.Y));

            JTokenTreeNode targetNode = this.GetNodeAt(targetPoint) as JTokenTreeNode;

            if (targetNode != null)
            {
                if (targetNode != lastDragDropTarget)
                {
                    if (IsDragDropValid(targetNode))
                    {
                        if (e.Effect == DragDropEffects.None)
                        {
                            e.Effect = lastValidDragDropEffect;
                        }

                        lastDragDropBackColor = targetNode.BackColor;
                        targetNode.BackColor = Color.BlueViolet;
                    }
                    else
                    {
                        if (e.Effect != DragDropEffects.None)
                        {
                            lastValidDragDropEffect = e.Effect;
                            e.Effect = DragDropEffects.None;
                        }
                    }

                    if (lastDragDropTarget != null)
                    {
                        lastDragDropTarget.BackColor = lastDragDropBackColor;
                    }

                    lastDragDropTarget = targetNode;
                    lastDragDropDateTime = DateTime.Now;
                }
                else
                {
                    if (DateTime.Now - lastDragDropDateTime >= new TimeSpan(5000000))
                    {
                        targetNode.Expand();
                    }
                }
            }
            else
            {
                if (e.Effect != DragDropEffects.None)
                {
                    lastValidDragDropEffect = e.Effect;
                    e.Effect = DragDropEffects.None;
                }

                if (lastDragDropTarget != null)
                {
                    lastDragDropTarget.BackColor = lastDragDropBackColor;
                }

                lastDragDropTarget = null;
            }
        }

        private bool IsDragDropValid(JTokenTreeNode node)
        {
            if (dragDropSource == null || node == null)
            {
                return false;
            }

            if (dragDropSource.JTokenTag is JProperty)
            {
                return (node.JTokenTag is JObject);
            }
            else if (dragDropSource.JTokenTag is JObject)
            {
                return node.JTokenTag is JProperty || node.JTokenTag is JArray;
            }
            else if (dragDropSource.JTokenTag is JObject)
            {
                return node.JTokenTag is JArray;
            }

            return false;
        }
    }
}
