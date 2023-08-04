using System;
using System.Collections.Generic;

namespace DoctorAppointment1.Models;

public partial class Disease
{
    public int? DiseaseId { get; set; }

    public string DiseaseName { get; set; } = null!;

    public string? SuitableDoctor { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Doctor? SuitableDoctorNavigation { get; set; }
}
