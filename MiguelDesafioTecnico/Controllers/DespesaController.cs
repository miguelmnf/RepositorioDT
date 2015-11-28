using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiguelDesafioTecnico.Models;

namespace MiguelDesafioTecnico.Controllers
{
    public class DespesaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Despesa
        public ActionResult Index(string data, string categoria)
        {
            ViewBag.Categoria = (from d in db.Despesas
                                 select d.Categoria).Distinct();

            ViewBag.Data = (from d in db.Despesas
                            select d.Data).Distinct();

            var model = from d in db.Despesas
                        orderby d.DespesaId
                        where d.Categoria == categoria || categoria.Equals(null) || categoria.Equals("")
                        where d.Data == data || data.Equals(null) || data.Equals("")
                        select d;

            return View(model);
           // return View(db.Despesas.ToList());
        }

        // GET: Despesa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Despesa despesa = db.Despesas.Find(id);
            if (despesa == null)
            {
                return HttpNotFound();
            }
            return View(despesa);
        }

        // GET: Despesa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Despesa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DespesaId,Valor,Categoria,Data,Observacao")] Despesa despesa)
        {
            if (ModelState.IsValid)
            {
                 try
                {

                    var buscar = (from r in db.Despesas select r.Categoria);

                    var resultado =
                    from string p in buscar
                    where p == despesa.Categoria
                    select p;

                    //  ViewBag.Dados = resultado.ToString();

                    buscar.Distinct().ToList();
                    resultado.ForEachAsync(n => TransformaUpperCase(n));

                    if (despesa.Observacao == null)
                    {
                        ViewBag.Messagem = "  O Campo Observação estar em branco";

                        if (!resultado.Equals(""))
                        {
                            ViewBag.Erro = "categoria já cadastrada ";
                        }

                        if (!despesa.Data.Equals(string.Format("dd/mm/yyyy")))
                        {
                            ViewBag.Messagem = "Verifique o formato data ex: DD/MM/AAAA";
                        }
                        if (despesa.Categoria == null)
                        {
                            ViewBag.Messagem = "  O Campo Categoria estar em branco";
                        }
                        if (despesa.Data == null)
                        {
                            ViewBag.Messagem = "  O Campo data estar em branco";
                        }
                        if ((despesa.Data.Length < 10) || (despesa.Data.Length > 10))
                        {
                            ViewBag.Messagem = "  O Campo Data deve conter 10 digitos";
                        }
                        if (!despesa.Data.Substring(2, 1).Equals("/"))
                        {
                            ViewBag.Messagem = "  O Campo Data deve conter / no 2º digito - " + despesa.Data.Substring(2, 1);
                        }
                        if (!despesa.Data.Substring(5, 1).Equals("/"))
                        {
                            ViewBag.Messagem = "  O Campo Data deve conter / no 5º digito  -  " + despesa.Data.Substring(5, 1);
                        }
                    }
                    else
                    {
                db.Despesas.Add(despesa);
                db.SaveChanges();
                return RedirectToAction("Index");
                    }

                }
                 catch (Exception ex)
                 {
                     ViewBag.Sistema = "Entre em contato com o administrador do sistema";
                 }
            }
            return View(despesa);
        }
        public void TransformaUpperCase(string nome)
        {
            ViewBag.Dados = (nome.ToUpper());

        }
        // GET: Despesa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Despesa despesa = db.Despesas.Find(id);
            if (despesa == null)
            {
                return HttpNotFound();
            }
            return View(despesa);
        }

        // POST: Despesa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DespesaId,Valor,Categoria,Data,Observacao")] Despesa despesa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(despesa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(despesa);
        }

        // GET: Despesa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Despesa despesa = db.Despesas.Find(id);
            if (despesa == null)
            {
                return HttpNotFound();
            }
            return View(despesa);
        }

        // POST: Despesa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Despesa despesa = db.Despesas.Find(id);
            db.Despesas.Remove(despesa);
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
