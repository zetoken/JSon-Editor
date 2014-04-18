using System.Reflection;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZTn.Json.Editor.Extensions;
using ZTn.Json.Editor.Generic;

namespace ZTn.Json.Editor.Forms
{
    public partial class JTokenTreeView : TreeView
    {
        #region >> Fields

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

        /// <summary>
        /// Occurs when the user begins dragging a node.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemDragHandler(object sender, ItemDragEventArgs e)
        {
            var sourceNode = e.Item as JTokenTreeNode;

            if (sourceNode == null)
            {
                return;
            }

            lastValidDragDropEffect = DragDropEffects.Copy;
            DoDragDrop(e.Item, DragDropEffects.Move | DragDropEffects.Copy);
        }

        /// <summary>
        /// Occurs when a drag-and-drop operation is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragDropHandler(object sender, DragEventArgs e)
        {
            if (lastDragDropTarget != null)
            {
                lastDragDropTarget.BackColor = lastDragDropBackColor;
                lastDragDropTarget = null;
            }

            var sourceNode = GetDragDropSourceNode(e);

            if (sourceNode == null)
            {
                MessageBox.Show(@"Drag & Drop Canceled: Unknown Source");

                return;
            }

            var targetNode = GetDragDropTargetNode(e);

            switch (e.Effect)
            {
                case DragDropEffects.Copy:
                    DoDragDropCopy((dynamic)sourceNode, (dynamic)targetNode);
                    break;
                case DragDropEffects.Move:
                    DoDragDropMove((dynamic)sourceNode, (dynamic)targetNode);
                    break;
            }
        }

        #region >> DoDragDropCopy

        /// <summary>
        /// Catches all unmanaged copies.
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="targetNode"></param>
        private void DoDragDropCopy(JTokenTreeNode sourceNode, JTokenTreeNode targetNode)
        {
            MessageBox.Show(@"Drag & Drop: Unmanaged Copy");
        }

        /// <summary>
        /// Copies a JProperty into a JObject as first child.
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="targetNode"></param>
        private void DoDragDropCopy(JPropertyTreeNode sourceNode, JObjectTreeNode targetNode)
        {
            sourceNode.ClipboardCopy();
            targetNode.ClipboardPasteInto();
        }

        /// <summary>
        /// Copies a JObject into a JArray as first child.
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="targetNode"></param>
        private void DoDragDropCopy(JObjectTreeNode sourceNode, JArrayTreeNode targetNode)
        {
            sourceNode.ClipboardCopy();
            targetNode.ClipboardPasteInto();
        }

        /// <summary>
        /// Copies a JArray into a JArray as first child.
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="targetNode"></param>
        private void DoDragDropCopy(JArrayTreeNode sourceNode, JArrayTreeNode targetNode)
        {
            sourceNode.ClipboardCopy();
            targetNode.ClipboardPasteInto();
        }

        #endregion

        #region >> DoDragDropMove

        private void DoDragDropMove(JTokenTreeNode sourceNode, JTokenTreeNode targetNode)
        {
            // TODO: Move sourceNode to target
            MessageBox.Show(@"Drag & Drop: Unmanaged Move");
        }

        #endregion

        /// <summary>
        /// Occurs when an object is dragged into the control's bounds.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragEnterHandler(object sender, DragEventArgs e)
        {
        }

        /// <summary>
        /// Occurs when an object is dragged over the control's bounds. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragOverHandler(object sender, DragEventArgs e)
        {
            var targetNode = GetDragDropTargetNode(e);

            var keyState = (KeyStates)e.KeyState;
            if ((keyState & KeyStates.Control) == KeyStates.Control)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if ((keyState & KeyStates.Shift) == KeyStates.Shift)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.Copy;
            }

            if (targetNode == null)
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

                return;
            }

            if (targetNode == lastDragDropTarget)
            {
                if (DateTime.Now - lastDragDropDateTime >= new TimeSpan(5000000))
                {
                    targetNode.Expand();
                }

                return;
            }

            var sourceNode = GetDragDropSourceNode(e);

            if (IsDragDropValid(sourceNode, targetNode))
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

        private static JTokenTreeNode GetDragDropSourceNode(DragEventArgs e)
        {
            return e.Data.GetData(e.Data.GetFormats().FirstOrDefault(), true) as JTokenTreeNode;
        }

        private JTokenTreeNode GetDragDropTargetNode(DragEventArgs e)
        {
            var targetPoint = PointToClient(new Point(e.X, e.Y));
            var targetNode = GetNodeAt(targetPoint) as JTokenTreeNode;

            return targetNode;
        }

        private bool IsDragDropValid(JTokenTreeNode sourceNode, JTokenTreeNode targetNode)
        {
            if (sourceNode == null || targetNode == null)
            {
                return false;
            }

            if (sourceNode.JTokenTag is JProperty)
            {
                return targetNode.JTokenTag is JObject;
            }
            if (sourceNode.JTokenTag is JObject)
            {
                return targetNode.JTokenTag is JProperty || targetNode.JTokenTag is JArray;
            }
            if (sourceNode.JTokenTag is JArray)
            {
                return targetNode.JTokenTag is JArray;
            }

            return false;
        }
    }
}
