﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiguelDesafioTecnico.Models;

using Rotativa;
using Rotativa.Options;

namespace MiguelDesafioTecnico.Controllers
{
    public class GeraPdfController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GeraPdf
        public ActionResult Index()
        {
            try
            {
                var tt = (from r in db.Receitas select r.Valor).Sum();

                var t = (from r in db.Despesas select r.Valor).Sum();

                ViewBag.TotalReceitas =("RECEITAS R$ "+tt);
                ViewBag.TotalDespesas =("DESPESAS R$ "+t);

                double total;
                double SomaA = Double.Parse(tt.ToString());
                double SomaB = Double.Parse(t.ToString());
                total = SomaA + SomaB;

                ViewBag.TotalGeral = ("R$ " + total);


                ViewBag.Msg = ("Os dados foram Calculados!");
            }
            catch (Exception ex)
            {

                ViewBag.Msg = ("Não foi possivél Calcular os dados!");

            }
            return new Rotativa.ViewAsPdf(db.GeraPdfs.ToList());
            //return View(db.GeraPdfs.ToList());
        }

        // GET: GeraPdf/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeraPdf geraPdf = db.GeraPdfs.Find(id);
            if (geraPdf == null)
            {
                return HttpNotFound();
            }
            return View(geraPdf);
        }

        // GET: GeraPdf/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GeraPdf/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GeraPdfId")] GeraPdf geraPdf)
        {
            if (ModelState.IsValid)
            {
                db.GeraPdfs.Add(geraPdf);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(geraPdf);
        }

        // GET: GeraPdf/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeraPdf geraPdf = db.GeraPdfs.Find(id);
            if (geraPdf == null)
            {
                return HttpNotFound();
            }
            return View(geraPdf);
        }

        // POST: GeraPdf/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GeraPdfId")] GeraPdf geraPdf)
        {
            if (ModelState.IsValid)
            {
                db.Entry(geraPdf).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(geraPdf);
        }

        // GET: GeraPdf/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeraPdf geraPdf = db.GeraPdfs.Find(id);
            if (geraPdf == null)
            {
                return HttpNotFound();
            }
            return View(geraPdf);
        }

        // POST: GeraPdf/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GeraPdf geraPdf = db.GeraPdfs.Find(id);
            db.GeraPdfs.Remove(geraPdf);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
