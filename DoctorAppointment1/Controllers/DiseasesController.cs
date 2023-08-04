using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoctorAppointment1.Models;

namespace DoctorAppointment1.Controllers
{
    public class DiseasesController : Controller
    {
        private readonly DoctorContext _context;

        public DiseasesController(DoctorContext context)
        {
            _context = context;
        }

        // GET: Diseases
        public async Task<IActionResult> Index()
        {
            var doctorContext = _context.Diseases.Include(d => d.SuitableDoctorNavigation);
            return View(await doctorContext.ToListAsync());
        }

        // GET: Diseases/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Diseases == null)
            {
                return NotFound();
            }

            var disease = await _context.Diseases
                .Include(d => d.SuitableDoctorNavigation)
                .FirstOrDefaultAsync(m => m.DiseaseName == id);
            if (disease == null)
            {
                return NotFound();
            }

            return View(disease);
        }

        // GET: Diseases/Create
        public IActionResult Create()
        {
            ViewData["SuitableDoctor"] = new SelectList(_context.Doctors, "Name", "Name");
            return View();
        }

        // POST: Diseases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiseaseId,DiseaseName,SuitableDoctor")] Disease disease)
        {
            if (ModelState.IsValid)
            {
                _context.Add(disease);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SuitableDoctor"] = new SelectList(_context.Doctors, "Name", "Name", disease.SuitableDoctor);
            return View(disease);
        }

        // GET: Diseases/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Diseases == null)
            {
                return NotFound();
            }

            var disease = await _context.Diseases.FindAsync(id);
            if (disease == null)
            {
                return NotFound();
            }
            ViewData["SuitableDoctor"] = new SelectList(_context.Doctors, "Name", "Name", disease.SuitableDoctor);
            return View(disease);
        }

        // POST: Diseases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DiseaseId,DiseaseName,SuitableDoctor")] Disease disease)
        {
            if (id != disease.DiseaseName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disease);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiseaseExists(disease.DiseaseName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SuitableDoctor"] = new SelectList(_context.Doctors, "Name", "Name", disease.SuitableDoctor);
            return View(disease);
        }

        // GET: Diseases/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Diseases == null)
            {
                return NotFound();
            }

            var disease = await _context.Diseases
                .Include(d => d.SuitableDoctorNavigation)
                .FirstOrDefaultAsync(m => m.DiseaseName == id);
            if (disease == null)
            {
                return NotFound();
            }

            return View(disease);
        }

        // POST: Diseases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Diseases == null)
            {
                return Problem("Entity set 'DoctorContext.Diseases'  is null.");
            }
            var disease = await _context.Diseases.FindAsync(id);
            if (disease != null)
            {
                _context.Diseases.Remove(disease);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiseaseExists(string id)
        {
          return (_context.Diseases?.Any(e => e.DiseaseName == id)).GetValueOrDefault();
        }
    }
}
