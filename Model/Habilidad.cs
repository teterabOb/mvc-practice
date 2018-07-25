namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Habilidad")]
    public partial class Habilidad
    {
        public int id { get; set; }

        public int Usuario_id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        public int Dominio { get; set; }

        public virtual Usuario Usuario { get; set; }

        public Habilidad Obtener(int id)
        {
            var Habilidad = new Habilidad();

            try
            {
                using (var ctx = new ProyectoContext())
                {
                    Habilidad = ctx.Habilidad.Where(x => x.id == id)
                                             .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Habilidad;
        }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();

            try
            {
                using (var ctx = new ProyectoContext())
                {
                    if (this.id > 0) ctx.Entry(this).State = EntityState.Modified;
                    else ctx.Entry(this).State = EntityState.Added;

                    ctx.SaveChanges();
                    rm.SetResponse(true);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return rm;
        }

        public ResponseModel Eliminar(int id)
        {
            var rm = new ResponseModel();

            try
            {
                using (var ctx = new ProyectoContext())
                {
                    this.id = id;
                    ctx.Entry(this).State = EntityState.Deleted;

                    ctx.SaveChanges();
                    rm.SetResponse(true);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return rm;
        }

        public AnexGRIDResponde Listar(AnexGRID grid, int Usuario_id)
        {
            try
            {
                using (var ctx = new ProyectoContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    grid.Inicializar();

                    var query = ctx.Habilidad.Where(x => x.Usuario_id == Usuario_id);

                    // Ordenamiento
                    if (grid.columna == "id")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.id)
                                                             : query.OrderBy(x => x.id);
                    }

                    if (grid.columna == "Nombre")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Nombre)
                                                             : query.OrderBy(x => x.Nombre);
                    }

                    if (grid.columna == "Dominio")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Dominio)
                                                             : query.OrderBy(x => x.Dominio);
                    }
                    

                    var Habilidads = query.Skip(grid.pagina)
                                          .Take(grid.limite)
                                          .ToList();

                    var total = query.Count();

                    grid.SetData(Habilidads, total);
                }
            }
            catch (Exception E)
            {

                throw;
            }

            return grid.responde();
        }
    }
}
