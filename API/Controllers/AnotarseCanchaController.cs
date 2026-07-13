using AutoMapper;
using AutoMapper.QueryableExtensions;
using FutbolAPI.DTOs;
using FutbolAPI.Entidades;
using FutbolAPI.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FutbolAPI.Controllers
{
    
    [Route("api/anotarse_Cancha")]
    public class AnotarseCanchaController: ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly AplicationDBContext context;
        private readonly IMapper mapper;
        private const string cacheTag = "Canchas";

        public AnotarseCanchaController(IOutputCacheStore outputCacheStore, AplicationDBContext context,IMapper mapper)
        {
            this.outputCacheStore = outputCacheStore;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet(Name = "ObtenerTodosCanchas")]
        [OutputCache(Tags = [cacheTag])]

        public async Task<List<CanchaDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            return await context.Equipos.ProjectTo<CanchaDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("{id}", Name = "ObtenerPorCanchaId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<CanchaDTO>> Get(int id)
        {
            var cancha = await context.Canchas
               .ProjectTo<CanchaDTO>(mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(e => e.id == id);

            if (cancha is null)
            {
                return NotFound();
            }

            return cancha;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CanchaCreacionDTO canchaCreacionDTO)
        {
            var cancha = mapper.Map<Cancha>(canchaCreacionDTO);
            context.Add(cancha);
            await context.SaveChangesAsync();
            return CreatedAtRoute("ObtenerPorCanchaId", new { id = cancha.id}, cancha);
        }

        [HttpDelete]
        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
