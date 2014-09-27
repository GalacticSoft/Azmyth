using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;


namespace XnaGUILib
{
    public delegate void XGTreeItemClicked(XGTreeItem item);


    public class XGTree : XGPanel
    {
        public int ItemHeight { get; set; }
        public readonly XGTeeItemCollection Items = new XGTeeItemCollection();
        XGVScrollbar scrollBar;
        public XGTreeItemClicked TreeItemClicked;
        public XGTreeItem SelectedItem { get; set; }

        public XGTree(Rectangle rect)
            : base(rect)
        {
            Construct();
        }

        public XGTree(Rectangle rect, XGTreeItemClicked itemClickedEvent)
            : base(rect)
        {
            Construct();
            TreeItemClicked += itemClickedEvent;
        }

        void Construct()
        {
            Items.Tree = this;

            scrollBar = new XGVScrollbar(new Rectangle(Rectangle.Width - 10, 1, 10, Rectangle.Height - 2), this.Scrollbar_ValueChanged);
            scrollBar.Scale = 100.0f;
            this.Children.Add(scrollBar);

            ItemHeight = XnaGUIManager.spriteFont.LineSpacing; // by default
        }

        public void ExpandAll()
        {
            foreach (XGTreeItem item in Items)
            {
                item.ExpandAll();
            }
        }

        int totalItemCount = 0;

        public int CountNumberOfVisibleItems()
        {
            totalItemCount = 0;
            foreach (XGTreeItem item in Items)
                totalItemCount += item.CountNumberOfVisibleItems();
            return totalItemCount;
        }

        public XGTreeItem GetNthVisibleItem(int count)
        {
            foreach (XGTreeItem item in Items)
            {
                XGTreeItem obj = item.GetNthVisibleItem(ref count);
                if (obj != null)
                    return obj;
            }
            return null;
        }

        public override void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);

            if (XnaGUIManager.HasFocus(this) == false)
                return;

            Point mousePoint = new Point(XnaGUIManager.mouseState.X, XnaGUIManager.mouseState.Y);

            Rectangle rect = ToScreen(Rectangle);

