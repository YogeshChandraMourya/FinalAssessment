using System;
using System.Collections.Generic;

namespace DoctorAppointment1.Models;

public partial class Doctor
{
    public int? DoctorId { get; set; }

    public string Name { get; set; } = null!;

    public string? Specialized { get; set; }

    public string? Qualification { get; set; }

    public TimeSpan? AvailableTime { get; set; }

    public long? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Disease> Diseases { get; set; } = new List<Disease>();
}
