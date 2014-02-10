using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
using PadCRM.Utils;
using Maitonn.Core;

namespace PadCRM.Service.Interface
{
    public interface IMemberService
    {
        IQueryable<Member> GetALL();

        IQueryable<Member> GetKendoALL();

        void Create(Member model);

        Member Create(MemberViewModel model);

        Member Update(MemberEditViewModel model);

        void Delete(Member model);

        Member Find(int ID);

        int Login(string UserName, string Md5Password);

        void SetLoginCookie(Member member);

        IQueryable<Member> GetBirthMemberInDays(int day);

        List<int> GetMemberIDs(int DepartmentID);

        string GetLunar(DateTime time);

        DateTime GetCalender(string lunar);

        bool ValidatePassword(int MemberID, string Password);

        void ResetPassword(Member member, string newpassword);

        bool ChangePassword(int MemberID, string oldpassword, string newpassword);

        void ChangeStatus(string ids, MemberCurrentStatus status);

        Member FindDescriptionMemberInLimitTime(string description, int limitHours);

        bool HasGetPasswordActionInLimitTime(string email, int limitMin, int memberAction);
    }
}