            if (rect.Contains(mousePoint))
            {

                if (XnaGUIManager.mouseState.LeftButton == ButtonState.Pressed && XnaGUIManager.prevMouseState.LeftButton == ButtonState.Released)
                {
                    rect.X += 4;
                    rect.Width -= 10 + 4; // shorten by scroll bar width + left padding
                    rect.Y += 4;
                    rect.Height = ItemHeight;

                    drawingContext.DoDraw = drawingContext.FirstItem == null; // true if null, else false

                    foreach (XGTreeItem item in Items)
                    {
                        if (item.Update(frameTime, mousePoint, ref rect, drawingContext))
                            break; // handled
                    }
                }
            }
            /*
            // I'm not Sure that keyboard selection here makes sense, so we'll wait on this....hmmm
            if (KeyJustPressed(Keys.Up, false, false, false))
            {
                SelectPrevious(false);
            }
            if (KeyJustPressed(Keys.Down, false, false, false))
            {
                SelectNext(false);
            }
            */
        }

        void SelectNext(bool autoExpand)
        {
            if (SelectedItem == null)
            {
                if (Items.Count > 0)
                {
                    SelectedItem = Items[0];
                    EnsureSelectedItemVisible();
                    return;
                }
            }
            // Select first child of current item if available
            if (SelectedItem.Items.Count > 0 && SelectedItem.Expanded)
            {
                SelectedItem = SelectedItem.Items[0];
                EnsureSelectedItemVisible();
                return;
            }

            // walk the tree from SelectedItem;

            XGTreeItem selItem = null;
            XGTreeItem parent = SelectedItem.Parent;
            XGTreeItem lastParent = SelectedItem;
            while (parent != null)
            {
                int index;
                for (index = 0; index < parent.Items.Count; index++)
                {
                    if (parent.Items[index] == SelectedItem)
                    {
                        index++;
                        break;
                    }
                }
                if (index < parent.Items.Count)
                {
                    selItem = parent.Items[index]; // We have the next one!
                    break;
                }
                lastParent = parent;
                parent = parent.Parent;
            }
            if (selItem == null)
            {
                // we came out, so we have to go to our next item
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i] == lastParent)
                    {
                        if (i + 1 < Items.Count)
                        {
                            selItem = Items[i + 1];
                            break;
                        }
                        break;
                    }
                }
            }

            if (selItem != null)
            {
                SelectedItem = selItem;
                EnsureSelectedItemVisible();
            }
        }

        void SelectPrevious(bool autoExpand)
        {
        }

        void EnsureSelectedItemVisible()
        {
        }


        XGTreeDrawingContext drawingContext = new XGTreeDrawingContext();


        public override void Draw(float frameTime)
        {
            base.Draw(frameTime); // Let XGPanel draw the borders and background

            scrollBar.Scale = CountNumberOfVisibleItems() - 1.0f;
            if (scrollBar.Scale < 1.0f)
                scrollBar.Scale = 1.0f;

            Rectangle rect = ToScreen(Rectangle);
            rect.X += 4; // left padding
            rect.Width -= 10 + 4; // shorten by scroll bar width + left padding
            rect.Y += 4;
            rect.Height = ItemHeight;

            drawingContext.DoDraw = drawingContext.FirstItem == null; // true if null, else false

            foreach (XGTreeItem item in Items)
            {
                if (!item.Draw(frameTime, ref rect, drawingContext))
                    return; // stop drawing items
            }
        }

        void Scrollbar_ValueChanged(XGControl sender)
        {
            XGVScrollbar scrollBar = sender as XGVScrollbar;
            if (scrollBar == null)
                return;

            int index = (int )scrollBar.Value;
            drawingContext.FirstItem = GetNthVisibleItem(index);
        }

        internal void NotifyTreeItemClicked(XGTreeItem item, bool expanded)
        {
            scrollBar.Scale = CountNumberOfVisibleItems() - 1.0f;
            if (scrollBar.Scale < 1.0f)
                scrollBar.Scale = 1.0f;

            if (!expanded)
            {
                XGTreeItemClicked handler = TreeItemClicked;
                if (handler != null)
                    TreeItemClicked(item);
            }
        }
    }

    public class XGTreeDrawingContext
    {
        public XGTreeItem FirstItem;
        public bool DoDraw;

        public XGTreeDrawingContext()
        {
            FirstItem = null;
            DoDraw = false;
        }
    }

    public class XGTeeItemCollection : List<XGTreeItem>
    {
        internal XGTree Tree;
        internal XGTreeItem Parent;

        internal XGTeeItemCollection()
        {
            Tree = null;
            Parent = null;
        }

        internal XGTeeItemCollection(XGTree tree)
        {
            Tree = tree;
            Parent = null;
        }

        public new void Add(XGTreeItem item)
        {
            item.Root = Tree;
            item.Parent = Parent;
            base.Add(item);
        }

        public new void Insert(int index, XGTreeItem item)
        {
            item.Root = Tree;
            item.Parent = Parent;
            base.Insert(index, item);
        }
    }


    public class XGTreeItem
    {
        private XGTree _root;
        public XGTreeItem Parent = null;
        public XGTree Root 
        {
            get { return _root; }
            internal set
            {
                _root = value;
                Items.Tree = value;
            } 
        }
        public object Value { get; set; }
        public Texture2D Image { get; set; }
        public object Tag { get; set; }
        public bool Expanded { get; set; }

        public readonly XGTeeItemCollection Items = new XGTeeItemCollection();

        public XGTreeItem(object value)
        {
            Value = value;
            Image = null; 
            Tag = null;
            Expanded = false;
            Items.Parent = this;
        }

        public XGTreeItem(object value, Texture2D image)
        {
            Value = value;
            Image = image;
            Tag = null;
            Expanded = false;
            Items.Parent = this;
        }

        public void ExpandAll()
        {
            Expanded = true;
            foreach (XGTreeItem item in Items)
            {
                item.ExpandAll();
            }
        }

        /// <summary>
        /// Called by XGTree.Update when mouse is down in it's control area
        /// (Will call child items Update if this item is Expanded)
        /// </summary>
        /// <param name="frameTime"></param>
        /// <param name="mousePoint"></param>
        /// <param name="rect">Rectangle area for TreeItem, gets modified during propagation.</param>
        /// <returns>true if handled to stop propagation to other items</returns>
        internal bool Update(float frameTime, Point mousePoint, ref Rectangle rect, XGTreeDrawingContext context)
        {
            bool haveChildren = Items.Count > 0;
            bool updateChildren = Expanded && haveChildren;

            if (context.FirstItem == this)
                context.DoDraw = true; // draw this and subsequent items

            if (context.DoDraw) // if we can draw we can be clicked on
            {
                Rectangle expRect = GetExpandRect(rect);
                if (expRect.Contains(mousePoint))
                {
                    Expanded = !Expanded;
                    Root.NotifyTreeItemClicked(this, true);
                    return true;
                }

                Rectangle textRect = rect;
                textRect.X += expRect.Width;
                textRect.Width -= expRect.Width;

                if (textRect.Contains(mousePoint))
                {
                    Root.SelectedItem = this;
                    Root.NotifyTreeItemClicked(this, false);
                    return true;
                }

                rect.Y += Root.ItemHeight;
            }

            if (updateChildren)
            {
                Rectangle childRect = rect;
                childRect.X += childRect.Height;
                childRect.Width -= childRect.Height;

                foreach (XGTreeItem item in Items)
                {
                    if (item.Update(frameTime, mousePoint, ref childRect, context))
                        return true;
                }

                rect.Y = childRect.Y;
            }
            return false;
        }

        /// <summary>
        /// Get the Nth displayable item in the list 
        /// (including children if Expanded = true)
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        internal XGTreeItem GetNthVisibleItem(ref int count)
        {
            if (count == 0)
                return this;

            count--; // -1 for us

            if (Expanded)
            {
                foreach (XGTreeItem item in Items)
                {
                    XGTreeItem firstItem = item.GetNthVisibleItem(ref count);
                    if (firstItem != null)
                        return firstItem;
                }
            }
            return null;
        }

        internal int CountNumberOfVisibleItems()
        {
            int count = 1; // this item
            if (Expanded)
            {
                foreach (XGTreeItem item in Items)
                {
                    count += item.CountNumberOfVisibleItems();
                }
            }
            return count;
        }

        internal bool Draw(float frameTime, ref Rectangle rect, XGTreeDrawingContext context)
        {
            Rectangle treeRect = Root.ToScreen(Root.Rectangle);
            if (!treeRect.Contains(rect))
                return false; // stop drawing items, we hit the bottom

            bool haveChildren = Items.Count > 0;
            bool drawChildren = Expanded && haveChildren;

            Rectangle expRect = GetExpandRect(rect);

            if (context.FirstItem == this)
                context.DoDraw = true; // draw this and subsequent items

            if (context.DoDraw)
            {
                // Draw the Expand Box if we have children

                if (haveChildren)
                {
                    XGControl.DrawBorder(expRect, 1, Root.ThisControlColor, true);
                    Rectangle pmRect = expRect;
                    pmRect.Inflate(-2, -2);
                    // DRAW + or -
                    Rectangle r = pmRect;
                    r.Y += r.Height / 2;
                    r.Height = 1;
                    XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, r, Root.ThisControlColor);
                    if (!drawChildren)
                    {
                        r = pmRect;
                        r.X += r.Width / 2;
                        r.Width = 1;
                        XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, r, Root.ThisControlColor);
                    }
                }
                // Draw our image, if supplied

                Rectangle textRect = rect;
                textRect.X += expRect.Width + 2;
                textRect.Width -= expRect.Width + 2;

                if (Image != null)
                {
                    Rectangle imageRect = textRect;
                    imageRect.Width = imageRect.Height;
                    textRect.X += imageRect.Width;
                    textRect.Width -= imageRect.Width;

                    XnaGUIManager.spriteBatch.Draw(Image, imageRect, Root.ThisForeColor); // Color.White);
                }

                // draw our Text

                if (Root.SelectedItem == this)
                {
                    XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, textRect, XGControl.ControlDarkColor);
                }

                Vector2 textPos = XGControl.GetTextPosition(textRect, Value.ToString(), GUIAlignment.Left | GUIAlignment.VCenter, Root.Padding);
                XnaGUIManager.spriteBatch.DrawString(XnaGUIManager.spriteFont, Value.ToString(), textPos, Root.ThisForeColor);

                rect.Y += Root.ItemHeight; // step down to next item
            }

            // Draw our children if expanded and have children

            if (drawChildren)
            {
                Rectangle childRect = rect;

                childRect.X += childRect.Height;
                childRect.Width -= childRect.Height;

                foreach (XGTreeItem item in Items)
                {
                    item.Draw(frameTime, ref childRect, context);
                }

                rect.Y = childRect.Y; // get the current Y from rendering child items
            }
            return true; // continue drawing
        }

        protected Rectangle GetExpandRect(Rectangle rect)
        {
            Rectangle expRect = rect;
            expRect.Height -= 3;
            expRect.Width = expRect.Height;
            expRect.Inflate(-1, -1);
            return expRect;
        }
    }
}
