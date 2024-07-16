using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Auth.Repositorios.Implementaciones;
using Unna.OperationalReport.Service.Auth.Grupos.Dtos;
using Unna.OperationalReport.Service.Auth.Grupos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Usuarios.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Auth.Grupos.Servicios.Implementaciones
{
    public class GrupoServicio: IGrupoServicio
    {
        private readonly IGrupoRepositorio _grupoRepositorio;

        public GrupoServicio(IGrupoRepositorio grupoRepositorio)
        {
            _grupoRepositorio = grupoRepositorio;
        }


        public async Task<OperacionDto<List<GrupoDto>>> ListarAsync()
        {
            var usuarios = await _grupoRepositorio.ListarAsync();
            if (usuarios == null)
            {
                return new OperacionDto<List<GrupoDto>>(CodigosOperacionDto.UsuarioIncorrecto, "Usuario no existe");
            }
            var dto = usuarios.Select(e=> new GrupoDto
            {
                Id = RijndaelUtilitario.EncryptRijndaelToUrl(e.IdGrupo),
                Nombre = e.Nombre,
                UrlDefecto = e.UrlDefecto
            }).ToList();
            return new OperacionDto<List<GrupoDto>>(dto);
        }
    }
}
