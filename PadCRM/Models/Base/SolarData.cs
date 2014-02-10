using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadCRM.Models
{
    public class SolarData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int yearid { get; set; }

        public string data { get; set; }

        public int dataint { get; set; }
    }
}