using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadCRM.ViewModels
{
    public class TreeViewItemViewModel
    {

        public TreeViewItemViewModel()
        {
            this.items = new List<TreeViewItemViewModel>();
        }


        public string id { get; set; }

        public string text { get; set; }

        public bool expanded { get; set; }

        public string spriteCssClass { get; set; }

        public List<TreeViewItemViewModel> items { get; set; }
    }
}