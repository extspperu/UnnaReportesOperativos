using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Dtos;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Configuraciones.MenuUrls.Servicios.Implementaciones
{
    public class MenuUrlServicio: IMenuUrlServicio
    {

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IMenuUrlRepositorio _menuUrlRepositorio;
        private readonly IDiaOperativoServicio _diaOperativoServicio;
        public MenuUrlServicio(
            IUsuarioRepositorio usuarioRepositorio,
            IMenuUrlRepositorio menuUrlRepositorio,
            IDiaOperativoServicio diaOperativoServicio
            )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _menuUrlRepositorio = menuUrlRepositorio;
            _diaOperativoServicio = diaOperativoServicio;
        }

        public async Task<OperacionDto<List<MenuUrlAdminDto>>> ObtenerListaMenuUrl(long idUsuario)
        {
            var esAdministrador = false;
            var usuario = await _usuarioRepositorio.BuscarPorIdAsync(idUsuario);
            if (usuario == null)
            {
                return new OperacionDto<List<MenuUrlAdminDto>>(new List<MenuUrlAdminDto>());
            }

            if (usuario != null)
            {
                esAdministrador = usuario.EsAdministrador;
            }
            var menuUrl = await _menuUrlRepositorio.ListarPorGrupoAsync(usuario.IdGrupo, usuario.EsAdministrador);
            var dto = new List<MenuUrlAdminDto>();
            foreach (var item in menuUrl)
            {
                var dato = new MenuUrlAdminDto
                {
                    IdMenuUrl = item.IdMenuUrl,
                    Nombre = item.Nombre,
                    Icono = item.Icono,
                    Url = item.Url,
                    IdMenuUrlPadre = item.IdMenuUrlPadre,
                    Orden = item.Orden
                };
                dto.Add(dato);
            }

            var dtoFinal = new List<MenuUrlAdminDto>();
            var conUrls = dto.Where(e => !string.IsNullOrWhiteSpace(e.Url)).ToList();
            var idExistentes = conUrls.Select(e => e.IdMenuUrl).ToList();
            dtoFinal.AddRange(conUrls);

            foreach (var conUrl in conUrls)
            {
                AgregarPadres(dto, conUrl, dtoFinal, idExistentes);
            }


            switch (usuario.IdGrupo)
            {

                case (int)TipoGrupos.FiscalizadorRegular:

                    var operacionExiste = await _diaOperativoServicio.ExisteParaEdicionDatosAsync(idUsuario, (int)TipoGrupos.FiscalizadorRegular,null);
                    if (!operacionExiste)
                    {
                        dtoFinal = dtoFinal.Where(e => e.IdMenuUrl != (int)TiposMenuUrls.EdicionDatos).ToList();
                    }                    
                    break;
                
                case (int)TipoGrupos.FiscalizadorEnel:

                    List<int> operacionEnel = await _diaOperativoServicio.ListarNumeroEdicionFiscalizadorEnelAsync(idUsuario, (int)TipoGrupos.FiscalizadorEnel, null);
                    if (operacionEnel.Count == 0)
                    {
                        dtoFinal = dtoFinal.Where(e => e.IdMenuUrl != (int)TiposMenuUrls.EdicionDatosPrimerRegistroEnel).ToList();
                        dtoFinal = dtoFinal.Where(e => e.IdMenuUrl != (int)TiposMenuUrls.EdicionDatosSegundoRegistroEnel).ToList();
                    }
                    else
                    {
                        if(operacionEnel.Where(e => e == (int)TiposNumeroRegistro.PrimeroRegistro).ToList().Count() == 0)
                        {
                            dtoFinal = dtoFinal.Where(e => e.IdMenuUrl != (int)TiposMenuUrls.EdicionDatosPrimerRegistroEnel).ToList();
                        }
                        if (operacionEnel.Where(e => e == (int)TiposNumeroRegistro.SegundoRegistro).ToList().Count() == 0)
                        {
                            dtoFinal = dtoFinal.Where(e => e.IdMenuUrl != (int)TiposMenuUrls.EdicionDatosSegundoRegistroEnel).ToList();
                        }
                    }

                    
                    var fiscalizacionOperacion = await _diaOperativoServicio.ListarRegistrosFiscalizadorRegularAsync();
                    if (!fiscalizacionOperacion.Completado || fiscalizacionOperacion.Resultado == null || fiscalizacionOperacion.Resultado.Count == 0)
                    {
                        dtoFinal = dtoFinal.Where(e => e.IdMenuUrl != (int)TiposMenuUrls.DatosDeFiscalizacion).ToList();
                    }
                    break;


            }


            return new OperacionDto<List<MenuUrlAdminDto>>(dtoFinal);
        }

        private void AgregarPadres(List<MenuUrlAdminDto> todos, MenuUrlAdminDto actual, List<MenuUrlAdminDto> final, List<long> idExistentes)
        {
            if (!actual.IdMenuUrlPadre.HasValue)
            {
                return;
            }
            var padre = todos.Where(e => e.IdMenuUrl == actual.IdMenuUrlPadre).FirstOrDefault();
            if (padre == null)
            {
                return;
            }
            else
            {
                if (!idExistentes.Contains(padre.IdMenuUrl))
                {
                    final.Add(padre);
                    idExistentes.Add(padre.IdMenuUrl);
                }
                AgregarPadres(todos, padre, final, idExistentes);
            }

        }


    }
}
