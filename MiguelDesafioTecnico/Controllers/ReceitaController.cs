using System;
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
using System.Collections;

namespace MiguelDesafioTecnico.Controllers
{
    public class ReceitaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Receita
        public ActionResult Index(string data, string categoria)
        {
            
            ViewBag.Categoria = (from r in db.Receitas
                                 select r.Categoria).Distinct();

            ViewBag.Data = (from r in db.Receitas
                            select r.Data).Distinct();

            var model = from r in db.Receitas
                        orderby r.ReceitaId
                        where r.Categoria == categoria || categoria.Equals(null) || categoria.Equals("")
                        where r.Data == data || data.Equals(null) || data.Equals("")
                        select r;

            return View(model);
           // return View(db.Receitas.ToList());     
        }
 
        // GET: Receita/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receita receita = db.Receitas.Find(id);
            if (receita == null)
            {
                return HttpNotFound();
            }
            return View(receita);
        }

        // GET: Receita/Create
        public ActionResult Create()
        {
            //var verificar = db.Receitas.Where(r => r.Categoria == "ALUMINIO");
            //ViewBag.Dados = verificar;
            return View();
        }

        // POST: Receita/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReceitaId,Valor,Categoria,Data,Observacao")] Receita receita)
        {
           
           if (ModelState.IsValid)
           {

               // long DATA = receita.Data;
               // string RGFormatado = String.Format(@"{0:00/00/0000}",DATA);
               // receita.Data = RGFormatado;
                try
                {
                  
                    var buscar = (from r in db.Receitas select r.Categoria);
                 
                    var resultado =
                    from string p in buscar
                    where p == receita.Categoria
                    select p; 

                    //  ViewBag.Dados = resultado.ToString();

                    buscar.Distinct().ToList();
                    resultado.ForEachAsync(n => TransformaUpperCase(n));

                    if (receita.Observacao == null)
                    {
                        ViewBag.Messagem = "  O Campo Observação estar em branco";

                        if (!resultado.Equals(""))
                        {
                            ViewBag.Erro = "categoria já cadastrada ";
                        }
                        
                        if (!receita.Data.Equals(string.Format("dd/mm/yyyy")))
                        {
                            ViewBag.Messagem =  "Verifique o formato data ex: DD/MM/AAAA";
                        }
                        if (receita.Categoria == null)
                        {
                            ViewBag.Messagem = "  O Campo Categoria estar em branco";
                        }
                        
                        if (receita.Data == null)
                        {
                            ViewBag.Messagem = "  O Campo data estar em branco";
                        }
                        if ((receita.Data.Length < 10) || (receita.Data.Length > 10))
                        {
                            ViewBag.Messagem = "  O Campo Data deve conter 10 digitos";
                        }
                        
                        if (!receita.Data.Substring(2, 1).Equals("/"))
                        {
                            ViewBag.Messagem = "  O Campo Data deve conter / no 2º digito - " + receita.Data.Substring(2, 1);
                        }
                        if (!receita.Data.Substring(5, 1).Equals("/"))
                        {
                            ViewBag.Messagem = "  O Campo Data deve conter / no 5º digito  -  " + receita.Data.Substring(5, 1);
                        }
                    }
                    else
                    {

                        db.Receitas.Add(receita);
                        db.SaveChanges();
                        return RedirectToAction("Index");
               
                    }

            }
                catch (Exception ex)
                {
                    ViewBag.Sistema = "Entre em contato com o administrador do sistema";
                }
            }
            return View(receita);
        }

        public void TransformaUpperCase(string nome)
        {
            ViewBag.Dados = (nome.ToUpper());

        }

        // GET: Receita/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receita receita = db.Receitas.Find(id);
            if (receita == null)
            {
                return HttpNotFound();
            }
            return View(receita);
        }

        // POST: Receita/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReceitaId,Valor,Categoria,Data,Observacao")] Receita receita)
        {
            if (ModelState.IsValid)
            {
                db.Entry(receita).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(receita);
        }

        // GET: Receita/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receita receita = db.Receitas.Find(id);
            if (receita == null)
            {
                return HttpNotFound();
            }
            return View(receita);
        }

        // POST: Receita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Receita receita = db.Receitas.Find(id);
            db.Receitas.Remove(receita);
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
