using LuxtourOnline.Utilites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Models.TelGrub
{

    public enum TelGrubStatus { Null = 0, New, Grab}
    public class TelGrubModel
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string FullName { get; set; }
        public string TelNumber { get; set; }

        public TelGrubStatus Status { get; set; } = TelGrubStatus.New;
        public string GrubKey { get; set; }

        [Required]
        public virtual AppUser Operator { get; set; } = null;
        public DateTime? GrubTime { get; set; }

        protected static int KeyLength = 15;

        public TelGrubModel()
        {

        }

        public static TelGrubModel Create(TelGrubNewModel model)
        {
            if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.TelNumber))
                return null;

            string key = IdGenerator.GenerateId().Substring(0, KeyLength);
            TelGrubStatus status = TelGrubStatus.New;
            return new TelGrubModel()
            {
                CreatedTime = DateTime.Now,
                GrubKey = key,
                Status = status,
                FullName = model.FullName,
                TelNumber = model.TelNumber,
                GrubTime = null,
            };
        }

        public void Grub (AppUser oper)
        {
            GrubTime = DateTime.Now;
            Operator = oper;

            Status = TelGrubStatus.Grab;
        }
    }

    public class TelGrubNewModel
    {
        public string FullName { get; set; }
        public string TelNumber { get; set; }
    }

    public class TelGrubListModel
    {
        public List<TelGrubModel> Models { get; set; }
        public PagingInfo Paging { get; set; }

        public TelGrubListModel()
        {

        }

        public TelGrubListModel(List<TelGrubModel> models, int currentPage = 1, int perPage = 15)
        {
            Models = models;

            PagingInfo info = new PagingInfo() { CurrentPange = currentPage, ItemsPerPage = perPage, TotalItems = models.Count };
            Paging = info;
        }
    }

    public class TelGrubOperators
    {
        public int Id { get; set; }

        public string OperatorName { get; set; }
        public string OperatorEmail { get; set; }

        public virtual List<TelGrubModel> Grubs { get; set; } = new List<TelGrubModel>();

        public TelGrubOperators()
        {
            Grubs = new List<TelGrubModel>();
        }

        public TelGrubOperators(string fullName, string Email) : this()
        {
            OperatorName = fullName;
            OperatorEmail = Email;
        }

        public TelGrubOperators(AppUser user) : this()
        {
            OperatorName = user.FullName;
            OperatorEmail = user.Email;

            
        }
    }

    public class TelGrubOperatorsList
    {
        public List<TelGrubOperators> Operators { get; set; }
        public PagingInfo Paging { get; set; }

        public TelGrubOperatorsList(List<TelGrubOperators> models, int currentPage = 1, int perPage = 15)
        {
            Operators = models;

            PagingInfo info = new PagingInfo() { CurrentPange = currentPage, ItemsPerPage = perPage, TotalItems = models.Count };
            Paging = info;
        }
    }

}