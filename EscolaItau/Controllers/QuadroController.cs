using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EscolaItau.Models;

namespace EscolaItau.Controllers
{
    public class QuadroController : Controller
    {
        private escolaEntities db = new escolaEntities();

        // GET: Quadro
        public ActionResult Index()
        {
            var quadroes = db.Quadroes.Include(a => a.Disciplina).Include(b => b.Professor).OrderBy(c => c.Disciplina.titulo);
            return View(quadroes.ToList());
        }

        // GET: Quadro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quadro quadro = db.Quadroes.Find(id);
            if (quadro == null)
            {
                return HttpNotFound();
            }
            return View(quadro);
        }

        //Verificação de Duplicidade do nome
        public bool Duplicado(Quadro q)
        {
            return db.Quadroes.Where(a => a.fk_disciplina_id == q.fk_disciplina_id).Where(b => b.fk_professor_id== q.fk_professor_id).Any();
        }

        //Retorna erro de duplicidade
        public ActionResult DuplicidadeError()
        {
            ModelState.AddModelError("fk_professor_id", "Este professor já leciona esta matéria");
            return RedirectToAction("Create");
        }

        // GET: Quadro/Create
        public ActionResult Create()
        {
            ViewBag.fk_disciplina_id = new SelectList(db.Disciplinas, "disciplina_id", "titulo");
            ViewBag.fk_professor_id = new SelectList(db.Professors, "professor_id", "nome");
            return View();
        }

        // POST: Quadro/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "quadro_id,fk_professor_id,fk_disciplina_id")] Quadro quadro)
        {
            //Chama o validador de duplicidade
            if (Duplicado(quadro))
            {
                DuplicidadeError();
            }

            if (ModelState.IsValid)
            {
                db.Quadroes.Add(quadro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fk_disciplina_id = new SelectList(db.Disciplinas, "disciplina_id", "titulo", quadro.fk_disciplina_id);
            ViewBag.fk_professor_id = new SelectList(db.Professors, "professor_id", "nome", quadro.fk_professor_id);
            return View(quadro);
        }

        // GET: Quadro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quadro quadro = db.Quadroes.Find(id);
            if (quadro == null)
            {
                return HttpNotFound();
            }
            ViewBag.fk_disciplina_id = new SelectList(db.Disciplinas, "disciplina_id", "titulo", quadro.fk_disciplina_id);
            ViewBag.fk_professor_id = new SelectList(db.Professors, "professor_id", "nome", quadro.fk_professor_id);
            return View(quadro);
        }

        // POST: Quadro/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "quadro_id,fk_professor_id,fk_disciplina_id")] Quadro quadro)
        {
            //Chama o validador de duplicidade
            if (Duplicado(quadro))
            {
                DuplicidadeError();
            }

            if (ModelState.IsValid)
            {
                db.Entry(quadro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fk_disciplina_id = new SelectList(db.Disciplinas, "disciplina_id", "titulo", quadro.fk_disciplina_id);
            ViewBag.fk_professor_id = new SelectList(db.Professors, "professor_id", "nome", quadro.fk_professor_id);
            return View(quadro);
        }

        // GET: Quadro/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quadro quadro = db.Quadroes.Find(id);
            if (quadro == null)
            {
                return HttpNotFound();
            }
            return View(quadro);
        }

        // POST: Quadro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quadro quadro = db.Quadroes.Find(id);
            db.Quadroes.Remove(quadro);
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
