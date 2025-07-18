﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class UserAnswer
{
    public int AnswerId { get; set; }

    public int? CourseId { get; set; }

    public int? UserId { get; set; }

    public int? QuestionId { get; set; }

    public int? OptionId { get; set; }

    public DateTime? AnswerAt { get; set; }

    public int? TotalPoint { get; set; }

    public virtual Course Course { get; set; }

    public virtual CourseQuestionOption Option { get; set; }

    public virtual CourseQuestion Question { get; set; }

    public virtual User User { get; set; }
}