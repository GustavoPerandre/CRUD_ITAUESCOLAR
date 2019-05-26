using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EscolaItau.Models;

namespace EscolaItau.Controllers
{
    public class DisciplinaController : Controller
    {
        private escolaEntities db = new escolaEntities();

        // GET: Disciplina
        public ActionResult Index()
        {
            return View(db.Disciplinas.OrderBy(d => d.titulo).ToList());
        }

        // GET: Disciplina/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina disciplina = db.Disciplinas.Find(id);
            if (disciplina == null)
            {
                return HttpNotFound();
            }
            return View(disciplina);
        }

        //Verificação de Duplicidade do título
        public bool Duplicado(Disciplina d)
        {
            return db.Disciplinas.Where(s => s.titulo == d.titulo).Any();
        }

        //Retorna erro de duplicidade
        public ActionResult DuplicidadeError()
        {
            ModelState.AddModelError("titulo", "Esta disciplina já está cadastrada");
            return RedirectToAction("Create");
        }

        // GET: Disciplina/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Disciplina/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "disciplina_id,titulo")] Disciplina disciplina)
        {
            //Chama o validador de duplicidade
            if (Duplicado(disciplina))
            {
                DuplicidadeError();
            }

            if (ModelState.IsValid)
            {
                db.Disciplinas.Add(disciplina);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(disciplina);
        }

        // GET: Disciplina/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina disciplina = db.Disciplinas.Find(id);
            if (disciplina == null)
            {
                return HttpNotFound();
            }
            return View(disciplina);
        }

        // POST: Disciplina/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "disciplina_id,titulo")] Disciplina disciplina)
        {
            //Chama o validador de duplicidade
            if (Duplicado(disciplina))
            {
                DuplicidadeError();
            }

            if (ModelState.IsValid)
            {
                db.Entry(disciplina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(disciplina);
        }

        // GET: Disciplina/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina disciplina = db.Disciplinas.Find(id);
            if (disciplina == null)
            {
                return HttpNotFound();
            }
            return View(disciplina);
        }

        // POST: Disciplina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //lista os quadros onde aquela disciplina está relacionada e deleta a lista
            List<Quadro> quadroList = db.Quadroes.Where(s => s.fk_disciplina_id == id).ToList();
            db.Quadroes.RemoveRange(quadroList);
            //busca pela disciplica selecionada e deleta
            Disciplina disciplina = db.Disciplinas.Find(id);
            db.Disciplinas.Remove(disciplina);

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
