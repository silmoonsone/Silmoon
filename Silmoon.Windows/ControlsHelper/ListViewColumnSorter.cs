using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Silmoon.Windows.ControlsHelper
{
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;
        /// <summary>
        /// ÔÚÅÅÐòÊ±ÐèÒªºöÂÔµÄÅÅÐò×Ö·û
        /// </summary>
        public string ReplaceString = string.Empty;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            string xString = listviewX.SubItems[ColumnToSort].Text;
            string yString = listviewY.SubItems[ColumnToSort].Text;

            if (ReplaceString != string.Empty)
            {
                xString = listviewX.SubItems[ColumnToSort].Text.Replace(ReplaceString, "");
                yString = listviewY.SubItems[ColumnToSort].Text.Replace(ReplaceString, "");
            }

            ulong xInt;
            ulong yInt;
            bool IsxInt = ulong.TryParse(xString, out xInt);
            bool IsyInt = ulong.TryParse(yString, out yInt);

            DateTime xDate;
            DateTime yDate;
            bool IsxDate = DateTime.TryParse(xString, out xDate);
            bool IsyDate = DateTime.TryParse(yString, out yDate);

            // Compare the two items
            if (IsxInt && IsyInt)
                compareResult = ObjectCompare.Compare(xInt, yInt);
            else if (IsxDate && IsyDate)
                compareResult = ObjectCompare.Compare(xDate, yDate);
            else
                compareResult = ObjectCompare.Compare(xString, yString);

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
                return compareResult;
            else if (OrderOfSort == SortOrder.Descending)
                return (-compareResult);
            else
                return 0;
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }
}
