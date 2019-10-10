﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetApi.Models
{
    /// <summary>
    /// Speedrun information
    /// </summary>
    public class Run : IRun
    {
        public Guid Id { get; set; }
        public float TimeTaken { get; set; }
        public DateTime TimePosted { get; set; }
        public string Level { get; set; }

        public Run()
        {
            Id = Guid.NewGuid();
            TimePosted = DateTime.UtcNow;
        }

        public Run(NewRun newRun)
        {
            Id = Guid.NewGuid();
            TimeTaken = newRun.TimeTaken;
            TimePosted = newRun.TimePosted;
            Level = newRun.Level;
        }
    }
}
