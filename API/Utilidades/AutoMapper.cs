using AutoMapper;
using FutbolAPI.DTOs;
using FutbolAPI.Entidades;

namespace FutbolAPI.Utilidades
{
    public class AutoMapper: Profile
    {

        public AutoMapper()
        {
            configurarMapeoEquipos();
            configurarMapeoCanchas();
            configurarMapeoFechasYhoras();
        }

        private void configurarMapeoEquipos()
        {
            CreateMap<EquipoCreacionDTO, Equipo>();
            CreateMap<Equipo, EquipoDTO>(); 
        }
        private void configurarMapeoCanchas()
        {
            CreateMap<CanchaCreacionDTO, Cancha>();
            CreateMap<Cancha, CanchaDTO>(); 
        }

        private void configurarMapeoFechasYhoras()
        {
            CreateMap<FechaYhoraCeacionDTO, FechaYhora>()
                .ForMember(dest => dest.id, opt => opt.Ignore()); //  aseguramos que nunca intente mapear el Id

            CreateMap<FechaYhora, FechaYhoraDTO>()
                .ForMember(dest => dest.TipoCancha,
                opt => opt.MapFrom(src => src.Cancha.TipoCancha))
                .ForMember(dest => dest.NumeroCancha,
                opt => opt.MapFrom(src => src.Cancha.NumeroCancha))
                .ForMember(e => e.NombreEquipo,
                opt => opt.MapFrom(src => src.Equipo.NombreEquipo));
        }

    }
}
