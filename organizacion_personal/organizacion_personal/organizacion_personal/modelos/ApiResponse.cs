using organizacion_personal.modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace organizacion_personal.modelos
{
    public class ApiResponse
    {
        public string Mensaje { get; set; }
        public List<actividad_diaria> actividades_diarias { get; set; }
        public actividad_diaria actividad_diaria { get; set; }
    }
}
