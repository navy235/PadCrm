﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using Maitonn.Core;

namespace PadCRM.ViewModels
{
    public class RolesViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入角色名称")]
        [Display(Name = "角色名称")]
        [StringCheckLength(4, 25)]
        public string Name { get; set; }

        [Required(ErrorMessage = "请输入角色描述")]
        [Display(Name = "角色描述")]
        [StringCheckLength(4, 75)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "角色权限")]
        [Required(ErrorMessage = "请选择角色权限")]
        [UIHint("GroupForeignKeyForCheckBox")]
        public string Permissions { get; set; }
    }
}