using organizacion_personal.modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace organizacion_personal.modelos
{
    public class ApiResponse_
    {
        public string Mensaje { get; set; }
        public actividad_diaria actividadDiaria { get; set; }
    }
}
