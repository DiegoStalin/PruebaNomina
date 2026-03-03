using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAppNomina.Models;
using WebAppNomina.DAL;
using System.Data.Entity;

namespace WebAppNomina.Controllers
{
    public class SalarioController : Controller
    {•using System;
2
using System.Collections.Generic;
3
using System.Linq;
 |  | 
4
 
5
 
using System.Web.Mvc;
6
 
using WebAppNomina.Models;
7
 
using WebAppNomina.DAL;
8
 
using System.Data.Entity;
9
 
13
​
14
namespace WebAppNomina.Controllers
15
{
16
    public class SalarioController : Controller
17
    {
 |  | 
18
 
19
 
        private NominaContext db = new NominaContext();
20
 
​
21
 
        public ActionResult Index(DateTime? fechaVigencia)
22
 
        {
23
 
            var query = db.Salarios.Include(s => s.Empleado).AsQueryable();
24
 
            if (fechaVigencia.HasValue)
25
 
            {
26
 
                query = query.Where(s => fechaVigencia >= s.from_date &&
27
 
                                   (s.to_date == null || fechaVigencia <= s.to_date));
28
 
            }
29
 
            return View(query.ToList());
30
 
        }
31
 
​
32
 
        public ActionResult Auditoria()
33
 
        {
34
 
            var historial = db.Auditorias
35
 
                              .Include(a => a.Empleado)
36
 
                              .OrderByDescending(a => a.fechaActualizacion)
37
 
                              .ToList();
38
 
            return View(historial);
39
 
        }
40
 
​
41
 
        // --- REPORTES CORREGIDOS (RF-09) ---
42
 
​
43
 
        public ActionResult ReporteNominaDepto()
44
 
        {
45
 
            // Se quitó .Include("Asignaciones") para evitar InvalidOperationException
46
 
            var departamentos = db.Departamentos.ToList();
47
 
            return View(departamentos);
48
 
        }
49
 
​
50
 
        public ActionResult EstructuraOrg()
51
 
        {
52
 
            // Se quitó .Include() para evitar errores de ruta no válida
53
 
            var estructura = db.Departamentos.ToList();
54
 
            return View(estructura);
55
 
        }
56
 
​
57
 
        // Métodos adicionales (Create, Delete, CargarEmpleados, Dispose) se mantienen igual
58
 
        public ActionResult Create() { CargarEmpleados(); return View(); }
59
 
        [HttpPost]
60
 
        [ValidateAntiForgeryToken]
61
 
        public ActionResult Create(Salario model)
62
 
        {
63
 
            if (ModelState.IsValid) { db.Salarios.Add(model); db.SaveChanges(); return RedirectToAction("Index"); }
        private NominaContext db = new NominaContext();

        public ActionResult Index(DateTime? fechaVigencia)
        {
            var query = db.Salarios.Include(s => s.Empleado).AsQueryable();
            if (fechaVigencia.HasValue)
            {
                query = query.Where(s => fechaVigencia >= s.from_date &&
                                   (s.to_date == null || fechaVigencia <= s.to_date));
            }
            return View(query.ToList());
        }

        public ActionResult Auditoria()
        {
            var historial = db.Auditorias
                              .Include(a => a.Empleado)
                              .OrderByDescending(a => a.fechaActualizacion)
                              .ToList();
            return View(historial);
        }

        // --- REPORTES CORREGIDOS (RF-09) ---

        public ActionResult ReporteNominaDepto()
        {
            // Se quitó .Include("Asignaciones") para evitar InvalidOperationException
            var departamentos = db.Departamentos.ToList();
            return View(departamentos);
        }

        public ActionResult EstructuraOrg()
        {
            // Se quitó .Include() para evitar errores de ruta no válida
            var estructura = db.Departamentos.ToList();
            return View(estructura);
        }

        // Métodos adicionales (Create, Delete, CargarEmpleados, Dispose) se mantienen igual
        public ActionResult Create() { CargarEmpleados(); return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Salario model)
        {
            if (ModelState.IsValid) { db.Salarios.Add(model); db.SaveChanges(); return RedirectToAction("Index"); }
            CargarEmpleados(); return View(model);
        }
        private void CargarEmpleados() { ViewBag.emp_no = new SelectList(db.Empleados, "emp_no", "first_name"); }
        protected override void Dispose(bool disposing) { if (disposing) { db.Dispose(); } base.Dispose(disposing); }
    }
}
