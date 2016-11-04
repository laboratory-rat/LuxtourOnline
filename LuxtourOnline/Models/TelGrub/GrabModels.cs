using LuxtourOnline.Utilites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Models.TelGrub
{

    public enum TelGrubStatus { Null = 0, New, Grab, Success, Fail}
    public class TelGrubModel
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string FullName { get; set; }
        public string TelNumber { get; set; }

        public TelGrubStatus Status { get; set; } = TelGrubStatus.New;
        public string GrubKey { get; set; }

        public string Language { get; set; } = "uk";

        public string Ip { get; set; } = "";

        public virtual AppUser Operator { get; set; } = null;
        public DateTime? GrubTime { get; set; }

        public string Comment { get; set; } = "";

        protected static int KeyLength = 15;

        public TelGrubModel()
        {

        }


        public static TelGrubModel Create(TelGrubNewModel model, string language)
        {
            if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.TelNumber))
                return null;

            string key = IdGenerator.GenerateId().Substring(0, KeyLength);
            TelGrubStatus status = TelGrubStatus.New;
            string ip = HttpContext.Current.Request.UserHostAddress;


            return new TelGrubModel()
            {
                CreatedTime = DateTime.Now,
                GrubKey = key,
                Status = status,
                FullName = model.FullName,
                TelNumber = model.TelNumber,
                GrubTime = null,
                Ip = ip,
                Language = language,
            };
        }

        public void Grub (AppUser oper)
        {
            GrubTime = DateTime.Now;
            Operator = oper;

            Status = TelGrubStatus.Grab;
        }
        
        public void Result(bool success, string comment)
        {
            if (success)
                Status = TelGrubStatus.Success;
            else
                Status = TelGrubStatus.Fail;

            Comment = comment;
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

    public class TelGrubModelDisplay
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string TelNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public TelGrubStatus Status { get; set; }

        public int MinutesFrom {
            get { return DateTime.Now.Subtract(CreateTime).Minutes; }
        }

        public LocationModel Location { get; set; }

        public TelGrubModelDisplay()
        {

        }

        public TelGrubModelDisplay(TelGrubModel model)
        {
            Id = model.Id;
            FullName = model.FullName;
            TelNumber = model.TelNumber;
            CreateTime = model.CreatedTime;
            Status = model.Status;


            if (model.Ip != "")
                Location = LocationMaster.GetLocation(model.Ip);
        }
    }


}