using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones
{
    public interface IMenuUrlRepositorio:IOperacionalRepositorio<MenuUrl, long>
    {
        Task<List<MenuUrl>> ListarPorGrupoAsync(int? idGrupo, bool? EsParaAdmin);
    }
}
