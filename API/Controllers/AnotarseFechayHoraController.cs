using AutoMapper;
using AutoMapper.QueryableExtensions;
using FutbolAPI.DTOs;
using FutbolAPI.Entidades;
using FutbolAPI.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FutbolAPI.Controllers
{
    
    [Route("api/anotarse_FechayHora")]
    public class AnotarseFechayHoraController: ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly AplicationDBContext context;
        private readonly IMapper mapper;
        private const string cacheTag = "FechaYhora";
        public AnotarseFechayHoraController(IOutputCacheStore outputCacheStore, AplicationDBContext context, IMapper mapper)
        {
            this.outputCacheStore = outputCacheStore;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet (Name = "ObtenerTodosFechayHoraCancha")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<FechaYhoraDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = context.Fechas_y_horas.Include(f => f.Cancha)
                .Include(e => e.Equipo);
            await HttpContext.InsertarParametrosPaginacion(queryable);
            return await queryable
                .OrderBy(f => f.FechaCancha)
                .Paginar(paginacion)
                .ProjectTo<FechaYhoraDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("{id}", Name = "ObtenerPorFechaId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<FechaYhoraDTO>> Get(int id)
        {
            var fechaYhora = await context.Fechas_y_horas
               .ProjectTo<FechaYhoraDTO>(mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(e => e.id == id);

            if (fechaYhora is null)
            {
                return NotFound();
            }

            return fechaYhora;
        }


        //Disponibilidad por fecha, cancha y tipo

        [HttpGet("disponibilidad")]
        public IActionResult ObtenerDisponibilidad(
    [FromQuery] string fecha,
    [FromQuery] string cancha,
    [FromQuery] string tipoCancha)
        {
            if (string.IsNullOrEmpty(fecha))
                return BadRequest("Debe enviar una fecha válida.");

            // Intentar parsear la fecha enviada
            if (!DateTime.TryParse(fecha, out DateTime fechaDT))
                return BadRequest("Formato de fecha inválido. Use YYYY-MM-DD.");

            // Traer reservas de manera segura
            var reservas = context.Fechas_y_horas
                .Include(f => f.Cancha)
                .AsEnumerable() // pasa a memoria para poder usar TryParse
                .Where(f =>
                    DateTime.TryParse(f.FechaCancha, out DateTime fDate) &&
                    fDate.Date == fechaDT.Date &&
                    f.Cancha.NumeroCancha.Trim() == cancha.Trim() &&
                    f.Cancha.TipoCancha.Trim() == tipoCancha.Trim()
                )
                .Select(f => f.HoraCancha)
                .ToList(); // List<string>, no await necesario

            // Devolver resultado
            return Ok(reservas);
        }






        // Validación antes de guardar reserva (por seguridad extra)

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FechaYhoraCeacionDTO fechaYhoraCeacionDTO)
        {
            // Verificar si ya existe una reserva igual
            bool existe = await context.Fechas_y_horas.AnyAsync(f =>
                f.FechaCancha == fechaYhoraCeacionDTO.FechaCancha &&
                f.HoraCancha == fechaYhoraCeacionDTO.HoraCancha &&
                f.CanchaId == fechaYhoraCeacionDTO.CanchaId);

            if (existe)
            {
                return Conflict("Esta fecha, hora y cancha ya están reservadas.");
            }

            var fechaYhora = mapper.Map<FechaYhora>(fechaYhoraCeacionDTO);
            context.Add(fechaYhora);
            await context.SaveChangesAsync();

            return CreatedAtRoute("ObtenerPorFechaId", new { id = fechaYhora.id }, fechaYhora);
        }


        [HttpPost("{id}/equipo-rival")]
        public async Task<IActionResult> CrearYAsignarRival(int id, [FromBody] GuardarYAgregarEquipoRivarDTO guardarYAgregarEquipoRivarDTO)
        {
            var equipo = await context.Fechas_y_horas
                .Include(f => f.Equipo) // Traemos el equipo principal para obtener el email
                .Include(f => f.Cancha) // traemos el objeto cancha
                .FirstOrDefaultAsync(f => f.id == id);

            if (equipo == null)
            {
                return NotFound();
            }
               

            //  Crear el equipo rival
            var nuevoEquipo = new Equipo
            {
                NombreEquipo = guardarYAgregarEquipoRivarDTO.NombreEquipo,
                Responsable = guardarYAgregarEquipoRivarDTO.Responsable,
                Email = guardarYAgregarEquipoRivarDTO.Email
            };
            context.Equipos.Add(nuevoEquipo);
            await context.SaveChangesAsync();

            //  Asignar como rival
            equipo.EquipoRivalId = nuevoEquipo.id;
            await context.SaveChangesAsync();

            DateTime fechaDt = DateTime.Parse(equipo.FechaCancha);
            string fechaFormateada = fechaDt.ToString("dd/MM/yyyy");

            //  Enviar email al equipo principal
            var email = equipo.Equipo.Email;
            var subject = "Partido confirmado";
            var body = $"Hola {equipo.Equipo.Responsable},\n\nSu partido contra {nuevoEquipo.NombreEquipo} ha sido confirmado en la cancha {equipo.Cancha.NumeroCancha} el día {fechaFormateada} a las {equipo.HoraCancha}.";

            await EnviarEmailAsync(email, subject, body);

            return NoContent();
        }

        private async Task EnviarEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, // puerto seguro con TLS
                Credentials = new NetworkCredential("marianohumhofe@gmail.com", "ucdj apuz eafl cctq"),  
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("marianohumhofe@gmail.com", "Futbol App"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var registroBorrado = await context.Fechas_y_horas.Where(f => f.id == id).ExecuteDeleteAsync();

            if (registroBorrado == 0)
            {
                return NotFound();
            }

            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();

        }

        [HttpDelete("{id:int}/Cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {

            var equipo = await context.Fechas_y_horas
               .Include(f => f.Equipo) // Traemos el equipo principal para obtener el email
               .Include(f => f.Cancha) // traemos el objeto cancha
               .Include(f => f.EquipoRival) // agregamos el equipo rival.
               .FirstOrDefaultAsync(f => f.id == id);

            if (equipo == null)
            {
                return NotFound();
            }

            DateTime fechaDt = DateTime.Parse(equipo.FechaCancha);
            string fechaFormateada = fechaDt.ToString("dd/MM/yyyy");

            var email = equipo.Equipo.Email;
            var subject = "Lo sentimos su partido fue Cancelado";
            var body = $"Hola {equipo.Equipo.Responsable},\n\nSu partido contra {equipo.EquipoRival!.NombreEquipo} en la cancha {equipo.Cancha.NumeroCancha} el día {fechaFormateada} a las {equipo.HoraCancha} fue cancelado. Vuelva a registrar el equipo en el formulario para poder buscar otro rival.";

            await EnviarEmailAsync(email, subject, body);

            var registroBorrado = await context.Fechas_y_horas.Where(f => f.id == id).ExecuteDeleteAsync();

            if (registroBorrado == 0)
            {
                return NotFound();
            }

            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }
    }
}
