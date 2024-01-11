using System;
using System.Collections.Generic;
using System.Text;

namespace organizacion_personal.modelos
{
    public class actividad_diaria
    {
        public int ad_secuencial { get; set; }
        public String ad_descripcion { get; set; }
        public String ad_hora_inicio { get; set; }
        public String ad_hora_fin { get; set; }
        public String ad_dia { get; set; }
        public String ad_usuario { get; set; }
        public String ad_estado { get; set; }
        public String ad_fecha_bd { get; set; }

    }
}
