using API.Dominio;
using APIEscolar.DTOs;
using AutoMapper;

namespace APIEscolar.Mapper
{
    public class EscolarMapper:Profile
    {
        public EscolarMapper()
        {
            CreateMap<PeriodoEscolar, PeriodoEscolarVM>().ReverseMap();
            CreateMap<PeriodoEscolar, PeriodosCreacionVM>().ReverseMap();
            CreateMap<Carreras, CarrerasVM>().ReverseMap();
            CreateMap<Carreras, CarrerasCrearVM>().ReverseMap();
            CreateMap<Materias, MateriasVM>().ReverseMap();
            CreateMap<Materias, MateriasCrearVM>().ReverseMap();
            CreateMap<Estudiantes, EstudiantesVM>().ReverseMap();
            CreateMap<Estudiantes, EstudiantesCreacionVM>().ReverseMap();
        }
    }
}
