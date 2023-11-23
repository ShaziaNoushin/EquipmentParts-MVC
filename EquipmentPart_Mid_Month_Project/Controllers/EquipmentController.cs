using EquipmentPart_Mid_Month_Project.Models;
using EquipmentPart_Mid_Month_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;
using X.PagedList.Mvc;
namespace EquipmentPart_Mid_Month_Project.Controllers
{
    [Authorize]
    public class EquipmentController : Controller
    {
        EquipmentDbContext db = new EquipmentDbContext();
        // GET: Equipment
        [AllowAnonymous]
        public ActionResult Index(int pg=1)
        {
            return View(db.Equipments.OrderBy(x => x.EquipmentId).ToPagedList(pg, 5));
        }
        ////Create
        public ActionResult Create()
        {
            var data = new EquipmentInputModel();
            data.Parts.Add(new Part());
            
            
            return View(data);
        }
        [HttpPost]
        public ActionResult Create(EquipmentInputModel data, string act = "")
        {
            if (act == "add")
            {
                data.Parts.Add(new Part());
                foreach (var v in ModelState.Values)
                {
                    v.Errors.Clear();
                }
            }
            if (act.StartsWith("remove"))
            {
                var index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Parts.RemoveAt(index);

                foreach (var v in ModelState.Values)
                {
                    v.Errors.Clear();
                }
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    Equipment e = new Equipment
                    {
                        EquipmentName = data.EquipmentName,
                        DeliveryDate = data.DeliveryDate,
                        Price = data.Price,
                        Available = data.Available,
                       
                    };
                    string ext = Path.GetExtension(data.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(Server.MapPath("~/Pictures"), fileName);
                    data.Picture.SaveAs(savePath);
                    e.Picture = fileName;
                    foreach (var p in data.Parts)
                    {
                        e.Parts.Add(p);
                    }
                    db.Equipments.Add(e);
                    db.SaveChanges();
                    
                }
                
            }
            ViewData["Act"] = act;
            return PartialView("_CreatePartial", data);
        }
        public PartialViewResult AddQuationToForm(EquipmentInputModel e = null, int? index = null)
        {
            if (e == null) e = new EquipmentInputModel();
            if (index.HasValue)
            {
                if (index < e.Parts.Count)
                {
                    e.Parts.RemoveAt(index.Value);
                }
            }
            else
            {
                e.Parts.Add(new Part());
            }
            return PartialView("_PartForm", e);
        }
        public ActionResult ImageUpload(int id, ImageUpload pic)
        {
            if (ModelState.IsValid)
            {
                if (pic.Picture != null)
                {
                    Equipment e = db.Equipments.First(x => x.EquipmentId == id);
                    string ext = Path.GetExtension(pic.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(Server.MapPath("~/Pictures"), fileName);
                    pic.Picture.SaveAs(savePath);
                    e.Picture = fileName;
                    db.SaveChanges();
                    return Json(fileName);

                }
            }
            return Json(null);
        }
        public ActionResult Edit(int id)
        {

            var equipment = db.Equipments.Include(c => c.Parts).First(c => c.EquipmentId == id);
            var data = new EquipmentEditModel
            {
                EquipmentId = equipment.EquipmentId,
                EquipmentName = equipment.EquipmentName,
                DeliveryDate = equipment.DeliveryDate,
                Price = equipment.Price,
                Available = equipment.Available,

                Parts = equipment.Parts.ToList()
            };
            ViewData["CurrentPic"] = equipment.Picture;
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(EquipmentEditModel data, string act="")
        {
            var existing = db.Equipments.First(c => c.EquipmentId == data.EquipmentId);
            if (act == "add")
            {
                data.Parts.Add(new Part());
                foreach (var v in ModelState.Values)
                {
                    v.Errors.Clear();
                }
            }
            if (act.StartsWith("remove"))
            {
                var index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Parts.RemoveAt(index);

                foreach (var v in ModelState.Values)
                {
                    v.Errors.Clear();
                }
            }
            if (act == "update")
            {
                if (ModelState.IsValid)
                {

                    existing.EquipmentName = data.EquipmentName;
                    existing.DeliveryDate = data.DeliveryDate;
                    existing.Price = data.Price;
                    existing.Available = data.Available;

                    
                    if(data.Picture != null)
                    {
                        string ext = Path.GetExtension(data.Picture.FileName);
                        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Path.Combine(Server.MapPath("~/Pictures"), fileName);
                        data.Picture.SaveAs(savePath);
                        existing.Picture = fileName;
                    }
                    db.Parts.RemoveRange(db.Parts.Where(x => x.EquipmentId == data.EquipmentId).ToList());
                    foreach (var item in data.Parts)
                    {
                        existing.Parts.Add(new Part
                        {
                            PartName = item.PartName,
                            Quantity = item.Quantity

                        });
                    }
                  
                    db.SaveChanges();

                }

            }
            @ViewData["Act"] = act;
            return PartialView("_EditPartial", data);
        }
        public PartialViewResult AddQuationToEditForm(EquipmentEditModel c, int? index = null)
        {

            if (index.HasValue)
            {
                if (index < c.Parts.Count)
                {
                    c.Parts.RemoveAt(index.Value);
                }
            }


            return PartialView("_PartEditForm", c);
        }
        public PartialViewResult AddMore(EquipmentEditModel c, int? index = null)
        {
            if (c == null) c = new EquipmentEditModel();
            if (index.HasValue)
            {
                if (index < c.Parts.Count)
                {
                    c.Parts.RemoveAt(index.Value);
                }
            }
            else
            {
                c.Parts.Add(new Part());
            }

            return PartialView("_QualificationEditForm", c);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var existing = db.Equipments.FirstOrDefault(c => c.EquipmentId == id);
            if (existing != null)
            {
                db.Equipments.Remove(existing);
                db.SaveChanges();
                return Json(existing.EquipmentId);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}