using System;
using System.Collections.Generic;

namespace DoctorAppointment1.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public string? PatientName { get; set; }

    public string? MedicalIssue { get; set; }

    public string? DoctorToVisit { get; set; }

    public TimeSpan? DoctorAvailableTime { get; set; }

    public DateTime? AppointmentTime { get; set; }

    public long? PatientPhoneNumber { get; set; }

    public string? PatientEmail { get; set; }

    public string? AppointmentStatus { get; set; }

    public virtual Doctor? DoctorToVisitNavigation { get; set; }

    public virtual Disease? MedicalIssueNavigation { get; set; }
}
