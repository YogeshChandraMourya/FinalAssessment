using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoctorAppointment1.Models;
using Microsoft.AspNetCore.Authorization;

namespace DoctorAppointment1.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly DoctorContext _context;

        public AppointmentsController(DoctorContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var doctorContext = _context.Appointments.Include(a => a.DoctorToVisitNavigation).Include(a => a.MedicalIssueNavigation);
            return View(await doctorContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.DoctorToVisitNavigation)
                .Include(a => a.MedicalIssueNavigation)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            
            ViewData["MedicalIssue"] = new SelectList(_context.Diseases, "DiseaseName", "DiseaseName");
            return View();
        }
        [HttpPost] 
        public IActionResult GetData(IFormCollection data)
        {
            string disease = data["Disease"];
            string doctorName = _context.Diseases.FirstOrDefault(d => d.DiseaseName.Equals(disease)).SuitableDoctor;
            Doctor doctor = _context.Doctors.FirstOrDefault(d => d.Name == doctorName);
            return Json(new { doctorName = doctor.Name, doctorTime=doctor.AvailableTime });
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,PatientName,MedicalIssue,DoctorToVisit,DoctorAvailableTime,AppointmentTime,PatientPhoneNumber,PatientEmail,AppointmentStatus")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorToVisit"] = new SelectList(_context.Doctors, "Name", "Name", appointment.DoctorToVisit);
            ViewData["MedicalIssue"] = new SelectList(_context.Diseases, "DiseaseName", "DiseaseName", appointment.MedicalIssue);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorToVisit"] = new SelectList(_context.Doctors, "Name", "Name", appointment.DoctorToVisit);
            ViewData["MedicalIssue"] = new SelectList(_context.Diseases, "DiseaseName", "DiseaseName", appointment.MedicalIssue);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,PatientName,MedicalIssue,DoctorToVisit,DoctorAvailableTime,AppointmentTime,PatientPhoneNumber,PatientEmail,AppointmentStatus")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId))
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
            ViewData["DoctorToVisit"] = new SelectList(_context.Doctors, "Name", "Name", appointment.DoctorToVisit);
            ViewData["MedicalIssue"] = new SelectList(_context.Diseases, "DiseaseName", "DiseaseName", appointment.MedicalIssue);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.DoctorToVisitNavigation)
                .Include(a => a.MedicalIssueNavigation)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointments == null)
            {
                return Problem("Entity set 'DoctorContext.Appointments'  is null.");
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
          return (_context.Appointments?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
        }

        //********************************************************************************************
        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the username is already taken
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View(user);
                }

                // Hash and salt the password before storing in the database
                // For password hashing, you should use a secure method like bcrypt or Argon2
                // Here's a basic example using a simple hashing algorithm (do not use this in production)
                user.Password = HashPassword(user.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Account");
            }
            return View(user);
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        public User? GetDbUser(User user)
        {
            return _context.Users.FirstOrDefault(u => u.Username == user.Username);
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user, User? dbUser)
        {

            // Check the password (you should compare hashed passwords here)
            // For production use, do not compare passwords like this. Use a secure password hashing method.
            if (dbUser != null && dbUser.Password == user.Password)
            {
                // Handle successful login (e.g., set authentication cookie)
                // For demonstration purposes, I'll just redirect to a dashboard page
                return RedirectToAction("Login", "Appointments");
            }
            else
            {
                ModelState.AddModelError("Password", "Invalid username or password.");
                return View(user);
            }
        }

        // Helper method to hash the password (not suitable for production, use a secure password hashing method)
        private string HashPassword(string password)
        {
            return password; // Replace this with a secure hashing algorithm
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }



}

