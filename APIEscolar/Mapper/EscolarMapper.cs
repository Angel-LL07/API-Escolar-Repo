using API.Dominio;
using APIEscolar.DTOs;
using AutoMapper;

namespace APIEscolar.Mapper
{
    public class EscolarMapper:Profile
    {
        public EscolarMapper()
        {
                CreateMap<PeriodoEscolar,PeriodoEscolarVM>().ReverseMap();
                CreateMap<PeriodoEscolar, PeriodosCreacionVM>().ReverseMap();
        }
    }
}
