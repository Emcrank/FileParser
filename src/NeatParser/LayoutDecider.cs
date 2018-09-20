using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeatParser
{
    /// <summary>
    /// Class that can apply rules to determine which layout should be used for the read row.
    /// </summary>
    public class LayoutDecider
    {
        /// <summary>
        /// Gets the current layout. (the one that was last decided or the default)
        /// </summary>
        public Layout Current => currentLayout ?? defaultLayout;

        private readonly IDictionary<Layout, Func<string, bool>> assignedLayouts =
            new Dictionary<Layout, Func<string, bool>>();

        private Layout currentLayout;
        private Layout defaultLayout;

        /// <summary>
        /// Constructs a new instance of the <see cref="LayoutDecider"/> class.
        /// </summary>
        /// <param name="defaultLayout"></param>
        public LayoutDecider(Layout defaultLayout)
        {
            AssignDefault(defaultLayout);
        }

        /// <summary>
        /// Assigns the default layout for the selector. Chosen last if no other match is found.
        /// </summary>
        /// <param name="layout">Default layout to use.</param>
        public void AssignDefault(Layout layout)
        {
            if (layout == null)
                throw new ArgumentNullException(nameof(layout));

            defaultLayout = layout;
        }

        /// <summary>
        /// Assigns a layout with a func to determine if it should be used.
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="whenFunc"></param>
        public void AssignWhen(Layout layout, Func<string, bool> whenFunc)
        {
            if (layout == null)
                throw new ArgumentNullException(nameof(layout));

            if (whenFunc == null)
                throw new ArgumentNullException(nameof(whenFunc));

            if (assignedLayouts.ContainsKey(layout))
                throw new ArgumentException("You have already assigned this layout.");

            assignedLayouts.Add(layout, whenFunc);
        }

        /// <summary>
        /// Decides and gets the layout that is to be used.
        /// </summary>
        /// <param name="data">Data to be decided on.</param>
        /// <returns>A file layout that should be used.</returns>
        public Layout Decide(string data)
        {
            if (defaultLayout == null && !assignedLayouts.Any())
                throw new InvalidOperationException("No layouts's have been assigned to the decider.");

            currentLayout = DecideInternal(data);
            currentLayout.Reset();
            return currentLayout;
        }

        /// <summary>
        /// Decides and gets the layout that is to be used.
        /// </summary>
        /// <param name="data">Data to be decided on.</param>
        /// <returns>A file layout that should be used.</returns>
        public Layout Decide(StringBuilder data)
        {
            return Decide(data.ToString());
        }

        /// <summary>
        /// Gets the layout that matches the string data given.
        /// </summary>
        /// <param name="data">Data to be decided on.</param>
        /// <returns>A file layout that should be used.</returns>
        private Layout DecideInternal(string data)
        {
            foreach (var layout in assignedLayouts.Keys)
            {
                var whenFunc = assignedLayouts[layout];
                if (whenFunc(data))
                    return layout;
            }

            return defaultLayout;
        }
    }
}