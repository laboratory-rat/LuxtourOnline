using LuxtourOnline.Models.Products;
using LuxtourOnline.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LuxtourOnline.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class DocumentController : BaseAppController, IDisposable
    {
        // GET: Document
        public async Task<ActionResult> LoadFileToOrderJson(string id)
        {
            if (Request.Files == null || Request.Files.Count == 0 || Request.Files[0] == null || Request.Files[0].ContentLength < 1)
                return null;

            try
            {

                var doc = Request.Files[0];
                Order order = _context.Orders.Where(x => x.Id == id).FirstOrDefault();

                if (order == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                var document = DocumentsMaster.UploadDocument(doc, order);

                if(document == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                _context.Documents.Add(document);
                await _context.SaveChangesAsync();

                return Json(document.Name, JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ActionResult> RemoveFileFromOrderJson(int id)
        {
            try
            {
                var doc = _context.Documents.Where(x => x.Id == id).FirstOrDefault();

                if (doc == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                DocumentsMaster.RemoveDocument(doc);
                _context.Documents.Remove(doc);
                await _context.SaveChangesAsync();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}