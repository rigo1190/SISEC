﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public abstract class Generica
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? FechaCaptura { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? FechaModificacion { get; set; }

        [ScaffoldColumn(false)]
        public string UsuarioCaptura { get; set; }

        [ScaffoldColumn(false)]
        public string UsuarioModifica { get; set; }

    }
}
