namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
        using System.Linq;

    [Table("Experiencia")]
    public partial class Experiencia
    {
        public int id { get; set; }

        public int Usuario_id { get; set; }

        public byte Tipo { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(10)]
        public string Desde { get; set; }

        [StringLength(10)]
        public string Hasta { get; set; }

        [Column(TypeName = "text")]
        public string Descripcion { get; set; }

        public virtual Usuario Usuario { get; set; }

        public Experiencia Obtener(int id)
        {
            var experiencia = new Experiencia();

            try
            {
                using (var ctx = new ProyectoContext())
                {
                    experiencia = ctx.Experiencia.Where(x => x.id == id)
                                                 .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return experiencia;
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
            catch (Exception e)
            {
                throw;
            }

            return rm;
        }

        public AnexGRIDResponde Listar(AnexGRID grid, int tipo, int Usuario_id)
        {
            try
            {
                using (var ctx = new ProyectoContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    grid.Inicializar();
                    
                    var query = ctx.Experiencia.Where(x => x.Tipo == tipo)
                                               .Where(x => x.Usuario_id == Usuario_id);

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

                    if (grid.columna == "Titulo")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Titulo)
                                                             : query.OrderBy(x => x.Titulo);
                    }

                    if (grid.columna == "Desde")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Desde)
                                                             : query.OrderBy(x => x.Desde);
                    }

                    if (grid.columna == "Hasta")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Hasta)
                                                             : query.OrderBy(x => x.id);
                    }

                    // id, Nombre, Titulo, Desde, Hasta

                    var experiencias = query.Skip(grid.pagina)
                                            .Take(grid.limite)
                                             .ToList();

                    var total = query.Count();

                    grid.SetData(experiencias, total);
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
