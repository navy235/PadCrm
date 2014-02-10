using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface IRelationCateService
    {
        IQueryable<RelationCate> GetALL();

        IQueryable<RelationCate> GetKendoALL();

        void Create(RelationCate model);

        void Update(RelationCate model);

        void Delete(RelationCate model);

        RelationCate Find(int ID);
    }
}