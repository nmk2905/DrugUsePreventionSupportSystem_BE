﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Consultant
{
    public int Number { get; set; }

    public int ConsultantId { get; set; }

    public string Specification { get; set; }

    public string Qualifications { get; set; }

    public int ExperienceYears { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual User ConsultantNavigation { get; set; }

    public virtual ICollection<ConsultantsAvailability> ConsultantsAvailabilities { get; set; } = new List<ConsultantsAvailability>();
}