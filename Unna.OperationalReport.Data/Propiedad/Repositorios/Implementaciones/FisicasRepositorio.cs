using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Propiedad.Entidades;
using Unna.OperationalReport.Data.Propiedad.Procedimientos;
using Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Procedimientos;

namespace Unna.OperationalReport.Data.Propiedad.Repositorios.Implementaciones
{
    public class FisicasRepositorio : OperacionalRepositorio<Fisicas, object>, IFisicasRepositorio
    {
        public FisicasRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<ListarPropiedadesFisicas>?> ListarPropiedadesFisicasAsync(string? grupo,DateTime diaOperativo)
        {
            List<ListarPropiedadesFisicas>? lista = new List<ListarPropiedadesFisicas>();
            var sql = "SELECT  tbl2.Id,tbl1.Grupo,tbl2.Suministrador,tbl1.PoderCalorifico,tbl1.RelacionVolumen,tbl1.PesoMolecular,tbl1.Densidad DensidadLiquido,tbl1.PoderCalorificoBruto,tbl2.Suministrador Componente " +
                "FROM Propiedad.FisicasGpsa_2 tbl1 INNER JOIN Propiedad.SuministradorComponente tbl2 ON tbl1.IdSuministradorComponente = tbl2.Id INNER JOIN [Reporte].[RegistroSupervisor] C ON tbl1.IdRegistroSupervisor=C.IdRegistroSupervisor " +
                "WHERE tbl1.Grupo=@Grupo and cast(C.Fecha as date)=CAST((CAST((YEAR(CAST(@DiaOperativo AS DATE)) * 100) + MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + CASE WHEN DAY(CAST(@DiaOperativo AS DATE))<10 THEN  '0' + CAST(DAY(CAST(@DiaOperativo AS DATE)) AS VARCHAR(2)) ELSE CAST(DAY(CAST(@DiaOperativo AS DATE)) AS VARCHAR(2)) END )   AS DATE) order by 1";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ListarPropiedadesFisicas>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        Grupo = grupo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<CantidadCalidadVolumenGnaLoteIv>?> ListarCantidadCalidadVolumenGnaLoteIvAsync(DateTime fecha)
        {
            List<CantidadCalidadVolumenGnaLoteIv>? lista = new List<CantidadCalidadVolumenGnaLoteIv>();
            var sql = "Reporte.CantidadCalidadVolumenGnaLoteIv";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<CantidadCalidadVolumenGnaLoteIv>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = fecha
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


        public async Task<List<ComponentesComposicionGna>?> ListarFactorLoteIvAsync(DateTime fecha)
        {
            List<ComponentesComposicionGna>? lista = new List<ComponentesComposicionGna>();
            var sql = "Reporte.BuscarFactorLoteIv";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ComponentesComposicionGna>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = fecha
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }



        public async Task<List<VolumenGasNaturalPorTipoLoteIv>?> ListarVolumenGasNaturalPorTipoLoteIvAsync(DateTime fecha)
        {
            List<VolumenGasNaturalPorTipoLoteIv>? lista = new List<VolumenGasNaturalPorTipoLoteIv>();
            var sql = "Reporte.VolumenGasNaturalPorTipoLoteIv";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<VolumenGasNaturalPorTipoLoteIv>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = fecha
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


        public async Task<List<BuscarSuministradorComponente>?> ListarSuministradorComponenteAsync(int idLote,DateTime diaOperativo)
        {
            List<BuscarSuministradorComponente>? lista = new List<BuscarSuministradorComponente>();
            var sql = "Propiedad.ComposicionGnaEntrada";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<BuscarSuministradorComponente>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        IdLote = idLote,
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }
        public async Task<List<ListarPropiedadesFisicas>?> FisicasPorGrupoAsync(string grupo)
        {
            List<ListarPropiedadesFisicas>? lista = new List<ListarPropiedadesFisicas>();
            var sql = "Propiedad.FisicasPorGrupo";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ListarPropiedadesFisicas>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Grupo = grupo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


    }
}
