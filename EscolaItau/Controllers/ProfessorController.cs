using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EscolaItau.Models;

namespace EscolaItau.Controllers
{
    public class ProfessorController : Controller
    {
        private escolaEntities db = new escolaEntities();

        // GET: Professor
        public ActionResult Index()
        {
            return View(db.Professors.OrderBy(p => p.nome).ToList());
        }

        // GET: Professor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        //Verificação de Duplicidade do nome
        public bool Duplicado(Professor p)
        {
            return db.Professors.Where(s => s.nome == p.nome).Any();
        }

        //Retorna erro de duplicidade
        public ActionResult DuplicidadeError()
        {
            ModelState.AddModelError("nome", "Este nome já está cadastrado");
            return RedirectToAction("Create");
        }

        // GET: Professor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Professor/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "professor_id,nome")] Professor professor)
        {
            //Chama o validador de duplicidade
            if (Duplicado(professor))
            {
                DuplicidadeError();
            }

            if (ModelState.IsValid)
            {
                db.Professors.Add(professor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(professor);
        }

        // GET: Professor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        // POST: Professor/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "professor_id,nome")] Professor professor)
        {
            //Chama o validador de duplicidade
            if (Duplicado(professor))
            {
                DuplicidadeError();
            }

            if (ModelState.IsValid)
            {
                db.Entry(professor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(professor);
        }

        // GET: Professor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        // POST: Professor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //lista os quadros onde aquele professor está relacionado e deleta a lista
            List<Quadro> quadroList = db.Quadroes.Where(s => s.fk_professor_id == id).ToList();
            db.Quadroes.RemoveRange(quadroList);
            //busca pelo professor selecionado e deleta
            Professor professor = db.Professors.Find(id);
            db.Professors.Remove(professor);

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
