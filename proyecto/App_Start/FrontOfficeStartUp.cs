﻿using System;
using System.Web;

namespace proyecto.App_Start
{
    public class FrontOfficeStartUp
    {
        public static int UsuarioVisualizando()
        {
            int usuario_id_por_defecto = 3;
            string usuario_id = HttpContext.Current.Request.QueryString["id"];

            return usuario_id != null ? Convert.ToInt32(usuario_id) : usuario_id_por_defecto;
        }
    }
}