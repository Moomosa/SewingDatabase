using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.Shared
{
    public class TableHeaderModel
    {
        public string SortProperty { get; set; }
        public string CurrentSort { get; set; }
        public string SortDirection { get; set; }
        public string HeaderText { get; set; }
        public bool HideOnMobile { get; set; }

        public TableHeaderModel(string sortProperty, string headerText, string currentSort, string sortDirection, bool hideOnMobile)
        {
			SortProperty = sortProperty;
			HeaderText = headerText;
			CurrentSort = currentSort;
			SortDirection = sortDirection;
            HideOnMobile = hideOnMobile;
		}
    }
}
